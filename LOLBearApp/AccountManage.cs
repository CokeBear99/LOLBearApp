using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace LOLBearApp
{
    public class AccountManage
    {

        private Dictionary<string, List<object>> accountDict;



        public AccountManage()
        {
            // 딕셔너리 초기화
            accountDict = new Dictionary<string, List<object>>();
        }




        public async Task AddAccount(string nickname, string id, string password)
        {

            try
            {
                var leagueInfo = await GetInfo(nickname);
                string tier = leagueInfo.Item1;
                string rank = leagueInfo.Item2;
                int leaguePoints = leagueInfo.Item3;
                int wins = leagueInfo.Item4;
                int losses = leagueInfo.Item5;
                int winrate = leagueInfo.Item6;


                // IronPannel 인스턴스 생성
                IronPannel ironPanel = new IronPannel();
                // IronA1 텍스트 설정
                ironPanel.SetIronA1Text(tier);



                // 여기에서 받아온 값들을 사용합니다.

                // 입력된 정보를 딕셔너리에 추가
                List<object> accountInfo = new List<object> { id, password, tier, rank, leaguePoints, wins, losses, winrate };
                accountDict.Add(nickname, accountInfo);

                string message = "계정 정보:\n";
                foreach (var entry in accountDict)
                {
                    message += $"Nickname: {entry.Key}\nID: {entry.Value[0]} Password: {entry.Value[1]}\n티어 : {entry.Value[2]} {entry.Value[3]} {entry.Value[4]}LP\n{entry.Value[5]}승 {entry.Value[6]}패 [{entry.Value[7]}%]\n";
                }
                System.Windows.MessageBox.Show(message, "계정 정보", MessageBoxButton.OK, MessageBoxImage.Information);



            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Error adding account: {e.Message}");
            }



        }








        public async Task<(string Tier, string Rank, int LeaguePoints, int Wins, int Losses, int WinRate)> GetInfo(string nickname)
        {
            // <API KEY>
            string apiKey = "RGAPI-afb7e062-d6bc-405d-ac01-180db68c1288"; // 사용자의 Riot Games API 키

            // <닉네임#네임태그 분리>
            string InNickName_Tag = nickname;
            int index = InNickName_Tag.IndexOf('#');
            string Nickname = InNickName_Tag.Substring(0, index).Trim();
            string Nametag = InNickName_Tag.Substring(index + 1).Trim().ToUpper();


            // <정보 추출 3단계>

            // PUUID 얻기
            string puuidUrl = $"https://asia.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{Nickname}/{Nametag}?api_key={apiKey}";
            string puuidResponse = await GetDataFromApi(puuidUrl);
            string puuid = ExtractPuuidFromJson(puuidResponse);

            if (puuid != null)
            {
                // Summoner ID 얻기
                string summonerIdUrl = $"https://kr.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{puuid}?api_key={apiKey}";
                string summonerIdResponse = await GetDataFromApi(summonerIdUrl);
                string summonerId = ExtractSummonerIdFromJson(summonerIdResponse);

                if (summonerId != null)
                {
                    // 소환사 리그 정보 가져오기
                    string leagueUrl = $"https://kr.api.riotgames.com/lol/league/v4/entries/by-summoner/{summonerId}?api_key={apiKey}";
                    string leagueInfo = await GetDataFromApi(leagueUrl);


                    //정보 추출 로직 
                    try
                    {
                        JArray leagueArray = JArray.Parse(leagueInfo);

                        foreach (JObject league in leagueArray)
                        {
                            string tier = league["tier"].ToString();
                            string rank = league["rank"].ToString();
                            int leaguePoints = league["leaguePoints"].ToObject<int>();
                            int wins = league["wins"].ToObject<int>();
                            int losses = league["losses"].ToObject<int>();

                            double WinRateCalculate = (double)wins / (wins + losses); // 승률 계산
                            int WinRate = (int)(WinRateCalculate * 100); // 백분율로 변환

                            // 티어, 랭크, 리그 포인트를 반환합니다.
                            return (tier, rank, leaguePoints, wins, losses, WinRate);
                        }


                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show($"Error displaying league information: {e.Message}");

                    }


                }
            }
            return (null, null, 0, 0, 0, 0);

        }




        //데이터 가져오기 로직

        public async Task<string> GetDataFromApi(string apiUrl)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    System.Windows.MessageBox.Show($"Error: {e.Message}");
                    return null;
                }
            }
        }




        //Puuid 추출 로직 

        public string ExtractPuuidFromJson(string jsonResponse)
        {
            try
            {
                // JSON 파싱하여 PUUID 추출
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                return data["puuid"];
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Error extracting PUUID from JSON response: {e.Message}");
                return null;
            }
        }
                



        //SummonerID 추출 로직

        public string ExtractSummonerIdFromJson(string jsonResponse)
        {
            try
            {
                // JSON 파싱하여 Summoner ID 추출
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                return data["id"];
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Error extracting Summoner ID from JSON response: {e.Message}");
                return null;
            }
        }



    }

}






       