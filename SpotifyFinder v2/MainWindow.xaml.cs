using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using SpotifyFinder.Data;

namespace SpotifyFinder_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        HttpGrabber http = new HttpGrabber();
        
        public MainWindow()
        {
            InitializeComponent();        
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //nie chcemy nic wyszukiwac po tym jak wlacyzmy program
           //GetData();
        }
        
        private async Task GetData(string search)
        {

            var data = await http.MakeStringGreatAgain(search);            
            dataList.ItemsSource = data.artists.items;
        }


     
        private void searchBox_OnKeyUP(object sender, KeyEventArgs e)
        {
            string search = searchBox.Text;
            if(search.Length>2 && TimeBetweenkeyup(500, search))
            {
                GetData(search);
            }
            else
            {
                dataList.ItemsSource = null;
            }
           
        }

        private int lastSearchStringLength = 0;
        private long lastKeyUp = 0;

        // Metoda zeby spamowac mniej server zapytaniami

        private bool TimeBetweenkeyup(int time, string search)
        {
            bool searchAfterTime = false;

            if(search.Length == lastSearchStringLength)         
                return false;

                var elapsedTics = Stopwatch.GetTimestamp() - lastKeyUp;
                var elapsedTimeMs = elapsedTics * 100000 / Stopwatch.Frequency;

                if(elapsedTimeMs > time)             
                    searchAfterTime = true;
                

                lastKeyUp = Stopwatch.GetTimestamp();

                return searchAfterTime;
          
        }

    }
}
