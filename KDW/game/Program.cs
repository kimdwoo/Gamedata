using System;
using System.IO;
using System.Collections.Generic;

namespace SwordEnhancement
{
    class Program
    {
        static string filePath = "userData.txt"; // 사용자 데이터를 저장할 파일
        static void CheckAndRecharge(ref int money, int enhancementCost)
        {
            if (money < enhancementCost)
            {
                Console.WriteLine("\n강화 비용이 부족합니다. 돈을 충전하실래요? 50,000원 충전하려면 1000을 입력하세요. (종료: n)");
                string rechargeInput = Console.ReadLine();
                if (rechargeInput == "1000")
                {
                    money += 100000;
                    Console.WriteLine("100,000원이 충전되었습니다. 게임을 계속합니다.");
                }
                else if (rechargeInput.ToLower() == "n")
                {
                    Console.WriteLine("게임을 종료합니다.");
                    Environment.Exit(0); // 프로그램 종료
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 게임을 종료합니다.");
                    Environment.Exit(0); // 프로그램 종료
                }
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("사용자 아이디를 입력하세요:");
            string userId = Console.ReadLine();
            int enhancementLevel, money, enhancementCost;
            double successRate, destructionRate; // 파괴 확률 초기값은 0.3%에서 시작

            // 사용자 데이터 불러오기
            if (TryLoadUserData(userId, out enhancementLevel, out successRate, out money, out destructionRate, out enhancementCost))
            {
                Console.WriteLine($"{userId}님, 이전 게임 상태를 불러왔습니다.");
                // 데이터 로딩 후 돈이 강화 비용보다 적은지 검사
                if (money < enhancementCost)
                {
                    Console.WriteLine("\n강화 비용이 부족합니다. 돈을 충전하실래요? 100,000원 충전하려면 1000을 입력하세요. (종료: n)");
                    string rechargeInput = Console.ReadLine();
                    if (rechargeInput == "1000")
                    {
                        money += 100000;
                        Console.WriteLine("100,000원이 충전되었습니다.감사합니다.");
                    }
                    else if (rechargeInput.ToLower() == "n")
                    {
                        Console.WriteLine("게임을 종료합니다.");
                        return; // 프로그램 종료
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 게임을 종료합니다.");
                        return; // 프로그램 종료
                    }
                }
            }
            else
            {
                enhancementLevel = 0;
                successRate = 95.0;
                destructionRate = 0.0; // 초기 파괴 확률 설정
                enhancementCost = 100; // 강화 비용 초기 설정
                money = 1000000;
                Console.WriteLine("새 게임을 시작합니다.");
            }

            bool playing = true;

            Console.WriteLine("검 강화 시뮬레이터에 오신 것을 환영합니다.");

            CheckAndRecharge(ref money, enhancementCost);

            while (playing && enhancementLevel < 20 && money >= enhancementCost)
            {
                double sellPrice = enhancementLevel >= 5 ? enhancementCost * 3 : 0;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\n현재 강화 단계: {enhancementLevel}, ");


                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"현재 성공률: {successRate}% ");


                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"파괴 확률: {destructionRate}%, ");


                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"현재 강화 비용: {enhancementCost}원, ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"판매 가격: {sellPrice}원, ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"남은 돈: {money}원");

                // 텍스트 색상을 다시 기본값(보통 흰색 또는 회색)으로 설정
                Console.ResetColor();

