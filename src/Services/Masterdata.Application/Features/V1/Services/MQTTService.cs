using Masterdata.Application.Features.V1.DTOs.MQTT;
using Microsoft.AspNetCore.SignalR;
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

        public MQTTService(IHubContext<ChathubService> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
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
            string message = Encoding.UTF8.GetString(e.Message);
            var Mqqt = JsonConvert.DeserializeObject<MQTTResponse>(message);

            Console.WriteLine($"Received message: {message}");

            _hubContext.Clients.All.SendAsync("ReceiveMessage", "User", Mqqt.Msg);


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _mqttClient.Disconnect();
            return Task.CompletedTask;
        }
    }
}
