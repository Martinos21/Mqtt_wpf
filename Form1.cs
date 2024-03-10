using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;




namespace Mqtt_homeapp
{
    public partial class Form1 : KryptonForm
    {

        MqttClient mqttClient;

        public Form1()
        {
            InitializeComponent();

        }

        private void MqClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            //label2.Invoke((MethodInvoker)(() => label2.Text = message));

            if (e.Topic == "test1892")
            {
                label2.Invoke((MethodInvoker)(() => label2.Text = message));
            }
            else if (e.Topic == "test1893")
            {
                label4.Invoke((MethodInvoker)(() => label4.Text = message));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                mqttClient = new MqttClient("broker.hivemq.com");
                mqttClient.MqttMsgPublishReceived += MqClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] {"test1892", "test1893"}, new byte[] {MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});
                mqttClient.Connect("testik");
            });
        }

        

    }
}
