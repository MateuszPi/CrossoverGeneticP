using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverGeneticPro
{
    class Loader
    {
        public List<City> ReadCitiesLocationFromFile(string fileName)
        {
           var lines = File.ReadAllLines($"{fileName}");
           //City[] output = new City[lines.Length];
           List<City> listOfCities = new List<City>();
           int i = 0;
            foreach (var line in lines)
            {
                var data = line.Split(';');
                listOfCities.Add(new City(Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), data[0]));
            }
            return listOfCities;
        }




    }
}
