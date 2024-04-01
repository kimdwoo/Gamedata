// 색깔 빠칭코 게임
// 같은 색깔이 5개면 10점, 4개면 5점, 3개면 2점, 2개면 0점, 모두 다른색이면 -10점

#include <iostream>
#include <string>
#include <random>
using namespace std;

void playGame() {
    int score = 0;

    string colors[5] = { "Red", "Blue", "Green", "Yellow", "Orange" };

    // 랜덤 엔진 생성
    random_device rd;
    mt19937 gen(rd());

    // 분포 설정 (0 이상 4 이하의 정수)
    uniform_int_distribution<int> dist(0, 4);
    while (true)
    {
        int RedCnt = 0, BlueCnt = 0, GreenCnt = 0, YellowCnt = 0, OrangeCnt = 0;
        for (int i = 0; i < 5; i++) {
            int randomNum = dist(gen);
            cout << i << "번 색깔 : " << colors[randomNum] << endl;
            if (randomNum == 0) {
                RedCnt += 1;
            }
            else if (randomNum == 1) {
                BlueCnt += 1;
            }
            else if (randomNum == 2) {
                GreenCnt += 1;
            }
            else if (randomNum == 3) {
                YellowCnt += 1;
            }
            else {
                OrangeCnt += 1;
            }
        }
        cout << endl;
        cout << "=====점수 집계=====" << endl;

        if (RedCnt == 5 || BlueCnt == 5 || GreenCnt == 5 || YellowCnt == 5 || OrangeCnt == 5)
        {
            score += 10;
        }
        else if (RedCnt == 4 || BlueCnt == 4 || GreenCnt == 4 || YellowCnt == 4 || OrangeCnt == 4)
        {
            score += 5;
        }
        else if (RedCnt == 3 || BlueCnt == 3 || GreenCnt == 3 || YellowCnt == 3 || OrangeCnt == 3)
        {
            score += 2;
        }
        else if (RedCnt == 2 || BlueCnt == 2 || GreenCnt == 2 || YellowCnt == 2 || OrangeCnt == 2)
        {
            score += 0;
        }
        else if (RedCnt == 1 || BlueCnt == 1 || GreenCnt == 1 || YellowCnt == 1 || OrangeCnt == 1)
        {
            score -= 10;
        }
        cout << "SCORE : " << score << endl;
        cout << endl<< "====================" << endl;
        if (score < 0) {
            break;
        }
        char choice;
        cout << "게임을 계속하시겠습니까? (Y/N): ";
        cin >> choice;
        if (choice == 'Y' || choice == 'y') {
            cout << "게임을 반복합니다" << endl;
            continue;
        }
        else if (choice == 'N' || choice == 'n') {
            cout << "게임메뉴로 돌아갑니다" << endl;
            break;
        }
    }
}

int main() {
    srand(time(0)); // Seed the random number generator

    string start;
    while (true) {
        cout << "===================================" << endl;
        cout << "게임을 실행하시겠습니까?(Y/N) : ";
        cin >> start;
        cout << "===================================" << endl;


        if (start == "Y" || start == "y") {
            cout << "게임을 실행합니다" << endl<<endl;
            cout << "해당 게임은 [색깔 빠칭코 게임]으로 같은 색깔이 5개면 10점, 4개면 5점, 3개면 2점, 2개면 0점, 모두 다른색이면 -10점 입니다" << endl
                << "점수가 0점 미만이 되면 게임 메인 화면으로 돌아가게 됩니다. 이점에 유의해주세요" << endl << endl;
            playGame();
        }
        else if (start == "N" || start == "n") {
            cout << "게임을 종료합니다" << endl;
            break;
        }
        else {
            cout << "잘못 입력하셨습니다" << endl;
            continue;
        }
    }

    return 0;
}