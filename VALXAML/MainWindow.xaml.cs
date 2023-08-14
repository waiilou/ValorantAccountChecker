using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using valorant;
using static valorant.Enums;
using static valorant.ValorantNET;
using System.Reflection;
using System.Windows.Media;

namespace VALXAML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ValorantNET client;
        private static string username;
        private static string tag;
        private static Regions region;

        public MainWindow()
        {
            InitializeComponent();
            lastGame.Visibility = Visibility.Hidden;
        }

        private async void btnButton_Click(object sender, RoutedEventArgs e)
        {
            lastGame.Visibility = Visibility.Visible;
            username = txtUsername.Text;
            tag = txtTag.Text;
            for (int i = 1; i <= 5; i++)
            {
                TextBlock? mapTextBlock = FindName($"map{i}") as TextBlock;
                TextBlock? eloTextBlock = FindName($"elo{i}") as TextBlock;
                TextBlock? dateTextBlock = FindName($"date{i}") as TextBlock;
                Image? mapImage = FindName($"image{i}") as Image;

                if (mapTextBlock != null && eloTextBlock != null && mapImage != null)
                {
                    mapTextBlock.Text = string.Empty;
                    eloTextBlock.Text = string.Empty;
                    dateTextBlock.Text = string.Empty;
                    mapImage.Source = null;
                }
            }

            Dictionary<RadioButton, Regions> radioButtonRegionMap = new Dictionary<RadioButton, Regions>
                {
                    { AP, Regions.AP },
                    { KR, Regions.KR },
                    { NA, Regions.NA },
                    { EU, Regions.EU }
                };
            foreach (var kvp in radioButtonRegionMap)
            {
                if (kvp.Key.IsChecked == true)
                {
                    region = kvp.Value;
                    break;
                }
            }
                client = new ValorantNET(username, tag, region, txtProfile, txtRank, txtCurrentRankInTier,imgRank,progressMMR,wideImg);
                await client.GetAccountAsync();
                await client.GetMMRAsync();

            List<MMRHistoryItem> mmrHistory = await client.GetMMRHistoryAsync();

           
            foreach (var item in mmrHistory)
            {
                
                TextBlock? mapTextBlock = FindName($"map{mmrHistory.IndexOf(item) + 1}") as TextBlock;
                TextBlock? eloTextBlock = FindName($"elo{mmrHistory.IndexOf(item) + 1}") as TextBlock;
                TextBlock? dateTextBlock = FindName($"date{mmrHistory.IndexOf(item) + 1}") as TextBlock;
                Image? mapImage = FindName($"image{mmrHistory.IndexOf(item) + 1}") as Image;

                if (mapTextBlock != null && eloTextBlock != null )
                {
                    mapTextBlock.Text = item.MapName;
                    dateTextBlock.Text = item.Date;
                    eloTextBlock.Text = item.MmrChange.ToString();
                    eloTextBlock.Text = (item.MmrChange > 0 ? "+" : "") + item.MmrChange.ToString();
                    eloTextBlock.Foreground = (item.MmrChange > 0 ? Brushes.LightGreen : Brushes.PaleVioletRed);

                    string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                    string imagePath = $"/{assemblyName};component/Resources/{item.MapName?.ToLower()}.png"; // Поправьте путь в соответствии со структурой вашего проекта

                    mapImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));


                }
            }




        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
