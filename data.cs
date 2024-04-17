using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mqtt_homeapp
{
    class data
    {
        public double Temp { get; set; }
        public double Hum { get; set; }
        public double Press { get; set;}

        public double life_index(double temperature, double press, double hum)
        {
            double idealTemp = 20.0;
            double idealPress = 1010.0;
            double idealHumLow = 30.0;
            double idealHumHigh = 40.0;

            double tempDiff = Math.Abs(temperature - idealTemp);
            double pressDiff = Math.Abs(press - idealPress);
            double humDiff;
            if (hum < idealHumLow)
            {
                humDiff = idealHumLow - hum;
            }
            else if (hum > idealHumHigh)
            {
                humDiff = hum - idealHumHigh;
            }
            else
            {
                humDiff = 0.0;
            }

            double tempWeight = 1.0 - (tempDiff / idealTemp);
            double pressWeight = 1.0 - (pressDiff / idealPress);
            double humWeight = 1.0 - (humDiff / (idealHumHigh - idealHumLow));

            double index = (tempWeight + pressWeight + humWeight) / 3.0 * 100.0;

            index = Math.Round(Math.Max(0.0, Math.Min(100.0, index)), 2);

            return index;
        }


        public double CalculateAverage(List<double> collection)
        {
            double sum = 0;

            foreach (double num in collection)
            {
                sum += num;
            }

            double average = sum / collection.Count;

            return average;
        }

        public data (double temp, double hum, double press )
        {
            Temp = temp;
            Hum = hum;
            Press = press;

        }
    }

    class home : data 
    {
        public List<Room> Rooms { get; set; }
        public home(double temp, double hum, double press) : base(temp, hum, press)
        {
            Rooms = new List<Room>();
        }
    }

    class Room : data
    {
        public string Name { get; set; }

        public Room(double temp, double hum, double press, string name) : base(temp, hum, press)
        {
            Name = name;
        }
    }

}
