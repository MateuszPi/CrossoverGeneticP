using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CrossoverGeneticPro
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    public partial class MainWindow : Window
    {
        int populationSize = 5000;
        List<City> listaMiast = new List<City>();
        bool CitiesLoaded = false;
        string Met;

        public MainWindow()
        {
            InitializeComponent();
            Wynik.Refresh();
            PopulationSize.MaxLength = 10;
            Met = Methode.Text;
        }

        private void Licz_Click(object sender, RoutedEventArgs e)
        {
            Wynik.Text = "Liczę...\r\nMoże to potrwać do kilku minut.";
            if (CitiesLoaded == false)
            {
                Wynik.Text = $"{Wynik.Text}\r\nZaładowano domyśly zestaw miast";
            }
            Wynik.Refresh();
            var rand = new Random();
            Met = Methode.Text;
            Console.WriteLine(Met);
            //int finalPopSize = populationSize;

            if (CitiesLoaded == false)
            {
                listaMiast.Add(new City(37, 79, "0"));
                listaMiast.Add(new City(-90, -44, "1"));
                listaMiast.Add(new City(-89, 13, "2"));
                listaMiast.Add(new City(38, -66, "3"));
                listaMiast.Add(new City(36, -100, "4"));
                listaMiast.Add(new City(38, 41, "5"));
                listaMiast.Add(new City(-31, 74, "6"));
                listaMiast.Add(new City(61, 20, "7"));
                listaMiast.Add(new City(-29, 39, "8"));
                listaMiast.Add(new City(71, -85, "9"));
                listaMiast.Add(new City(12, -2, "10"));
                listaMiast.Add(new City(-41, 17, "11"));
                listaMiast.Add(new City(17, -36, "12"));
                listaMiast.Add(new City(91, -66, "13"));
                listaMiast.Add(new City(71, -70, "14"));
                listaMiast.Add(new City(74, -21, "15"));
                listaMiast.Add(new City(16, -74, "16"));
                listaMiast.Add(new City(-56, -67, "17"));
                listaMiast.Add(new City(63, 15, "18"));
                listaMiast.Add(new City(-55, 26, "19"));
                listaMiast.Add(new City(-38, 26, "20"));
                listaMiast.Add(new City(8, 59, "21"));
                listaMiast.Add(new City(91, -86, "22"));
                listaMiast.Add(new City(-99, -12, "23"));
                listaMiast.Add(new City(-96, -58, "24"));
            }




            foreach (var item in listaMiast)
            {
                Console.WriteLine($"{item.X} \t{item.Y} \t{item.CityName}");
            }

            Calculations calc = new Calculations();


            var pop = new Population();

            bool cykl = false;

            if (Cykl.IsChecked == true)
            {
                cykl = true;
            }
            var mut = new Mutation(cykl);


            

            int finalPopulationSize = populationSize;

            mut.GenerateRandomPopulation(pop, listaMiast, populationSize);
            calc.CalculatePopulationDistances(pop, cykl);
            calc.OrderPopulation(pop);
            Stopwatch sw = new Stopwatch();

            if (Met == "OX1")
            {
                sw.Start();
                mut.MutateOX1(pop, listaMiast);
                sw.Stop();
            }
            else if (Met == "ERO")
            {
                sw.Start();
                mut.MutateERO(pop, listaMiast);
                sw.Stop();
            }
            else if (Met == "CCO")
            {
                sw.Start();
                mut.MutateCCO(pop);
                sw.Stop();
            }

            string wynik = $"Liczba iteracji: {mut.Iterations} w czasie {sw.Elapsed}\r\n" +
                $"Optymalny Wynik: {pop.PopulationList[0].TotalDistance}\r\n";
            var lista = pop.PopulationList[0].CitiesList;
            foreach (var VARIABLE in lista)
            {
                wynik += $"{VARIABLE.CityName}|";
            }
            Wynik.Text = wynik;

            Console.WriteLine(wynik);

            Bitmap bmp = new Bitmap(10000, 10000);
            System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Red, 10);

            for (int i = 0; i < pop.PopulationList[0].CitiesList.Count; i++)
            {
                if (cykl == false && i == pop.PopulationList[0].CitiesList.Count-1)
                {
                    continue;
                }
                else if (cykl == true && i == pop.PopulationList[0].CitiesList.Count-1)
                {
                    int x1b = (int)pop.PopulationList[0].CitiesList[i].X + 100;
                    int x2b = (int)pop.PopulationList[0].CitiesList[0].X + 100;
                    int y1b = (int)pop.PopulationList[0].CitiesList[i].Y + 100;
                    int y2b = (int)pop.PopulationList[0].CitiesList[0].Y + 100;
                    using (var graphics = Graphics.FromImage(bmp))
                    {
                        graphics.DrawLine(blackPen, x1b, y1b, x2b, y2b);
                    }
                    continue;
                }
                int x1 = (int)pop.PopulationList[0].CitiesList[i].X + 100;
                int x2 = (int)pop.PopulationList[0].CitiesList[i + 1].X + 100;
                int y1 = (int)pop.PopulationList[0].CitiesList[i].Y + 100;
                int y2 = (int)pop.PopulationList[0].CitiesList[i + 1].Y + 100;

                using (var graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawLine(blackPen, x1, y1, x2, y2);
                }
            }

            bmp.Save("map.bmp");
        }

        private void PopulationSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PopulationSize.Text.Length == 0)
            {
                populationSize = 5000;
                return;
            }
            string popSizeString = PopulationSize.Text;
            populationSize = Convert.ToInt32(popSizeString);
            Console.WriteLine(populationSize);

        }

        private void PopulationSiz_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void LoadCities_Click(object sender, RoutedEventArgs e)
        {
            Loader load = new Loader();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                listaMiast = load.ReadCitiesLocationFromFile(filename);
                CitiesLoaded = true;
            }

        }

        private void Instrukcja_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("InstrukcjaObsługiCrossoverGeneticP.pdf");

        }

        private void Methode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }


    }
}
