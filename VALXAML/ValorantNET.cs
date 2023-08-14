using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using valorant.Models;
using static valorant.Enums;
using static valorant.Models.MMRHistory;


namespace valorant
{
    public class ValorantNET
    {
        private const string Endpoint = "https://api.henrikdev.xyz";
        private const string Route = "/valorant";

        public string Name { get; private set; }
        public string Tag { get; private set; }
        public Regions Region { get; private set; }


        public TextBlock _txtProfile;
        public TextBlock _txtRank;
        public TextBlock _txtCurrentRankInTier;
        public Image _imgRank;
        public ProgressBar _progressMMR;
        public Image _wideImg;


        public ValorantNET(string name, string tag, Regions region,
            TextBlock txtProfile, TextBlock txtRank, TextBlock txtCurrentRankInTier,
            Image imgRank , ProgressBar progressMMR, Image wideImg)
        {
            Name = name;
            Tag = tag;
            Region = region;
            _txtProfile = txtProfile;
            _txtRank = txtRank;
            _txtCurrentRankInTier = txtCurrentRankInTier;
            _imgRank = imgRank;
            _progressMMR = progressMMR;
            _wideImg = wideImg;
        }


        public async Task GetAccountAsync()
        {
            await GetAndDeserializeDataAsync<Account.JsonResponse>($"/v1/account/{Name}/{Tag}", jsonResponse =>
            {
                
                _txtProfile.Text = $"{jsonResponse.Data.Name}#{jsonResponse.Data.Tag}";
                /*var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri($"{jsonResponse.Data.Card.Small}");
                bitmapImage.EndInit();*/
                var wideImage = new BitmapImage();
                wideImage.BeginInit();
                wideImage.UriSource = new Uri($"{jsonResponse.Data.Card.Wide}"); ;
                wideImage.EndInit();

                //_imgProfile.Source = bitmapImage;
                _wideImg.Source = wideImage;
            });
        }

        public async Task GetMMRAsync()
        {
            await GetAndDeserializeDataAsync<MMR.JsonModel>($"/v1/mmr/{Region}/{Name}/{Tag}", jsonResponse =>
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri($"{jsonResponse.Data.Images.Large}"); ;
                bitmapImage.EndInit();
                _imgRank.Source = bitmapImage;  
                _txtRank.Text = $"{jsonResponse.Data.CurrentTierPatched}";
                _txtCurrentRankInTier.Text = $"{jsonResponse.Data.RankingInTier}/100";
                _progressMMR.Value = jsonResponse.Data.RankingInTier;
            });
        }


        public class MMRHistoryItem
        {
            public string MapName { get; set; }
            public string Date { get; set; }
            public int MmrChange { get; set; }
            public string MapImagePath { get; set; }
        }

        private Dictionary<string, string> MapImagePaths { get; } = new Dictionary<string, string>
    {
        {"Bind", "bind.png"},
        {"Pearl", "pearl.png"},
        {"Ascent", "ascent.png"},
        {"Split", "split.png"},
        {"Fracture", "fracture.png"},
        {"Lotus", "lotus.png"},
        {"Heaven", "heaven.png"},
    };

        public async Task<List<MMRHistoryItem>> GetMMRHistoryAsync()
        {
            List<MMRHistoryItem> mmrHistory = new List<MMRHistoryItem>();

            await GetAndDeserializeDataAsync<MMRHistory.JsonModel>($"/v1/mmr-history/{Region}/{Name}/{Tag}", jsonResponse =>
            {
                if (jsonResponse.Data.Count >= 5)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        mmrHistory.Add(new MMRHistoryItem
                        {
                            MapName = jsonResponse.Data[i].Map.Name,
                            Date = jsonResponse.Data[i].Date,
                            MmrChange = jsonResponse.Data[i].MmrChangeToLastGame,
                            MapImagePath = MapImagePaths.ContainsKey(jsonResponse.Data[i].Map.Name)
        ? MapImagePaths[jsonResponse.Data[i].Map.Name]
        : string.Empty
                        });
                    }
                }
            });

            return mmrHistory;
        }




        /*public async Task GetMMRHistoryAsync()
        {
            await GetAndDeserializeDataAsync<MMRHistory.JsonModel>($"/v1/mmr-history/{Region}/{Name}/{Tag}", jsonResponse =>
            {
               
                if (jsonResponse.Data.Count < 5) Console.WriteLine("Пользователь сыграл меньше 5 матчей за последнее время.");
                else
                {
                    for (int i = 0; i < 5; i++)
                    {

                        Console.WriteLine($"{jsonResponse.Data[i].Map.Name.PadRight(10)} | {(jsonResponse.Data[i].MmrChangeToLastGame > 0 ? "+" : "")}" +
                            $"{jsonResponse.Data[i].MmrChangeToLastGame} | {jsonResponse.Data[i].CurrentTierPatched} {jsonResponse.Data[i].RankingInTier.ToString().PadLeft(2)}/100 | {jsonResponse.Data[i].Date} \n");
                    }
                }
            });
        }*/



        private async Task GetAndDeserializeDataAsync<T>(string route, Action<T> printAction) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Endpoint);
                    HttpResponseMessage response = await client.GetAsync(Route + route);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    T jsonResponse = JsonConvert.DeserializeObject<T>(json);

                    printAction(jsonResponse);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Ошибка HTTP запроса: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Ошибка десериализации JSON: {ex.Message}");
                }
                
            }
        }
    }

}
