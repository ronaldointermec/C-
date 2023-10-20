using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innheritance
{
   class Employee:Human
    {

        public string Position { get; set; }
        public double Salary { get; set; }
        public void SetValues() 
        {
            Name = "Ronaldo";
            SurName = "Silva";
            ID = 1;
            Gender = 'M';
            Age = 43;
        }
    }
}
