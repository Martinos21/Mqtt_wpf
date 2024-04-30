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
using System.Globalization;
using System.IO;



namespace Mqtt_homeapp
{
    public partial class Form1 : KryptonForm
    {

        MqttClient mqttClient;
        string temp, hum, press;
        double temp_double, hum_double, press_double;

        List<double> temp_avg = new List<double>();
        List<double> hum_avg = new List<double>();
        List<double> press_avg = new List<double>();
        List<data> dataList = new List<data>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newform = new Form2(temp_avg,hum_avg, press_avg, temp_double, hum_double, press_double);
            newform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName = "Test";

            string directory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(directory, fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {

                foreach (var item in listBox1.Items)
                {
                    writer.WriteLine(item.ToString());
                }
            }

            MessageBox.Show("List content saved to CSV file successfully.");
        }

        private void UpdateListBox()
        {
            
            listBox1.Items.Clear();

            foreach (data item in dataList)
            {
                listBox1.Items.Add($"Temperature: {item.Temp}, Humidity: {item.Hum}, Pressure: {item.Press}, Life Index: {item.life_index(item.Temp, item.Press, item.Hum)}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data newItem = new data(temp_double, hum_double, press_double);
            dataList.Add(newItem);
            UpdateListBox();
        }

        private void MqClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            //label2.Invoke((MethodInvoker)(() => label2.Text = message));

            if (e.Topic == "wpf-home-temp")
            {
                temp = Convert.ToString(message);
                temp_double = Convert.ToDouble(temp, CultureInfo.InvariantCulture);
                label2.Invoke((MethodInvoker)(() => label2.Text = temp));
                temp_avg.Add(temp_double);
                
            }
            else if (e.Topic == "wpf-home-hum")
            {
                hum = Convert.ToString(message);
                hum_double = Convert.ToDouble(hum, CultureInfo.InvariantCulture);
                label4.Invoke((MethodInvoker)(() => label4.Text = hum));
                hum_avg.Add(hum_double);
            }
            else if (e.Topic == "wpf-home-press")
            {
                press = Convert.ToString(message);
                press_double = Convert.ToDouble(press, CultureInfo.InvariantCulture);
                label8.Invoke((MethodInvoker)(() => label8.Text = press));
                press_avg.Add(press_double);

            }

            data test = new data (temp_double, hum_double, press_double);

            double test_val = test.life_index(temp_double, press_double, hum_double);
            double temp_avg_val = Math.Round(test.CalculateAverage(temp_avg),2);
            double hum_avg_val = Math.Round(test.CalculateAverage(hum_avg),2);
            double press_avg_val = Math.Round(test.CalculateAverage(press_avg),2);

            string test_val_var = Convert.ToString(test_val, CultureInfo.InvariantCulture);
            string temp_avg_val_str = Convert.ToString(temp_avg_val, CultureInfo.InvariantCulture);
            string hum_avg_val_str = Convert.ToString(hum_avg_val, CultureInfo.InvariantCulture);
            string press_avg_val_str = Convert.ToString (press_avg_val, CultureInfo.CurrentCulture);

            label11.Invoke((MethodInvoker)(() => label11.Text = test_val_var));
            label13.Invoke((MethodInvoker)(() => label13.Text = temp_avg_val_str));
            label14.Invoke((MethodInvoker)(() => label14.Text = hum_avg_val_str));
            label15.Invoke((MethodInvoker)(() => label15.Text = press_avg_val_str));

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                mqttClient = new MqttClient("broker.hivemq.com");
                mqttClient.MqttMsgPublishReceived += MqClient_MqttMsgPublishReceived;
                mqttClient.Subscribe(new string[] {"wpf-home-temp", "wpf-home-hum", "wpf-home-press"}, new byte[] {MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});
                mqttClient.Connect("testik");
                label9.Invoke((MethodInvoker)(() => label19.Text = mqttClient.IsConnected ? "Connected" : "Disconnected"));
            });
        }
    }
}
