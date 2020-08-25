using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork13
{
    class Program
    {
        static void Main(string[] args)
        {
            PayBill bill1 = new PayBill( 12, 33, 23, 2);
            Console.WriteLine(bill1);
            PayBill.check = false;                        //public static property, to serialize counting fields - true , to not - false
            Console.WriteLine();

            SaveAsSoapFormater(bill1, "bill1.dat");
            ReadAsSoapFormater("bill1.dat");
        }
        static void SaveAsSoapFormater(object objGraph, string fileName) // method for stream creating and SOAP serialization
        {
            SoapFormatter sf = new SoapFormatter();
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                sf.Serialize(fStream, objGraph);
            }
            Console.WriteLine("Serialization OK!\n\n");
        }
        static void ReadAsSoapFormater(string fileName)  // deserialization method
        {
            SoapFormatter sf = new SoapFormatter();
            PayBill result = null;
            using (Stream fStream = File.OpenRead(fileName))
            {
                result = (PayBill)sf.Deserialize(fStream);
            }
            Console.WriteLine("Deserialization OK!\n\n");
            Console.WriteLine(result);
        }
    }
    [Serializable]
    public class PayBill : ISerializable   
    {
        public static bool check { get; set; }
        private int taxPerDay;
        private int daysCount;
        private int taxForDelaying;
        private int daysWereDelayed;
        private int sumeWithoutTax;
        private int totalTax;
        private int totalSume;
        public PayBill() { }  // default constructor for user's serialization
        public PayBill( int taxPerDay, int daysCount, int taxForDelaying, int daysWereDelayed)
        {
            this.taxPerDay = taxPerDay;
            this.daysCount = daysCount;
            this.taxForDelaying = taxForDelaying;
            this.daysWereDelayed = daysWereDelayed;
            this.sumeWithoutTax = daysCount * taxPerDay;
            this.totalTax = taxForDelaying * daysWereDelayed;
            this.totalSume = sumeWithoutTax + totalTax;
        }
        private void setSume()
        {
            sumeWithoutTax =  daysCount * taxPerDay;
        }
        private void setTax()
        {
            totalTax = daysWereDelayed * taxPerDay;
        }
        private void setTotalSume()
        {
            totalSume = sumeWithoutTax + totalTax;
        }
        public override string ToString()
        {
            return $"Tax per day : {taxPerDay}\n" +
                $"Days amount : {daysCount}\n" +
                $"Extra pay : {taxForDelaying}\n" +
                $"Days delayed : {daysWereDelayed}\n" +
                $"Sume without taxes : {sumeWithoutTax}\n" +
                $"Sume of taxes : {totalTax}\n" +
                $"Total sume : {totalSume}";
        }

        protected PayBill(SerializationInfo info, StreamingContext context)  // constructor for deserialization 
        {
            if (check == true)                                               // check our static value
            {
                taxPerDay = info.GetInt32("Tax_Per_Day");
                daysCount = info.GetInt32("Days_amount");
                taxForDelaying = info.GetInt32("Tax_for_delaying");
                daysWereDelayed = info.GetInt32("Days_were_delayed");
                sumeWithoutTax = info.GetInt32("Sume_without_tax");
                totalTax = info.GetInt32("Total_tax_sume");
                totalSume = info.GetInt32("Total_sume_with_tax");
            }
            else
            {
                taxPerDay = info.GetInt32("Tax_Per_Day");
                daysCount = info.GetInt32("Days_amount");
                taxForDelaying = info.GetInt32("Tax_for_delaying");
                daysWereDelayed = info.GetInt32("Days_were_delayed");
            }
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) //user's method for serialization
        {
            if (check == true)
            {
                info.AddValue("Tax_Per_Day", taxPerDay);
                info.AddValue("Days_amount", daysCount);
                info.AddValue("Tax_for_delaying", taxForDelaying);
                info.AddValue("Days_were_delayed", daysWereDelayed);
                info.AddValue("Sume_without_tax", sumeWithoutTax);
                info.AddValue("Total_tax_sume", totalTax);
                info.AddValue("Total_sume_with_tax", totalSume);
            }
            else
            {
                info.AddValue("Tax_Per_Day", taxPerDay);
                info.AddValue("Days_amount", daysCount);
                info.AddValue("Tax_for_delaying", taxForDelaying);
                info.AddValue("Days_were_delayed", daysWereDelayed);
            }
        }        

    }
}
