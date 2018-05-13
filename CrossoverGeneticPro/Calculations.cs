﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverGeneticPro
{
    class Calculations
    {
        public decimal CalculateDistances(City A, City B)
        {
            decimal distanceX = (decimal)Math.Pow((double)A.X - (double)B.X, 2);
            decimal distanceY = (decimal)Math.Pow((double)A.Y - (double)B.Y, 2);
            decimal result = (decimal)Math.Sqrt((double)distanceX + (double)distanceY);
            return result;
        }
        public void CalculateTotalDistance(Road track)
        {
            for (int i = 0; i < track.CitiesList.Count - 1; i++)
            {
                City cityA = track.CitiesList[i];
                City cityB = track.CitiesList[i + 1];
                if (cityA == cityB)
                {
                    track.TotalDistance = 99999999999.0m;
                    return;
                }
                var res = CalculateDistances(cityA, cityB);
                track.TotalDistance += res;
            }
        }

        //public void CalculateDistancesFromDIstanceSet(TrRack track, List<Distance> listOfDistances)
        //{

        //}

        public void CalculatePopulationDistances(Population population)
        {
            var popSize = population.PopulationList.Count;
            for (int i = 0; i < popSize; i++)
            {
                CalculateTotalDistance(population.PopulationList[i]);
            }
        }
        public void OrderPopulation(Population population)
        {
            population.PopulationList.Sort(delegate (Road trackA, Road trackB)
            {
                return trackA.TotalDistance.CompareTo(trackB.TotalDistance);
            });
        }
    }
}