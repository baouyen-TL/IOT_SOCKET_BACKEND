using Core.Constant;
using Core.Exceptions;
using Core.Extensions;
using Masterdata.Application.Features.V1.DTOs.MQTT;
using Masterdata.Application.Features.V1.Queries.Remote;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Masterdata.Application.Features.V1.Services
{
    public class MQTTService : IHostedService
    {
        private MqttClient _mqttClient;
        private readonly IHubContext<ChathubService> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        //private readonly IRemoteQuery _remoteQuery;

        public MQTTService(IHubContext<ChathubService> hubContext, /*IRemoteQuery remoteQuery, */IServiceScopeFactory query)
        {
            _hubContext = hubContext;
            //_remoteQuery = remoteQuery;
            _serviceScopeFactory = query;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var _configManager = new ConfigManager();
            var brokerAddress = _configManager.BrokerAddress;
            var port = _configManager.Port;
            var username = _configManager.UserName;
            var password = _configManager.Password;
            try
            {
                _mqttClient = new MqttClient(brokerAddress, port, true, MqttSslProtocols.TLSv1_2, null, null);
                _mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;

                string clientId = Guid.NewGuid().ToString();
                _mqttClient.Connect(clientId, username, password);

                _mqttClient.Subscribe(new string[]
                    { 
                        ConstTopicConnect.ConnectWifi,
                        ConstTopicConnect.ChooseAnswer,
                        ConstTopicConnect.NextQuestion
                    }, 
                    new byte[] {
                        MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                        MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                        MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE
                    });

                Console.WriteLine("Connected to MQTT broker and subscribed to topic 'socket/info'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private async void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            #region Cũ
            // Kết nối thiết bị
            //if (e.Topic == ConstTopicConnect.ConnectWifi)
            //{
            //    var response = Encoding.UTF8.GetString(e.Message);
            //    var MQTT = JsonConvert.DeserializeObject<MQTTResponse>(response);
            //    // ClientId là RemoteName
            //    //await _remoteQuery.UpdateStatusRemoteByRemoteName(MQTT.ClientId);
            //}

            //// Nhận đáp án từ thiết bị
            //if(e.Topic == ConstTopicConnect.ChooseAnswer)
            //{
            //    string response = Encoding.UTF8.GetString(e.Message);
            //    var MQTT = JsonConvert.DeserializeObject<MQTTResponse>(response);

            //    _hubContext.Clients.All.SendAsync("ReceiveMessage", MQTT.ClientId ?? "", MQTT.Msg);
            //}
            #endregion
            #region Mới
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var remoteQuery = scope.ServiceProvider.GetRequiredService<IRemoteQuery>();

                    var message = Encoding.UTF8.GetString(e.Message);
                    var mqttResponse = JsonConvert.DeserializeObject<Dictionary<string,string>>(message);
                    string ClientValue = mqttResponse.Values.FirstOrDefault();

                    string ClientKey = mqttResponse.Keys.FirstOrDefault();

                    switch (e.Topic)
                    {
                        case ConstTopicConnect.ConnectWifi:
                            await remoteQuery.UpdateStatusRemoteByRemoteName(ClientValue);
                            break;

                        case ConstTopicConnect.ChooseAnswer:
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", ClientKey ?? "", ClientValue);
                            break;

                        case ConstTopicConnect.NextQuestion:
                            Console.WriteLine("Received message on 'NextQuestion': " + message);
                            break;

                        default:
                            Console.WriteLine($"Unknown topic: {e.Topic}, message: {message}");
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new BadRequestException("Lỗi convert json to object!");
            }
            #endregion
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _mqttClient.Disconnect();
            return Task.CompletedTask;
        }
    }
}
