using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverGeneticPro
{
    class Mutation
    {
        private readonly Random _rand = new Random();
        private Calculations _calc = new Calculations();
        public int Iterations { get; set; }
        public void GenerateRandomPopulation(Population population, List<City> citiesList, int populationSize)
        {
            Road[] randomPopulation = new Road[populationSize];

            List<Road> randomPopulationList = new List<Road>();

            for (int i = 0; i < populationSize; i++)
            {
                randomPopulationList.Add(GenerateRandomPopulationMember(citiesList));
            }
            population.PopulationList = randomPopulationList;
        }
        private Road GenerateRandomPopulationMember(List<City> citiesList)
        {
            int[] citiesIndexTable = new int[citiesList.Count];

            for (int i = 0; i < citiesList.Count; i++)
            {
                citiesIndexTable[i] = i;
            }

            int[] randomIndexTable = new int[citiesList.Count];


            Road road = new Road();

            int size = citiesIndexTable.Length;
            List<City> newCityList = new List<City>();

            for (int i = 0; i < citiesList.Count; i++)
            {
                int randomIndex = _rand.Next(0, size);
                randomIndexTable[i] = citiesIndexTable[randomIndex];
                newCityList.Add(citiesList[randomIndexTable[i]]);
                for (int j = randomIndex; j < size - 1; j++)
                {
                    citiesIndexTable[j] = citiesIndexTable[j + 1];
                }
                size--;
            }

            road.CitiesList = newCityList;
            return road;
        }

        private Road RandomMutationForCrossover(Road Parent)
        {
            int rand1 = _rand.Next(0, Parent.CitiesList.Count);
            int rand2 = _rand.Next(0, Parent.CitiesList.Count);
            City[] citiesArray = new City[Parent.CitiesList.Count];
            Road child = new Road();
            for (int i = 0; i < Parent.CitiesList.Count; i++)
            {
                if (i == rand1)
                {
                    citiesArray[i] = Parent.CitiesList[rand2];
                }
                else if (i == rand2)
                {
                    citiesArray[i] = Parent.CitiesList[rand1];
                }
                else
                {
                    citiesArray[i] = Parent.CitiesList[i];
                }
            }

            List<City> listOfCitiesForChild = new List<City>();

            for (int i = 0; i < Parent.CitiesList.Count; i++)
            {
                listOfCitiesForChild.Add(citiesArray[i]);
            }

            child.CitiesList = listOfCitiesForChild;
            _calc.CalculateTotalDistance(child);
            decimal P = Parent.TotalDistance;
            decimal C = child.TotalDistance;

            return child;
        }


        public void MutateOX1(Population population)
        {
            _calc.OrderPopulation(population);
            int sizeOfPopulation = population.PopulationList.Count;
            Road bestRoad = population.PopulationList[0];
            int licznikPoprawy = 0;
            do
            {
                decimal toKill1 = Convert.ToDecimal(sizeOfPopulation) * 0.5m;
                decimal toKill2 = Convert.ToDecimal(sizeOfPopulation) - toKill1;
                population.PopulationList.RemoveRange(Convert.ToInt32(toKill1), Convert.ToInt32(toKill2));
                int z = 0;
                for (int i = population.PopulationList.Count; i < sizeOfPopulation; i++)
                {
                    int newSizeOfPopulation = population.PopulationList.Count;
                    int parent1Index = _rand.Next(0, newSizeOfPopulation);
                    int parent2Index = _rand.Next(0, newSizeOfPopulation);
                    Road parent1 = population.PopulationList[parent1Index];
                    Road parent2 = population.PopulationList[parent2Index];
                    int sizeOfRoad = population.PopulationList[0].CitiesList.Count;
                    Road child = new Road();
                    City nullCity = new City(0, 0, "N");
                    List<City> nullCityList = new List<City>();
                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        nullCityList.Add(nullCity);
                    }
                    child.CitiesList = nullCityList;
                    int cross1 = _rand.Next(0, sizeOfRoad - 1);
                    int cross2 = _rand.Next(cross1, sizeOfRoad);
                    int c2c1 = cross2 - cross1;
                    for (int j = cross1; j <= cross2; j++)
                    {
                        child.CitiesList[j] = parent1.CitiesList[j];
                    }
                    int childIndex = 0;
                    int parentIndex = 0;
                    do
                    {
                        if (child.CitiesList[childIndex].CityName != "N")
                        {
                            childIndex++;
                            continue;
                        }
                        if (!child.CitiesList.Contains(parent2.CitiesList[parentIndex]))
                        {
                            child.CitiesList[childIndex] = parent2.CitiesList[parentIndex];
                            childIndex++;
                            parentIndex++;
                        }
                        else
                        {
                            parentIndex++;
                        }
                    } while (childIndex < sizeOfRoad);
                    _calc.CalculateTotalDistance(child);
                    Road child2 = RandomMutationForCrossover(child);
                    population.PopulationList.Add(child);
                    population.PopulationList.Add(child);
                }
               
                _calc.OrderPopulation(population);
                decimal oldBest = bestRoad.TotalDistance;

                if (population.PopulationList[0].TotalDistance < bestRoad.TotalDistance)
                {
                    bestRoad = population.PopulationList[0];
                    licznikPoprawy = 0;
                }
                else
                {
                    licznikPoprawy++;
                }
                Iterations++;
                Console.WriteLine($"Iteracja {Iterations}, current best: {bestRoad.TotalDistance}");
                z++;
            } while (licznikPoprawy <= 21);
        }
    }
}
