using Cassandra;
using KafkaNet;
using KafkaNet.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace PoC_Kore1
{
    class Program
    {
        static void Main(string[] args)
        {


            /*
             *  kafka configurations
             */ 
            string topic = "test-7";
            Uri uri = new Uri("http://localhost:9092");
            var options = new KafkaOptions(uri);
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(topic, router));
            

            /*
             *  cassandra configurations
             */
            Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            ISession session = cluster.Connect("pocdemo");
            var tableName = "messagesdemo";

            var deviceobj = new DeviceData();
            foreach (var message in consumer.Consume())
            {


                string data = Encoding.UTF8.GetString(message.Value);
                var user = JsonConvert.DeserializeObject<DeviceData>(data);

                var timestamp = ConvertToTimestamp(user.ActualDate);

                var query = $"INSERT INTO {tableName}" +
                $"(id,iMEI,actualdate,lat,lon,direction, odometer, speed, analog, temp, eventcode, textm, fuel, temp2, voltage)" +
                $" VALUES" +
                $"({user.id},'{user.iMEI}',{timestamp},{user.Lat},{user.Lon}" +
                $",{user.Direction},{user.Odotemer},{user.Speed},{user.Analog},{user.Temp},{user.EventCode}" +
                $",{user.TextM},{user.Fuel},{user.Temp2},{user.Voltage});";

                var result = session.Execute(query);

                Row x = session.Execute("select * from messagesdemo").First();
                Console.WriteLine("{0} {1} ", x["lon"], x["lat"]);

                Console.WriteLine("{0}", user.id + " " + user.iMEI + " 2018-08-30T08:03:31 " + user.Lat + " " + user.Lon +" " + user.Direction + " " + user.Odotemer + " " + user.Speed + " " + user.Analog + " " + user.Temp + " " + user.EventCode + " " + user.TextM + " " + user.Fuel + " " + user.Temp2 + " " + user.Voltage);
            }
            Console.ReadLine();
        }

        /*
         * Method to convert to timestamp
         */
        private static object ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
    }

}



