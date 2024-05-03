using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
// https://docs.google.com/spreadsheets/d/1xlx4gDohutZTMRaaF5IG6alvnAH2S4asyXSjuvUV8tc/edit#gid=0
namespace SwordEnhancement
{
    class Program
    {
        static string spreadsheetId = "1xlx4gDohutZTMRaaF5IG6alvnAH2S4asyXSjuvUV8tc";
        static Stopwatch stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            Console.WriteLine("아이디를 입력해주세요");
            string userId = Console.ReadLine();
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            var service = InitializeGoogleSheetsService();

            try
            {
                string range = $"Gamedata!A:Z";
                SpreadsheetsResource.ValuesResource.GetRequest getRequest = service.Spreadsheets.Values.Get(spreadsheetId, range);
                ValueRange getResponse = getRequest.Execute();
                IList<IList<Object>> userData = getResponse.Values;

                int rowIndex = FindUserRowIndex(userData, userId, today);

                if (rowIndex == -1) // 사용자 데이터가 없는 경우 새로운 데이터 행을 추가
                {
                    // 초기 게임 설정 데이터
                    List<object> newUserRow = new List<object> { userId, today, 1, 95.0, 0.0, 100, 5000000, "00:00:00", 0, 0, 0, 0, 0, 0, 0, 0 };
                    userData.Add(newUserRow); // 로컬 리스트에 추가
                    rowIndex = userData.Count - 1; // 새로 추가된 행의 인덱스
                    ValueRange appendRequest = new ValueRange { Values = new List<IList<object>> { newUserRow } };
                    string appendRange = $"Gamedata!A{rowIndex + 1}:P";
                    SpreadsheetsResource.ValuesResource.AppendRequest request = service.Spreadsheets.Values.Append(appendRequest, spreadsheetId, appendRange);
                    request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                    request.Execute();
                }

                // 게임 시작
                stopwatch.Start();
                PlayGame(userData, rowIndex, userId, service, today);
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류: " + ex.Message);
            }
        }

