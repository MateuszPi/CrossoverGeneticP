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

        public MainWindow()
        {
            InitializeComponent();
            Wynik.Refresh();
            PopulationSize.MaxLength = 10;
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

            var mut = new Mutation();

            int finalPopulationSize = populationSize;

            mut.GenerateRandomPopulation(pop, listaMiast, populationSize);
            calc.CalculatePopulationDistances(pop);
            calc.OrderPopulation(pop);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            mut.MutateOX1(pop);
            sw.Stop();

            string wynik = $"Liczba iteracji: {mut.Iterations} w czasie {sw.Elapsed}\r\n" +
                $"Optymalny Wynik: {pop.PopulationList[0].TotalDistance}\r\n";
            var lista = pop.PopulationList[0].CitiesList;
            foreach (var VARIABLE in lista)
            {
                wynik += $"{VARIABLE.CityName}|";
            }
            Wynik.Text = wynik;

            Console.WriteLine(wynik);

            Bitmap bmp = new Bitmap(200, 200);
            System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            for (int i = 0; i < 24; i++)
            {
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
                sss.Text = filename;
                listaMiast =  load.ReadCitiesLocationFromFile(filename);
                CitiesLoaded = true;
            }

        }
    }
}
