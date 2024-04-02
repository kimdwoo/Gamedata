using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Random rand = new Random();

        while (true)
        {
            Console.WriteLine("===================================");
            Console.Write("게임을 실행하시겠습니까?(Y/N) : ");
            string start = Console.ReadLine().ToLower();
            Console.WriteLine("===================================");

            if (start == "y")
            {
                Console.WriteLine("게임을 실행합니다\n");
                Console.WriteLine("해당 게임은 [색깔 빠칭코 게임]으로 같은 색깔이 5개면 10점, 4개면 5점, 3개면 2점, 2개면 0점, 모두 다른색이면 -10점 입니다");
                Console.WriteLine("점수가 0점 미만이 되면 게임 메인 화면으로 돌아가게 됩니다. 이점에 유의해주세요\n");
                ColorGame(rand);
            }
            else if (start == "n")
            {
                Console.WriteLine("게임을 종료합니다");
                break;
            }
            else
            {
                Console.WriteLine("잘못 입력하셨습니다");
                continue;
            }
        }
    }

    static void ColorGame(Random rand)
    {
        int score = 0;

        string[] colors = { "Red", "Blue", "Green", "Yellow", "Orange" };

        while (true)
        {
            int RedCnt = 0, BlueCnt = 0, GreenCnt = 0, YellowCnt = 0, OrangeCnt = 0;

            for (int i = 0; i < 5; i++)
            {
                int randomNum = rand.Next(0, 5);
                Console.WriteLine($"{i}번 색깔 : {colors[randomNum]}");

                switch (randomNum)
                {
                    case 0:
                        RedCnt++;
                        break;
                    case 1:
                        BlueCnt++;
                        break;
                    case 2:
                        GreenCnt++;
                        break;
                    case 3:
                        YellowCnt++;
                        break;
                    case 4:
                        OrangeCnt++;
                        break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("=====점수 집계=====");

            if (RedCnt == 5 || BlueCnt == 5 || GreenCnt == 5 || YellowCnt == 5 || OrangeCnt == 5)
                score += 10;
            else if (RedCnt == 4 || BlueCnt == 4 || GreenCnt == 4 || YellowCnt == 4 || OrangeCnt == 4)
                score += 5;
            else if (RedCnt == 3 || BlueCnt == 3 || GreenCnt == 3 || YellowCnt == 3 || OrangeCnt == 3)
                score += 2;
            else if (RedCnt == 2 || BlueCnt == 2 || GreenCnt == 2 || YellowCnt == 2 || OrangeCnt == 2)
                score += 0;
            else if (RedCnt == 1 || BlueCnt == 1 || GreenCnt == 1 || YellowCnt == 1 || OrangeCnt == 1)
                score -= 10;

            Console.WriteLine($"SCORE : {score}\n");
            Console.WriteLine("====================");

            if (score < 0)
            {
                Console.WriteLine("점수가 0점 미만입니다. 게임 메인 화면으로 돌아갑니다.");
                break;
            }

            Console.Write("게임을 계속하시겠습니까? (Y/N): ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (char.ToLower(choice) == 'n')
            {
                Console.WriteLine("게임메뉴로 돌아갑니다");
                break;
            }
            else if (char.ToLower(choice) != 'y')
            {
                Console.WriteLine("잘못된 입력입니다. 게임메뉴로 돌아갑니다");
                break;
            }
            Console.WriteLine("게임을 반복합니다\n");
        }
    }
}
