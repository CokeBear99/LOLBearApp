using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Net.Http.Formatting;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace LOLBearApp
{
    public partial class HomePannel : UserControl
    {
        private DispatcherTimer timer;
        private const string OpenWeatherMapApiKey = "72026f00d083d0d8fe5ae6b4fc711574";


        public HomePannel()
        {
            InitializeComponent();


            // 타이머를 초기화하고 설정합니다.
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1초마다 업데이트
            timer.Tick += Timer_Tick;

            // 타이머를 시작합니다.
            timer.Start();

            // UserControl이 로드될 때 한 번은 즉시 RAM 사용량을 업데이트합니다.
            UpdateRamUsage();

            // UserControl이 로드될 때 한 번은 즉시 현재 시간을 업데이트합니다.
            UpdateCurrentTime();

            // 날씨 정보 초기화
            InitializeWeather();
        }

        private async Task<(string, string)> GetLocation()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"https://ipinfo.io/json";
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseContent);
                        string city = result.city;
                        string region = result.region;
                        return (region, city);
                    }
                    else
                    {
                        return ("", "위치를 찾을 수 없음");
                    }
                }
            }
            catch (Exception ex)
            {
                return ("", $"위치를 가져오는 중 오류 발생: {ex.Message}");
            }
        }

        private async Task<WeatherInfo> GetWeatherInfo(string city)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={OpenWeatherMapApiKey}&units=metric";

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsAsync<OpenWeatherMapResponse>();
                        return new WeatherInfo
                        {
                            Temperature = $"{content.Main.Temp}℃",
                            Weather = content.Weather[0].Description,
                            Icon = content.Weather[0].Icon // 아이콘 URL 설정
                        };
                    }
                    else
                    {
                        return new WeatherInfo
                        {
                            Temperature = "N/A",
                            Weather = "N/A",
                            Icon = "" // 기본값 설정
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting weather information: {ex.Message}");
                return new WeatherInfo
                {
                    Temperature = "N/A",
                    Weather = "N/A",
                    Icon = "" // 오류 발생 시 기본값 설정
                };
            }
        }

        public async void InitializeWeather()
        {
            try
            {
                (string region, string city) = await GetLocation();
                UpdateLocation($"{region} {city}");

                // 날씨 정보 가져오기
                WeatherInfo weatherInfo = await GetWeatherInfo(city);
                UpdateWeather(weatherInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing weather: {ex.Message}");
            }
        }

        private void UpdateWeather(WeatherInfo weatherInfo)
        {
            Dispatcher.Invoke(() =>
            {
                TemperatureTextBlock.Text = $"{weatherInfo.Temperature}";
                string koreanWeather = GetKoreanWeatherName(weatherInfo.Weather);
                WeatherTextBlock.Text = koreanWeather;

                // 아이콘 불러오기
                string iconUrl = $"http://openweathermap.org/img/wn/{weatherInfo.Icon}@2x.png";
                WeatherIconImage.Source = new BitmapImage(new Uri(iconUrl));
            });

        }

        private string GetKoreanWeatherName(string englishWeather)
        {
            if (weatherDictionary.ContainsKey(englishWeather))
            {
                return weatherDictionary[englishWeather];
            }
            else
            {
                // 대응되는 한글 이름이 없는 경우 기존 영어 이름 그대로 반환합니다.
                return englishWeather;
            }
        }


        public async void UpdateLocation(string location)
        {
            try
            {
                // 파파고 API를 사용하여 번역된 결과를 가져옵니다.
                string translatedLocation = await TranslateLocation(location);

                Dispatcher.Invoke(() =>
                {
                    // 번역된 위치 정보를 텍스트 블록에 표시합니다.
                    LocationTextBlock.Text = translatedLocation;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error translating location: {ex.Message}");
            }
        }

        private async Task<string> TranslateLocation(string location)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // MyMemoryTranslated API 요청 URL
                    string url = $"https://api.mymemory.translated.net/get?q={WebUtility.UrlEncode(location)}&langpair=en|ko";

                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();

                    // 응답 데이터 파싱 및 번역된 텍스트 추출
                    dynamic data = JsonConvert.DeserializeObject(result);
                    string translatedText = data?.responseData?.translatedText;

                    return translatedText ?? location;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error translating location: {ex.Message}");
                return location;
            }
        }








        private void Timer_Tick(object sender, EventArgs e)
        {
            // 타이머가 틱될 때마다 RAM 사용량을 업데이트합니다.
            UpdateRamUsage();

            // 타이머가 틱될 때마다 현재 시간을 업데이트합니다.
            UpdateCurrentTime();


        }

        private void UpdateRamUsage()
        {
            try
            {
                // 현재 프로세스의 RAM 사용량을 가져옵니다.
                Process currentProcess = Process.GetCurrentProcess();
                long ramUsageBytes = currentProcess.WorkingSet64;

                // 현재 프로세스의 RAM 사용량 가져오기
                Process process = Process.GetCurrentProcess();
                long usedRamBytes = process.WorkingSet64;
                double usedRamMB = usedRamBytes / (1024.0 * 1024.0);

                // RAM 사용량 표시
                RamUsage.Text = $"Memory Usage : {usedRamMB:F2} MB";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting RAM usage: {ex.Message}");
            }
        }

        private void UpdateCurrentTime()
        {
            // 현재 시간을 가져옵니다.
            DateTime currentTime = DateTime.Now;
            DateTime currentDate = DateTime.Now;

            // 오전/오후 표시를 포함한 형식으로 시간을 포맷팅합니다.
            string formattedTime = currentTime.ToString("tt hh:mm");

            string formattedDate = currentTime.ToString("yyyy년 MM월 dd일 dddd");


            // 형식화된 시간을 텍스트 요소에 설정합니다.
            currentTimeTextBlock.Text = formattedTime;

            currentDateTextBlock.Text = formattedDate;

        }


        private Dictionary<string, string> weatherDictionary = new Dictionary<string, string>()
        {
            {"thunderstorm with light rain", "가벼운 비를 동반한 천둥구름"},{"thunderstorm with rain", "비를 동반한 천둥구름"},{"thunderstorm with heavy rain", "폭우를 동반한 천둥구름"},
            {"light thunderstorm", "약한 천둥구름"},{"thunderstorm", "천둥구름"},{"heavy thunderstorm", "강한 천둥구름"},{"ragged thunderstorm", "불규칙적 천둥구름"},
            {"thunderstorm with light drizzle", "약한 연무를 동반한 천둥구름"},{"thunderstorm with drizzle", "연무를 동반한 천둥구름"},{"thunderstorm with heavy drizzle", "강한 안개비를 동반한 천둥구름"},
            {"light intensity drizzle", "가벼운 안개비"},{"drizzle", "안개비"},{"heavy intensity drizzle", "강한 안개비"},{"light intensity drizzle rain", "가벼운 적은비"},
            {"drizzle rain", "적은비"},{"heavy intensity drizzle rain", "강한 적은비"},{"shower rain and drizzle", "소나기와 안개비"},
            {"heavy shower rain and drizzle", "강한 소나기와 안개비"},
            {"shower drizzle", "소나기"},
            {"light rain", "악한 비"},
            {"moderate rain", "중간 비"},
            {"heavy intensity rain", "강한 비"},
            {"very heavy rain", "매우 강한 비"},
            {"extreme rain", "극심한 비"},{"freezing rain", "우박"},{"light intensity shower rain", "약한 소나기 비"},{"shower rain", "소나기 비"},
            {"heavy intensity shower rain", "강한 소나기 비"},
            {"ragged shower rain", "불규칙적 소나기 비"},
            {"light snow", "가벼운 눈"},{"snow", "눈"},{"heavy snow", "강한 눈"},
            {"sleet", "진눈깨비"},{"shower sleet", "소나기 진눈깨비"},
            {"light rain and snow", "약한 비와 눈"},
            {"rain and snow", "비와 눈"},{"light shower snow", "약한 소나기 눈"},
            {"shower snow", "소나기 눈"},{"heavy shower snow", "강한 소나기 눈"},
            {"mist", "박무"},
            {"smoke", "연기"},{"haze", "연무"},{"sand, dust whirls", "모래 먼지"},
            {"fog", "안개"},{"sand", "모래"},
            {"dust", "먼지"},
            {"volcanic ash", "화산재"},
            {"squalls", "돌풍"},
            {"tornado", "토네이도"},
            {"clear sky", "구름 한 점 없는 맑은 하늘"},{"few clouds", "약간의 구름이 낀 하늘"},
            {"scattered clouds", "드문드문 구름이 낀 하늘"},
            {"broken clouds", "구름이 거의 없는 하늘"},{"overcast clouds", "구름으로 뒤덮인 흐린 하늘"},{"tropical storm", "태풍"},
            {"hurricane", "허리케인"},
            {"cold", "한랭"},{"hot", "고온"},{"windy", "바람부는"},
            {"hail", "우박"},
            {"calm", "바람이 거의 없는"},
            {"light breeze", "약한 바람"},
            {"gentle breeze", "부드러운 바람"},
            {"moderate breeze", "중간 세기 바람"},{"fresh breeze", "신선한 바람"},{"strong breeze", "센 바람"},
            {"high win", "돌풍에 가까운 센 바람"},{"gale", "돌풍"},
            {"severe gale", "심각한 돌풍"},{"storm", "폭풍"},
            {"violent storm", "강한 폭풍"},
        };






    }

    // 추가 클래스들 정의

    public class WeatherInfo
    {
        public string Temperature { get; set; }
        public string Weather { get; set; }

        public string Icon { get; set; }
    }

    public class OpenWeatherMapResponse
    {
        public Weather[] Weather { get; set; }
        public MainInfo Main { get; set; }
    }

    public class Weather    
    {
        public string Description { get; set; }
        public string Icon { get; set; } // 아이콘 속성 추가

    }

    public class MainInfo
    {
        public float Temp { get; set; }
    }
}
