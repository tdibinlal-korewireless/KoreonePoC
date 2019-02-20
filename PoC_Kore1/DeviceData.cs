using System;
using System.Collections.Generic;

namespace PoC_Kore1
{
    class DeviceData
    {
        public int id { get; set; }
        public string iMEI { get; set; }
        public DateTime ActualDate { get; set; }
        
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Direction { get; set; }
        public double Odotemer { get; set; }
        public float Speed { get; set; }
        public int Analog { get; set; }
        public float Temp { get; set; }
        public int EventCode { get; set; }
        public string TextM { get; set; }
        public float Fuel { get; set; }
        public float Temp2 { get; set; }
        public double Voltage { get; set; }
    }

    
}