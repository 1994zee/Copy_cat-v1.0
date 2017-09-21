using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copy_cat.Model
{
    class Record
    {
        public string CompanyName { get; set; }
        public string status { get; set; }
        public Record(string a, string b)
        {
            CompanyName = a;
            status = b;
        }
        public void print()
        {
            Console.WriteLine("--> " + CompanyName + " -- " + status);
        }
    }
}
