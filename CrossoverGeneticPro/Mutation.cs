using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverGeneticPro
{

    public struct NeighborList
    {
        public string CityName;
        public int Neighbors;

        public NeighborList(string cityName, int neighbors)
        {
            CityName = cityName;
            Neighbors = neighbors;
        }
    }

    class Mutation
    {
        private readonly Random _rand = new Random();
        private Calculations _calc = new Calculations();
        public int Iterations { get; set; }
        public int RandomMutations = 0;
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
                RandomMutations++;
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
                    Road parent1;
                    Road parent2;
                    if (licznikPoprawy > 10)
                    {
                        parent1 = RandomMutationForCrossover(population.PopulationList[parent1Index]);
                        _calc.CalculateTotalDistance(parent1);
                        parent2 = RandomMutationForCrossover(population.PopulationList[parent2Index]);
                        _calc.CalculateTotalDistance(parent2);

                        if (parent1.TotalDistance < parent2.TotalDistance)
                        {
                            population.PopulationList.Add(parent1);
                        }
                        else
                        {
                            population.PopulationList.Add(parent2);
                        }
                    }
                    else
                    {
                        parent1 = population.PopulationList[parent1Index];
                        parent2 = population.PopulationList[parent2Index];
                    }
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
            } while (licznikPoprawy <= 31);
            Console.WriteLine(RandomMutations);
        }

        private List<NeighborList> generateNeighborLists(List<NeighborList> NLL, string[] parent1, string[] parent2)
        {
            for (int i = 0; i < NLL.Count; i++)
            {
                NeighborList n1 = NLL[i];
                string city = n1.CityName;
                int lvl = GetNeighborLevel(parent1, city);
                lvl += GetNeighborLevel(parent2, city);
                n1.Neighbors = lvl;
                NLL[i] = n1;
            }
            return NLL;
        }

        private int GetNeighborLevel(string[] parent, string cityName)
        {
            int lvl = 0;
            for (int i = 0; i < parent.Length; i++)
            {
                if (parent[i] == cityName)
                {
                    if (i == 0)
                    {
                        if (parent[i + 1] != "X")
                        {
                            lvl++;
                        }
                    }
                    else if (i == parent.Length - 1)
                    {
                        if (parent[i - 1] != "X")
                        {
                            lvl++;
                        }
                    }
                    else
                    {
                        if (parent[i + 1] != "X")
                        {
                            lvl++;
                        }
                        if (parent[i - 1] != "X")
                        {
                            lvl++;
                        }
                    }
                }
            }

            return lvl;
        }

        public void MutateERO(Population population, List<City> citiesList)
        {
            _calc.OrderPopulation(population);
            int sizeOfPopulation = population.PopulationList.Count;
            int sizeOfRoad = population.PopulationList[0].CitiesList.Count;
            Road bestRoad = population.PopulationList[0];

            int licznikPoprawy = 0;

            List<NeighborList> NLList = new List<NeighborList>();

            for (int i = 0; i < sizeOfRoad; i++)
            {
                NLList.Add(new NeighborList(citiesList[i].CityName, 0));
            }

            bool poprawiono = false;

            do
            {
                decimal toKill1 = Convert.ToDecimal(sizeOfPopulation) * 0.2m;
                decimal toKill2 = Convert.ToDecimal(sizeOfPopulation) - toKill1;

                population.PopulationList.RemoveRange(Convert.ToInt16(toKill1), Convert.ToInt16(toKill2));

                int z = 0;
                for (int i = population.PopulationList.Count; i < sizeOfPopulation; i++)
                {
                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        NeighborList jList = new NeighborList();
                        jList.CityName = citiesList[j].CityName;
                        NLList[j] = jList;
                    }
                    int newSizeOfPopulation = population.PopulationList.Count;
                    int parent1Index = _rand.Next(0, newSizeOfPopulation);
                    int parent2Index = _rand.Next(0, newSizeOfPopulation);
                    Road parent1 = population.PopulationList[parent1Index];
                    Road parent2 = population.PopulationList[parent2Index];
                    Road child = new Road();
                    City nullCity = new City(0, 0, "N");
                    List<City> nullCityList = new List<City>();
                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        nullCityList.Add(nullCity);
                    }
                    child.CitiesList = nullCityList;

                    string[] parent1Table = new string[sizeOfRoad];
                    string[] parent2Table = new string[sizeOfRoad];
                    string[] childTable = new string[sizeOfRoad];


                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        parent1Table[j] = parent1.CitiesList[j].CityName;
                        parent2Table[j] = parent2.CitiesList[j].CityName;
                    }
                    NLList = generateNeighborLists(NLList, parent2Table, parent1Table);

                    childTable[0] = parent1Table[0];
                    parent1Table[0] = "X";

                    for (int j = 0; j < parent2Table.Length; j++)
                    {
                        if (parent2Table[j] == childTable[0])
                        {
                            parent2Table[j] = "X";
                        }
                    }


                    int idx = NLList.Select((value, index) => new { value, index })
                       .Where(x => x.value.CityName == childTable[0]).Select(x => x.index).FirstOrDefault();

                    NeighborList zaz = new NeighborList();
                    zaz.CityName = "X";
                    NLList[idx] = zaz;

                    for (int j = 1; j < sizeOfRoad; j++)
                    {
                        int r = 0;
                        List<string> minimalCities = new List<string>();
                        int minumum = 4;
                        for (int k = 0; k < sizeOfRoad; k++)
                        {
                            if (NLList[k].Neighbors < minumum && NLList[k].CityName != "X")
                            {
                                minumum = NLList[k].Neighbors;
                            }
                        }
                        for (int k = 0; k < sizeOfRoad; k++)
                        {
                            if (NLList[k].Neighbors == minumum)
                            {
                                minimalCities.Add(NLList[k].CityName);
                            }
                        }

                        if (minimalCities.Count == 0)
                        {
                            Console.WriteLine("AAA");
                        }

                        try
                        {
                            r = _rand.Next(0, minimalCities.Count);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ERROR: Pusta lista miast z minimalną liczbą sąsiadów!!!");
                            throw;
                        }

                        childTable[j] = minimalCities[r];
                        for (int k = 0; k < parent2Table.Length; k++)
                        {
                            if (parent2Table[k] == minimalCities[r])
                            {
                                parent2Table[k] = "X";
                            }
                        }
                        for (int k = 0; k < parent1Table.Length; k++)
                        {
                            if (parent1Table[k] == minimalCities[r])
                            {
                                parent1Table[k] = "X";
                            }
                        }
                        int indeks = NLList.Select((value, index) => new { value, index })
                            .Where(x => x.value.CityName == childTable[j]).Select(x => x.index).FirstOrDefault();

                        NeighborList zaa = new NeighborList();
                        zaa.CityName = "X";
                        r = indeks;
                        NLList[indeks] = zaa;
                    }
                    City[] citiesTable = new City[sizeOfRoad];
                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        City c = new City(0, 0, "X");
                        var cc = citiesList.Where(x => x.CityName == childTable[j]).GroupBy(x => x.CityName)
                            .Select(x => x.FirstOrDefault());
                        foreach (var VARIABLE in cc)
                        {
                            c = VARIABLE;
                        }
                        citiesTable[j] = c;
                    }

                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        child.CitiesList[j] = citiesTable[j];
                    }
                    _calc.CalculateTotalDistance(child);
                    population.PopulationList.Add(child);
                }
                decimal oldBest = bestRoad.TotalDistance;
                decimal pretender = population.PopulationList[0].TotalDistance;

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

            } while (licznikPoprawy <= 20);
        }

        public void MutatePMX(Population population)
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
                    Road parent1;
                    Road parent2;
                    parent1 = population.PopulationList[parent1Index];
                    parent2 = population.PopulationList[parent2Index];
                    int sizeOfRoad = population.PopulationList[0].CitiesList.Count;
                    Road child1 = new Road();
                    Road child2 = new Road();

                    City nullCity = new City(0, 0, "N");
                    List<City> nullCityList = new List<City>();
                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        nullCityList.Add(nullCity);
                    }
                    child1.CitiesList = nullCityList;
                    child2.CitiesList = nullCityList;

                    //int indexStart = _rand.Next(0, sizeOfRoad);
                    int indexStart = 0;


                    bool backAtStart = false;
                    string[] Cities = new string[sizeOfRoad];
                    int[] CitiesIndex1 = new int[sizeOfRoad];
                    int[] CitiesIndex2 = new int[sizeOfRoad];
                    int c1 = 0;
                    int c2 = 0;
                    CitiesIndex2[c2] = 0;
                    c1++;
                    int x = 0;
                    string startCity = parent1.CitiesList[indexStart].CityName;
                    string sp1 = startCity;

                    while (backAtStart == false)
                    {
                        Cities[x] = sp1;
                        x++;
                        string sp2 = "";
                        int ip2 = 0;
                        int ip1 = 0;
                        for (int j = 0; j < sizeOfRoad; j++)
                        {
                            if (parent2.CitiesList[j].CityName == sp1)
                            {
                                sp2 = parent2.CitiesList[j].CityName;
                                ip2 = j;
                                CitiesIndex1[c1] = ip2;
                                c1++;
                            }
                        }
                        for (int j = 0; j < sizeOfRoad; j++)
                        {
                            if (parent1.CitiesList[j].CityName == sp2)
                            {
                                sp1 = parent2.CitiesList[j].CityName;
                                ip1 = j;
                                CitiesIndex2[c2] = ip1;
                                c2++;
                            }
                        }
                        if (sp1 == startCity)
                        {
                            backAtStart = true;
                            continue;
                        }
                    }

                    for (int j = 0; j < sizeOfRoad; j++)
                    {
                        //child1.CitiesList[j] = 
                    }







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
            } while (licznikPoprawy <= 31);
            Console.WriteLine(RandomMutations);
        }

    }
}