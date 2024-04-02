using System;
using System.Threading;

namespace RACEGAME
{
    internal class Program
    {
        static void Main(string[] args) // 메인 창
        {
            while (true)
            {
                Random rand = new Random();

                Console.WriteLine("===================================");
                Console.Write("게임을 실행하시겠습니까?(Y/N) : ");
                string start = Console.ReadLine().ToLower();
                Console.WriteLine("===================================");

                if (start == "y" || start == "Y")
                {
                    Console.WriteLine("게임을 실행합니다\n");
                    Console.WriteLine("===========================================================");
                    Console.WriteLine("해당 게임은 [레이스 게임]으로 경주를 ");
                    Console.WriteLine("게임은 당신을 포함한 총 6명이 진행을 하게 되고 3위 이상에 들지 못하게 되면 게임은 종료됩니다.");
                    Console.WriteLine("이점에 유의해주세요");
                    Console.WriteLine("자신이 원하는 차량을 선택하면 바로 게임을 시작합니다.");
                    Console.WriteLine("===========================================================\n");

                    char selectedCar = SelectCar(); // 차 선택

                    RACE(rand, selectedCar, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' });

                    if (selectedCar >= 'a' && selectedCar <= 'f')
                    {
                        int playerPosition = Array.IndexOf(new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, selectedCar);
                        if (playerPosition >= 3)
                        { 
                            continue;
                        }
                        else
                        {                            
                            return;
                        }
                    }
                }
                else if (start == "n" || start == "N")
                {
                    Console.WriteLine("게임을 종료합니다");
                    break;
                }
                else
                {
                    Console.WriteLine("잘못 입력하셨습니다");
                    Console.WriteLine("===========================================================");
                    continue;
                }
            }
        }

        static char SelectCar()
        {
            while (true)
            {
                Console.Write("본인의 차를 선택하세요 (a, b, c, d, e, f): ");
                char selectedCar = char.ToLower(Console.ReadKey().KeyChar);
                Console.WriteLine();
                if (selectedCar >= 'a' && selectedCar <= 'f')
                    return selectedCar;
                else
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
            }
        }

        static void RACE(Random rand, char selectedCar, char[] PLAYER) // 레이스 게임 
        {
            Console.WriteLine("===========================================================");
            int[] PLACE = { 1, 2, 3, 4, 5, 6 }; // 순위 선언
            int cnt = 1;
            Shuffle(PLACE, rand);
            Console.WriteLine(cnt + "차 순위 집계");
            for (int i = 0; i < PLAYER.Length; i++)
            {
                Console.WriteLine($"{PLAYER[i]} = {PLACE[i]}");
            }

            // 5초마다 변경
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 60)
            {
                cnt++;
                Console.WriteLine("===========================================================");
                Console.WriteLine(cnt + "차 순위 집계");
                Thread.Sleep(5000); // 5초 대기
                Shuffle(PLACE, rand);
                for (int i = 0; i < PLAYER.Length; i++)
                {
                    Console.WriteLine($"{PLAYER[i]} = {PLACE[i]}");
                }

            }
            Console.WriteLine("===========================================================");
            Console.WriteLine("순위 발표");
            for (int i = 0; i < PLACE.Length; i++)
            {
                Console.WriteLine($"{i + 1}등 : {PLAYER[i]}");
            }

            int playerPosition = Array.IndexOf(PLAYER, selectedCar); // 플레이어의 위치 확인
            if (playerPosition >= 3)
            {
                Console.WriteLine($"당신이 선택한 차({selectedCar})가 4등 이상입니다.");
                Console.WriteLine("게임이 종료되었습니다. 다시 시작하려면 Y를 누르세요.");
                return;
            }
            else
            {
                Console.WriteLine($"당신이 선택한 차({selectedCar})가 4등 이하입니다.");
                Console.WriteLine("프로그램을 종료합니다.");
                return;
            }
        }

        static void Shuffle(int[] array, Random random)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                int temp = array[k];
                array[k] = array[n];
                array[n] = temp;
            }
        }
    }
}
