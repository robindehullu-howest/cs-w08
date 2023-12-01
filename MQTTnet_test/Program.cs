using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;

var mqttFactory = new MqttFactory();

// Create a new MQTT server.
// var optionsBuilder = new MqttServerOptionsBuilder()
//     .WithDefaultEndpointPort(1883);
// var mqttServer = mqttFactory.CreateMqttServer(optionsBuilder.Build());
// await mqttServer.StartAsync();

// Create a new MQTT clients.
var mqttClient1 = mqttFactory.CreateMqttClient();
var mqttClient2 = mqttFactory.CreateMqttClient();

var options1 = new MqttClientOptionsBuilder()
    .WithClientId("Client1")
    .WithTcpServer("broker.hivemq.com", 1883)
    .Build();

var options2 = new MqttClientOptionsBuilder()
    .WithClientId("Client2")
    .WithTcpServer("broker.hivemq.com", 1883)
    .Build();

await mqttClient1.ConnectAsync(options1);
await mqttClient2.ConnectAsync(options2);

// Subscribe to a topic.
await mqttClient1.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("test/topic").Build());

// Register to message received events.
mqttClient1.ApplicationMessageReceivedAsync += async e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            };

// Publish a message.
await mqttClient2.PublishAsync(new MqttApplicationMessageBuilder()
    .WithTopic("MCT/CloudServices")
    .WithPayload("Hello World")
    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
    .WithRetainFlag()
    .Build());