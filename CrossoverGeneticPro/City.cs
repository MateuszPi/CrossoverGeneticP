using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverGeneticPro
{
    class City
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public string CityName { get; set; }

        public City(decimal x, decimal y, string cityName)
        {
            X = x;
            Y = y;
            CityName = cityName;
        }
    }
}
