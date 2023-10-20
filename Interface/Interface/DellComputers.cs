using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    class DellComputers : Computer, Dell
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Mark { get; set; }
        public void GetDesktops()
        {
            Name = "Desktop1";
            Price = 2000;   
        }

        public void GetLaptops()
        {
            Name = "Laptop1";
            Price = 1500;
        }

        public void GetMark()
        {
            Mark = "Dell";
        }
    }
}
