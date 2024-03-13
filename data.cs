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
            double index;

            index = (temperature + press + hum) / 10f;

            return index;
        }

        public data (double temp, double hum, double press )
        {
            Temp = temp;
            Hum = hum;
            Press = press;

        }
    }
}