        static SheetsService InitializeGoogleSheetsService()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = "560915268870-494407a3t83i14eirqk7rmqi69v3rhfj.apps.googleusercontent.com",
                        ClientSecret = "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"
                    },
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("MyAppsToken")).Result,
                ApplicationName = "Google Sheets API .NET Quickstart",
            });
        }

        static int FindUserRowIndex(IList<IList<Object>> userData, string userId, string today)
        {
            for (int i = 0; i < userData.Count; i++)
            {
                IList<Object> row = userData[i];
                if (row.Count >= 2 && row[0].ToString() == userId && row[1].ToString() == today)
                {
                    return i;
                }
            }
            return -1;
        }

        static void PlayGame(IList<IList<Object>> userData, int rowIndex, string userId, SheetsService service, string today)
        {
           
                int enhancementLevel = Convert.ToInt32(userData[rowIndex][2]);
                double successRate = Convert.ToDouble(userData[rowIndex][3]);
                double destructionRate = Convert.ToDouble(userData[rowIndex][4]);
                double enhancementCost = Convert.ToDouble(userData[rowIndex][5]);
                double money = Convert.ToDouble(userData[rowIndex][6]); // 시트에서 올바르게 포맷된지 확인 필요
                TimeSpan totalPlayTime = TimeSpan.Parse(userData[rowIndex][7].ToString());

                int tryCount1to9 = Convert.ToInt32(userData[rowIndex][8]);
                int successCount1to9 = Convert.ToInt32(userData[rowIndex][9]);
                int tryCount10to15 = Convert.ToInt32(userData[rowIndex][10]);
                int successCount10to15 = Convert.ToInt32(userData[rowIndex][11]);
                int tryCount16to21 = Convert.ToInt32(userData[rowIndex][12]);
                int successCount16to21 = Convert.ToInt32(userData[rowIndex][13]);
                int tryCount22to25 = Convert.ToInt32(userData[rowIndex][14]);
                int successCount22to25 = Convert.ToInt32(userData[rowIndex][15]);


                // 게임 실행

                bool playing = true;
                    while (playing && enhancementLevel < 25 && money >= enhancementCost)
                    {
                        double sellPrice = enhancementLevel >= 5 ? enhancementCost * 3 : 0;
                        Console.WriteLine($"현재 강화 단계: {enhancementLevel}단계");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"현재 강화 비용: {enhancementCost:F0}원, ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"현재 성공률: {successRate}% ");

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"파괴 확률: {destructionRate}%, ");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"판매 가격: {sellPrice:F0}원, ");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"남은 돈: {money:F0}원");
                        Console.ResetColor();

                        Console.WriteLine("강화를 시도하시겠습니까? (예: y, 판매: s, 저장후 종료: n)");
                        string userInput = Console.ReadLine();

                        if (userInput.ToLower() == "y")
                        {
                            if (enhancementLevel <= 9)
                                tryCount1to9 += 1;
                            else if (enhancementLevel <= 15)
                                tryCount10to15 += 1;
                            else if (enhancementLevel <= 21)
                                tryCount16to21 += 1;
                            else
                                tryCount22to25 += 1;

                            money -= enhancementCost;
                            Random rnd = new Random();
                            double chance = rnd.NextDouble() * 100;
                            int caseNum = rnd.Next(1, 6);

                            if (enhancementLevel < 10 && chance <= successRate)
                                successCount1to9 += 1;
                            else if (enhancementLevel < 16 && chance <= successRate)
                                successCount10to15++;
                            else if (enhancementLevel < 22 && chance <= successRate)
                                successCount16to21 += 1;
                            else if (enhancementLevel < 23 && chance <= successRate)
                                successCount22to25++;

                            if (chance <= successRate)//검 강화 성공 코드
                            {
                                enhancementLevel++;
                                successRate -= 3;       //강화 확률 3씩 다운
                                enhancementCost *= 1.5; // 강화 성공 시 강화 비용 1.5배 증가
                                Math.Truncate(enhancementCost);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("***************SUCCESS****************");
                                Console.WriteLine("*                                    *");
                                Console.WriteLine("*     성공! 검이 강화되었습니다.     *");
                                Console.WriteLine("*                                    *");
                                Console.WriteLine("***************SUCCESS****************");
                                if (enhancementLevel >= 15)
                                {
                                    destructionRate = (enhancementLevel - 14) * 4; // 5단계 이상부터 파괴 확률 적용
                                }
                            }
                            else if (chance <= successRate + destructionRate && enhancementLevel >= 15)//검 강화 파괴 코드
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(" -------------DESTRUCTION--------------");
                                Console.WriteLine("|                                      |");
                                Console.WriteLine("|      저런.... 검이 파괴되었어요      |");
                                Console.WriteLine("|                                      |");
                                Console.WriteLine(" -------------DESTRUCTION--------------");
                                //파괴후 초기화 수치 조정
                                enhancementLevel = 1;
                                successRate = 95.0;
                                destructionRate = 0.0;
                                enhancementCost = 100;
                            }
                            else   // 파괴되지 않고 강화만 실패한 경우, 하락
                            {
                                enhancementLevel--;
                                successRate += 3;
                                enhancementCost /= 1.5;
                                if (enhancementLevel >= 15)
                                {
                                    destructionRate -= 4; // 4%만큼 감소
                                }
                                else if (enhancementLevel >= 14)
                                {
                                    destructionRate = 0;
                                }
                                Console.ForegroundColor = ConsoleColor.Blue;
                                switch (caseNum)
                                {
                                    case 1:
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        Console.WriteLine("|                                     |");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("        강화 실패... 운이 없군요.      ");
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine("|                                     |");
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        break;
                                    case 2:
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        Console.WriteLine("|                                     |");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("    다음 번엔 꼭 성공하길 바랍니다!    ");
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine("|                                     |");
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        break;
                                    case 3:
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        Console.WriteLine("|                                     |");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("           조금만 더 힘내세요!         ");
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine("|                                     |");
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        break;
                                    case 4:
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        Console.WriteLine("|                                     |");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("   강화는 실패했지만 포기하지 마세요!  ");
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine("|                                     |");
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        break;
                                    case 5:
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        Console.WriteLine("|                                     |");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("          파괴는 안 됐잖아요?          ");
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine("|                                     |");
                                        Console.WriteLine(" ----------------FAIL-----------------");
                                        break;
                                }
                            }
                        }

                        else if (userInput.ToLower() == "s") //s를 누르면 실행되는 코드 
                        {
                            Console.WriteLine($"검을 판매했어요. {sellPrice:F0}원이 들어왔습니다.");
                            int newEnhancementLevel;
                            double newSuccessRate;
                            double newDestructionRate;
                            double newEnhancementCost;
                            SellSword(ref money, enhancementLevel, successRate, destructionRate, enhancementCost, out newEnhancementLevel, out newSuccessRate, out newDestructionRate, out newEnhancementCost);
                            enhancementLevel = newEnhancementLevel;
                            successRate = newSuccessRate;
                            destructionRate = newDestructionRate;
                            enhancementCost = newEnhancementCost;
                        }
                        else if (userInput.ToLower() == "n")
                        {
                            totalPlayTime = totalPlayTime.Add(stopwatch.Elapsed); // stopwatch가 멈추지 않은 상태에서 totalPlayTime을 업데이트
                            stopwatch.Stop();
                            UpdateUserData(userId, rowIndex, today, enhancementLevel, successRate, destructionRate, Math.Floor(enhancementCost), Math.Floor(money), totalPlayTime, service,
                             tryCount1to9, successCount1to9, tryCount10to15, successCount10to15, tryCount16to21, successCount16to21, tryCount22to25, successCount22to25);

                            Console.WriteLine("게임을 종료합니다.");
                            playing = false;
                        }//게임 실행하는 도중에 충전 가능하게 만든 코드
                        else if (userInput == "1000")
                        {
                            money += 1000000;
                            Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                        }
                        else if (userInput == "10000")
                        {
                            money += 5000000;
                            Console.WriteLine("5,000,000원이 충전되었습니다. 게임을 계속합니다.");
                        }


                        if (money < enhancementCost)
                        {
                            // 돈이 부족할 경우 몇 단계인지 강화 확인후 검을 팔거나 충전할수있는 코드

                            Console.WriteLine($"현재 강화 단계: {enhancementLevel}단계");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write($"현재 강화 비용: {enhancementCost:F0}원, ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"현재 성공률: {successRate}% ");

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"파괴 확률: {destructionRate}%, ");

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"판매 가격: {sellPrice:F0}원, ");

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"남은 돈: {money:F0}원");
                            Console.ResetColor();

                            Console.WriteLine("\n돈을 충전하시거나 검을 판매하실 수 있습니다.");
                            Console.WriteLine("\n돈을 충전하시려면 1000을 입력하세요. (종료: n)");
                            Console.WriteLine("\n돈을 충전하시려면 10000을 입력하세요. (종료: n)");
                            Console.WriteLine("\n검을 판매하시겠습니까? (판매: s)");
                            string charge = Console.ReadLine();
                            if (charge.ToLower() == "s")
                            {
                                Console.WriteLine($"검을 판매했어요. {sellPrice:F0}원이 들어왔습니다.");
                                //판매후 초기화
                                int newEnhancementLevel;
                                double newSuccessRate;
                                double newDestructionRate;
                                double newEnhancementCost;
                                SellSword(ref money, enhancementLevel, successRate, destructionRate, enhancementCost, out newEnhancementLevel, out newSuccessRate, out newDestructionRate, out newEnhancementCost);
                                enhancementLevel = newEnhancementLevel;
                                successRate = newSuccessRate;
                                destructionRate = newDestructionRate;
                                enhancementCost = newEnhancementCost;
                            }
                            else if (charge == "1000")
                            {
                                money += 1000000;
                                Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                            }
                            else if (charge == "10000")
                            {
                                money += 5000000;
                                Console.WriteLine("5,000,000원이 충전되었습니다. 게임을 계속합니다.");
                            }
                            else if (userInput.ToLower() == "n")
                            {
                                Console.WriteLine("게임을 종료합니다.");
                                UpdateUserData(userId, rowIndex, today, enhancementLevel, successRate, destructionRate, Math.Floor(enhancementCost), Math.Floor(money), totalPlayTime, service, tryCount1to9, successCount1to9, tryCount10to15, successCount10to15, tryCount16to21, successCount16to21, tryCount22to25, successCount22to25);

                                stopwatch.Stop();
                                playing = false;
                            }
                        }
                    }

                    if (enhancementLevel == 25)
                    {
                        int start_point = 5;
                        int coin_count = 10;
                        for (int i = 0; i < coin_count; i++)
                        {
                            Console.Clear();
                            Console.SetCursorPosition(start_point + i, 5);
                            if (i % 2 == 0)
                                Console.WriteLine("축r(@▽@)J하");
                            else if (i % 2 == 1)
                                Console.WriteLine("축J(@▽@)r하");
                            Thread.Sleep(300);
                        }
                        Console.WriteLine("                            *                                             ");
                        Console.WriteLine("                           * *                                            ");
                        Console.WriteLine("                          *****                                           ");
                        Console.WriteLine("                         *******                                          ");
                        Console.WriteLine("                        *********                                         ");
                        Console.WriteLine("         ***************************************                          ");
                        Console.WriteLine("            *********************************                             ");
                        Console.WriteLine("               ***************************                                ");
                        Console.WriteLine("                  *********************                                   ");
                        Console.WriteLine("                     ***************                                      ");
                        Console.WriteLine("                   ********   ********                                    ");
                        Console.WriteLine("                 ********       ********                                  ");
                        Console.WriteLine("                *******           *******                                 ");
                        Console.WriteLine("               ******               ******                                ");
                        Console.WriteLine("              *****                   *****                               ");
                        Console.WriteLine("             ***                         ***                              ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("25단계 까지 성공했습니다!");

                    }


                
           


        }

        static void UpdateUserData(string userId, int rowIndex, string playDate, int enhancementLevel, double successRate, double destructionRate, double enhancementCost, double money, TimeSpan totalPlayTime, SheetsService service,
                   int tryCount1to9, int successCount1to9, int tryCount10to15, int successCount10to15, int tryCount16to21, int successCount16to21, int tryCount22to25, int successCount22to25)
        {
            List<object> newData = new List<object> { userId, playDate, enhancementLevel, successRate, destructionRate, enhancementCost, money, totalPlayTime.ToString(), tryCount1to9, successCount1to9, tryCount10to15, successCount10to15, tryCount16to21, successCount16to21, tryCount22to25, successCount22to25 };
            string updateRange = $"Gamedata!A{rowIndex + 1}:P{rowIndex + 1}";
            ValueRange updateRequest = new ValueRange { Values = new List<IList<object>> { newData } };
            SpreadsheetsResource.ValuesResource.UpdateRequest request = service.Spreadsheets.Values.Update(updateRequest, spreadsheetId, updateRange);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = request.Execute();
        }
        static void SellSword(ref double money, int enhancementLevel, double successRate, double destructionRate, double enhancementCost, out int newEnhancementLevel, out double newSuccessRate, out double newDestructionRate, out double newEnhancementCost)   //검 판매후 초기화 코드
        {
            if (enhancementLevel >= 5) // 판매는 5단계 이상부터 가능합니다.
            {
                double sellPrice = enhancementLevel >= 5 ? enhancementCost * 3 : 0; //판매가격 강화비용의 3배 
                money += (int)sellPrice;

                // 판매 후 초기화 수치조정 가능
                newEnhancementLevel = 1;
                newSuccessRate = 95.0;
                newDestructionRate = 0.0;
                newEnhancementCost = 100;
            }
            else
            {
                Console.WriteLine("5단계 이상의 검만 팔수있어요!");
                // 초기화하지 않고 기존 값 그대로 반환
                newEnhancementLevel = enhancementLevel;
                newSuccessRate = successRate;
                newDestructionRate = destructionRate;
                newEnhancementCost = enhancementCost;
            }
        }
        static int FindUserRowIndex(IList<IList<Object>> userData, string userId)
        {
            int latestIndex = -1;
            DateTime latestDate = DateTime.MinValue;

            for (int i = 0; i < userData.Count; i++)
            {
                IList<Object> row = userData[i];
                if (row.Count > 1 && row[0].ToString() == userId)
                {
                    DateTime rowDate;
                    if (DateTime.TryParse(row[1].ToString(), out rowDate))
                    {
                        if (rowDate > latestDate)
                        {
                            latestDate = rowDate;
                            latestIndex = i;
                        }
                    }
                }
            }
            return latestIndex;
        }
    }
}
