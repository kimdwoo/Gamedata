using System;
using System.IO;
using System.Collections.Generic;

namespace SwordEnhancement
{
    class Program
    {
        static string filePath = "userData.txt"; // 사용자 데이터를 저장할 파일

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

                    Console.WriteLine("\n강화 비용이 부족합니다. 돈을 충전하시거나 검을 판매하실 수 있습니다.");
                    Console.WriteLine("\n 돈을 충전하시려면 1000을 입력하세요. (종료: n)");
                    Console.WriteLine("\n돈을 충전하시려면 10000을 입력하세요. (종료: n)");
                    Console.WriteLine("\n검을 판매하시겠습니까? (판매: s)");
                    string userinp = Console.ReadLine();

                    if (userinp == "s")
                    {

                        int newEnhancementLevel;
                        double newSuccessRate;
                        double newDestructionRate;
                        int newEnhancementCost;
                        SellSword(ref money, enhancementLevel, successRate, destructionRate, enhancementCost, out newEnhancementLevel, out newSuccessRate, out newDestructionRate, out newEnhancementCost);
                        enhancementLevel = newEnhancementLevel;
                        successRate = newSuccessRate;
                        destructionRate = newDestructionRate;
                        enhancementCost = newEnhancementCost;
                        
                    }
                    else if (userinp == "1000")
                    {
                        money += 100000;
                        Console.WriteLine("100,000원이 충전되었습니다. 게임을 계속합니다.");
                    }
                    else if (userinp == "10000") // 10000 입력 시 1000000원 충전
                    {
                        money += 1000000;
                        Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                    }
                    
                }
            }
            else
            {
                enhancementLevel = 0;  //아이디가 없을경우 새로운 게임 시작
                successRate = 95.0; //수치조정
                destructionRate = 0.0;
                enhancementCost = 100;
                money = 1000000;
                Console.WriteLine("새 게임을 시작합니다.");
            }

            bool playing = true;





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

                // 텍스트 색상을 다시 기본값으로 설정
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
                            destructionRate = enhancementLevel - 5 + 1; // 5단계 이상부터 파괴 확률 적용
                        }
                    }
                    else if (chance <= successRate + destructionRate && enhancementLevel >= 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("!!! 저런... 손이 미끄러졌네요!!!");
                        //파괴후 초기화 수치조정
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
                    Console.WriteLine($"검을 판매했어요. {sellPrice}원이 들어왔습니다.");
                    int newEnhancementLevel;
                    double newSuccessRate;
                    double newDestructionRate;
                    int newEnhancementCost;
                    SellSword(ref money, enhancementLevel, successRate, destructionRate, enhancementCost, out newEnhancementLevel, out newSuccessRate, out newDestructionRate, out newEnhancementCost);
                    enhancementLevel = newEnhancementLevel;
                    successRate = newSuccessRate;
                    destructionRate = newDestructionRate;
                    enhancementCost = newEnhancementCost;
                }
                else if (userInput.ToLower() == "n")
                {
                    Console.WriteLine("게임을 종료합니다.");
                    playing = false;
                }
                else if (userInput == "1000")
                {
                    money += 100000;
                    Console.WriteLine("100,000원이 충전되었습니다. 게임을 계속합니다.");
                }
                else if (userInput == "10000")
                {
                    money += 1000000;
                    Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                }


                if (money < enhancementCost)
                {
                    // 돈이 부족할 경우 확인할수있게
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

                    // 텍스트 색상을 다시 기본값으로 설정
                    Console.ResetColor();
                    Console.WriteLine("\n돈을 충전하시거나 검을 판매하실 수 있습니다.");
                    Console.WriteLine("\n돈을 충전하시려면 1000을 입력하세요. (종료: n)");
                    Console.WriteLine("\n돈을 충전하시려면 10000을 입력하세요. (종료: n)");
                    Console.WriteLine("\n검을 판매하시겠습니까? (판매: s)");
                    string charge = Console.ReadLine();
                    if (charge.ToLower() == "s")
                    {
                        Console.WriteLine($"검을 판매했어요. {sellPrice}원이 들어왔습니다.");
                        int newEnhancementLevel;
                        double newSuccessRate;
                        double newDestructionRate;
                        int newEnhancementCost;
                        SellSword(ref money, enhancementLevel, successRate, destructionRate, enhancementCost, out newEnhancementLevel, out newSuccessRate, out newDestructionRate, out newEnhancementCost);
                        enhancementLevel = newEnhancementLevel;
                        successRate = newSuccessRate;
                        destructionRate = newDestructionRate;
                        enhancementCost = newEnhancementCost;
                    }
                    else if (userInput == "1000")
                    {
                        money += 100000;
                        Console.WriteLine("100,000원이 충전되었습니다. 게임을 계속합니다.");
                    }
                    else if (userInput == "10000")
                    {
                        money += 1000000;
                        Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                    }
                }
            }

            if (!playing)
            {
                SaveUserData(userId, enhancementLevel, successRate, money, destructionRate, enhancementCost);
            }





            static void AskForAction(ref int money, int enhancementCost, string userId, int enhancementLevel, double successRate, double destructionRate, int sellPrice)
            {
                Console.WriteLine("\n 검을 판매하시겠습니까? (판매: s)");
                Console.WriteLine("\n돈을 충전하시려면 1000을 입력하세요. (종료: n)");
                Console.WriteLine(" 돈을 충전하시려면 10000을 입력하세요. (종료: n)");
                string userInp = Console.ReadLine();

                int newEnhancementLevel;
                double newSuccessRate;
                double newDestructionRate;
                int newEnhancementCost;

                if (userInp == "s")
                {
                    SellSword(ref money, enhancementLevel, successRate, destructionRate, enhancementCost, out newEnhancementLevel, out newSuccessRate, out newDestructionRate, out newEnhancementCost);
                    enhancementLevel = newEnhancementLevel;
                    successRate = newSuccessRate;
                    destructionRate = newDestructionRate;
                    enhancementCost = newEnhancementCost;
                    Console.WriteLine($"검을 판매했어요. {sellPrice}원 이 들어왔습니다.");
                }
                else if (userInp == "1000")
                {
                    money += 100000;
                    Console.WriteLine("100,000원이 충전되었습니다. 게임을 계속합니다.");
                }
                else if (userInp == "10000") // 10000 입력 시 1000000원 충전
                {
                    money += 1000000;
                    Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                }
            }

            static void SellSword(ref int money, int enhancementLevel, double successRate, double destructionRate, int enhancementCost, out int newEnhancementLevel, out double newSuccessRate, out double newDestructionRate, out int newEnhancementCost)
            {
                if (enhancementLevel >= 5) // 판매는 5단계 이상부터 가능합니다.
                {
                    double sellPrice = enhancementLevel >= 5 ? enhancementCost * 3 : 0;
                    money += (int)sellPrice;

                    // 판매 후 초기화
                    newEnhancementLevel = 0;
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

            static bool TryLoadUserData(string userId, out int enhancementLevel, out double successRate, out int money, out double destructionRate, out int enhancementCost)
            {
                enhancementLevel = 0; //사용자 데이터 수치조정
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
                            updatedLines.Add($"{userId},{enhancementLevel},{successRate},{money},{destructionRate},{enhancementCost}");
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
