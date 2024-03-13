using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        string temp, hum, press;
        double temp_double, hum_double, press_double;
        
        //data test = new data(float.Parse(temp), float.Parse(hum), float.Parse(press));
        

        public Form1()
        {
            InitializeComponent();

        }

        private void MqClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            //label2.Invoke((MethodInvoker)(() => label2.Text = message));

            if (e.Topic == "wpf-home-temp")
            {
                temp = message;
                label2.Invoke((MethodInvoker)(() => label2.Text = temp));
            }
            else if (e.Topic == "wpf-home-hum")
            {
                hum = message;
                label4.Invoke((MethodInvoker)(() => label4.Text = hum));
            }
            else if (e.Topic == "wpf-home-press")
            {
                press = message;
                label8.Invoke((MethodInvoker)(() => label8.Text = press));
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                mqttClient = new MqttClient("broker.hivemq.com");
                mqttClient.MqttMsgPublishReceived += MqClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] {"wpf-home-temp", "wpf-home-hum", "wpf-home-press"}, new byte[] {MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});
                mqttClient.Connect("testik");
            });



        }
    }
}