                Console.WriteLine("강화를 시도하시겠습니까? (예: y, 판매: s, 저장후 종료: n)");
                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "y")
                {
                    money -= enhancementCost;
                    Random rnd = new Random();
                    double chance = rnd.NextDouble() * 100;

                    if (chance <= successRate)
                    {
                        enhancementLevel++;
                        successRate -= 3;
                        enhancementCost *= 2; // 강화 성공 시 강화 비용 2배 증가
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("*** 성공! 검이 강화되었습니다. ***");
                        if (enhancementLevel >= 5)
                        {
                            destructionRate = Math.Pow(2, enhancementLevel - 5) * 0.3; // 5단계 이상부터 파괴 확률 적용
                        }
                    }
                    else if (chance <= successRate + destructionRate && enhancementLevel >= 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!! 저런... 손이 미끄러졋네요!!!");
                        enhancementLevel = 0;
                        successRate = 95.0;
                        destructionRate = 0.0;
                        enhancementCost = 100;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("강화 실패... 이번엔 붙지 않을까요?");
                        // 파괴되지 않고 강화만 실패한 경우, 설정 유지
                    }
                }
                else if (userInput.ToLower() == "s")
                {
                    if (enhancementLevel >= 5)
                    {
                        money += (int)sellPrice;
                        Console.WriteLine($"검을 판매했습니다. {sellPrice}원이나 벌었어요!!!");
                        // 판매 후 초기화
                        enhancementLevel = 0;
                        successRate = 95.0;
                        destructionRate = 0.0;
                        enhancementCost = 100;
                    }
                    else
                    {
                        Console.WriteLine("5단계 이상의 검만 팔수있어요!");
                    }
                }
                else if (userInput.ToLower() == "n")
                {
                    Console.WriteLine("게임을 종료합니다.");
                    playing = false;
                }
                else
                {
                    Console.WriteLine("이건 강화를위한 입력이 아니네요..");
                }
                if (money < enhancementCost)
                {
                    Console.WriteLine("\n벌써 돈을 다썻어요.. 돈을 충전하실래요? 100,000원 충전하려면 1000을 입력하세요. (종료: n)");
                    string rechargeInput = Console.ReadLine();
                    if (rechargeInput == "1000")
                    {
                        money += 100000;
                        Console.WriteLine("100,000원이 충전되었어요 다시 강화해봐요!");
                    }
                    else if (rechargeInput.ToLower() == "n")
                    {
                        Console.WriteLine("게임을 종료합니다.");
                        playing = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력이에요. 게임을 끌게요.");
                        playing = false;
                        break;
                    }
                }
                if (money < enhancementCost)
                {
                    CheckAndRecharge(ref money, enhancementCost);
                    // 충전 후에도 돈이 부족하면 게임 루프 종료
                    if (money < enhancementCost)
                    {
                        Console.WriteLine("돈이 여전히 부족합니다. 게임을 종료합니다.");
                        break;
                    }
                }
            }
        
            if (!playing)
            {
                SaveUserData(userId, enhancementLevel, successRate, money, destructionRate, enhancementCost);
            }
     

    // 사용자 데이터 불러오기 메서드에 강화 비용 및 파괴 확률 처리 추가
    static bool TryLoadUserData(string userId, out int enhancementLevel, out double successRate, out int money, out double destructionRate, out int enhancementCost)
            {
                enhancementLevel = 0;
                successRate = 95.0;
                money = 1000000;
                destructionRate = 0.0; // 기본값 설정
                enhancementCost = 100; // 기본 강화 비용 설정

                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0] == userId)
                        {
                            enhancementLevel = int.Parse(parts[1]);
                            successRate = double.Parse(parts[2]);
                            money = int.Parse(parts[3]);
                            destructionRate = double.Parse(parts[4]); // 파괴 확률 불러오기
                            enhancementCost = int.Parse(parts[5]); // 강화 비용 불러오기
                            return true;
                        }
                    }
                }

                return false;
            }

           
            static void SaveUserData(string userId, int enhancementLevel, double successRate, int money, double destructionRate, int enhancementCost)
            {
                string[] lines;
                if (File.Exists(filePath))
                {
                    List<string> updatedLines = new List<string>();
                    lines = File.ReadAllLines(filePath);
                    bool found = false;
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0] == userId)
                        {
                            updatedLines.Add($"{userId},{enhancementLevel},{successRate},{money},{destructionRate},{enhancementCost}"); // 강화 비용 및 파괴 확률 저장
                            found = true;
                        }
                        else
                        {
                            updatedLines.Add(line);
                        }
                    }
                    if (!found)
                    {
                        updatedLines.Add($"{userId},{enhancementLevel},{successRate},{money},{destructionRate},{enhancementCost}");
                    }
                    File.WriteAllLines(filePath, updatedLines.ToArray());
                }
                else
                {
                    string newLine = $"{userId},{enhancementLevel},{successRate},{money},{destructionRate},{enhancementCost}";
                    File.WriteAllText(filePath, newLine + Environment.NewLine);
                }
            }

        }
    }
}


