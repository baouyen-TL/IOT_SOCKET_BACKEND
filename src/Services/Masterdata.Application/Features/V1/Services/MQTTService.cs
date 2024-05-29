using Core.Constant;
using Core.Extensions;
using Masterdata.Application.Features.V1.DTOs.MQTT;
using Masterdata.Application.Features.V1.Queries.Remote;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
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

namespace Masterdata.Application.Features.V1.Services
{
    public class MQTTService : IHostedService
    {
        private MqttClient _mqttClient;
        private readonly string brokerAddress = "c805b8e62c2044939cc9ebcada8a35ee.s1.eu.hivemq.cloud";
        private readonly int port = 8883;
        private readonly string username = "vinhvq";
        private readonly string password = "Voquangvinh2552001";
        private readonly IHubContext<ChathubService> _hubContext;
        private readonly IRemoteQuery _remoteQuery;

        public MQTTService(IHubContext<ChathubService> hubContext, IRemoteQuery remoteQuery)
        {
            _hubContext = hubContext;
            _remoteQuery = remoteQuery;
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

                _mqttClient.Subscribe(new string[] { "socket/info" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

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
            // Kết nối thiết bị
            if(e.Topic == ConstTopicConnect.ConnectWifi)
            {
                var response = Encoding.UTF8.GetString(e.Message);
                var MQTT = JsonConvert.DeserializeObject<MQTTResponse>(response);
                await _remoteQuery.GetStatusRemoteByRemoteName(MQTT.ClientId);
            }

            // Nhận đáp án từ thiết bị
            if(e.Topic == ConstTopicConnect.ChooseAnswer)
            {
                string response = Encoding.UTF8.GetString(e.Message);
                var MQTT = JsonConvert.DeserializeObject<MQTTResponse>(response);

                _hubContext.Clients.All.SendAsync("ReceiveMessage", MQTT.ClientId ?? "", MQTT.Msg);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _mqttClient.Disconnect();
            return Task.CompletedTask;
        }
    }
}
