using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Mqtt_homeapp
{
    public partial class Form2 : Form
    {

        private List<double> tempAvg;
        private List<double> humAvg;
        private List<double> pressAvg;
        private double temperature;
        private double pressure;
        private double humidity;

        private List<Room> dataList = new List<Room>();

        public Form2(List<double> tempAvg, List<double> humAvg, List<double> pressAvg, double temperature, double humidity, double pressure)
        {
            InitializeComponent();
            PopulateChart(chart1, tempAvg);
            PopulateChart(chart2, humAvg);
            PopulateChart(chart3, pressAvg);

            this.tempAvg = tempAvg;
            this.humAvg = humAvg;
            this.pressAvg = pressAvg;
            this.temperature= temperature;
            this.pressure = pressure;
            this.humidity = humidity;
        }

        private void UpdateListBox()
        {

            listBox1.Items.Clear();

            foreach (Room item in dataList)
            {
                listBox1.Items.Add($"Temperature: {item.Temp}, Humidity: {item.Hum}, Pressure: {item.Press}, Name: {item.Name}");
            }
        }

        private void PopulateChart(Chart chart, List<double> data)
        {
            chart.Series.Clear();

            Series series = new Series();
            series.ChartType = SeriesChartType.Line; 
            chart.Series.Add(series);

            foreach (double value in data)
            {
                series.Points.AddY(value);
            }

            for (int i = 0; i < data.Count; i++)
            {
                chart.ChartAreas[0].AxisX.CustomLabels.Add(i + 0.5, i + 1.5, (i + 1).ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string room = textBox1.Text;

            Room newRoom = new Room(temperature, humidity, pressure, room);
            dataList.Add(newRoom);
            UpdateListBox();

        }
    }
}
