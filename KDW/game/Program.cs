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
            string userId = Console.ReadLine();   //아이디 입력시 TryLoadUserData 에 저장되있던 아이디 불러옴
            int enhancementLevel;
            double successRate, destructionRate, enhancementCost, money; 

            // 사용자 데이터 불러오기
            if (TryLoadUserData(userId, out enhancementLevel, out successRate, out money, out destructionRate, out enhancementCost))
            {
                Console.WriteLine($"{userId}님, 이전 게임 상태를 불러왔습니다.");
                

                // 데이터 로딩 후 돈이 강화 비용보다 적은지 검사후 출력
                if (money < enhancementCost)
                {
                    bool play = true;
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

                    Console.WriteLine("\n강화 비용이 부족합니다. 돈을 충전하시거나 검을 판매하실 수 있습니다.");
                    Console.WriteLine("\n 돈을 충전하시려면 1000을 입력하세요. (종료: n)");
                    Console.WriteLine("\n돈을 충전하시려면 10000을 입력하세요. (종료: n)");
                    Console.WriteLine($"\n검을 판매하시겠습니까? 판매가격: {sellPrice:F0} (판매: s)");
                    
                    string userinput = Console.ReadLine();

                    if (userinput == "s")
                    {
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
                    else if (userinput == "1000")
                    {
                        money += 1000000;
                        Console.WriteLine("1,000,000원이 충전되었습니다. 게임을 계속합니다.");
                    }
                    else if (userinput == "10000") 
                    {
                        money += 5000000;
                        Console.WriteLine("5,000,000원이 충전되었습니다. 게임을 계속합니다.");
                    }
                    else if (userinput.ToLower() == "n")
                    {
                        Console.WriteLine("게임을 종료합니다.");
                        play = false;
                    }
                }
            }
            else                                 //아이디가 없을경우 새로운 게임 시작
            {
                enhancementLevel = 0;
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
                    money -= enhancementCost;
                    Random rnd = new Random();
                    double chance = rnd.NextDouble() * 100;

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
                        if (enhancementLevel >= 5)
                        {
                            destructionRate = enhancementLevel - 5 + 1; // 5단계 이상부터 파괴 확률 적용
                        }
                    }
                    else if (chance <= successRate + destructionRate && enhancementLevel >= 5)//검 강화 파괴 코드
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" -------------DESTRUCTION--------------");
                        Console.WriteLine("|                                      |");
                        Console.WriteLine("|      저런.... 검이 파괴되었어요      |");
                        Console.WriteLine("|                                      |");
                        Console.WriteLine(" -------------DESTRUCTION--------------");
                        //파괴후 초기화 수치 조정
                        enhancementLevel = 0;
                        successRate = 95.0;
                        destructionRate = 0.0;
                        enhancementCost = 100;
                    }
                    else   // 파괴되지 않고 강화만 실패한 경우, 설정 유지
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(" ----------------FAIL-----------------");
                        Console.WriteLine("|                                     |");
                        Console.WriteLine("|  강화 실패... 이번엔 붙지 않을까요? |");
                        Console.WriteLine("|                                     |");
                        Console.WriteLine(" ----------------FAIL-----------------");
                    }
                }

                else if (userInput.ToLower() == "s") //s를 누르면 실행되는 코드 
                {
                    Console.WriteLine($"검을 판매했어요. {sellPrice}원이 들어왔습니다.");
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
                        playing = false;
                    }
                }
            }

            if (!playing) //플레이를 종료시켯을때 아이디 강화레벨 성공확률 돈 파괴확률 강화비용 저장 코드
            {
                SaveUserData(userId, enhancementLevel, successRate, money, destructionRate, enhancementCost);
            }





           

            static void SellSword(ref double money, int enhancementLevel, double successRate, double destructionRate, double enhancementCost, out int newEnhancementLevel, out double newSuccessRate, out double newDestructionRate, out double newEnhancementCost)   //검 판매후 초기화 코드
            {
                if (enhancementLevel >= 5) // 판매는 5단계 이상부터 가능합니다.
                {
                    double sellPrice = enhancementLevel >= 5 ? enhancementCost * 3 : 0; //판매가격 강화비용의 3배 
                    money += (int)sellPrice;

                    // 판매 후 초기화 수치조정 가능
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
            //유저 데이터 불러오는 코드
            static bool TryLoadUserData(string userId, out int enhancementLevel, out double successRate, out double money, out double destructionRate, out double enhancementCost)
            {
                enhancementLevel = 0; 
                successRate = 95.0;
                money = 1000000.0;
                destructionRate = 0.0; 
                enhancementCost = 100; 

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
            //유저가 게임 종료시에 저장되는 코드
            static void SaveUserData(string userId, int enhancementLevel, double successRate, double money, double destructionRate, double enhancementCost)
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
