// ���� ��Ī�� ����
// ���� ������ 5���� 10��, 4���� 5��, 3���� 2��, 2���� 0��, ��� �ٸ����̸� -10��

#include <iostream>
#include <string>
#include <random>
using namespace std;

void playGame() {
    int score = 0;

    string colors[5] = { "Red", "Blue", "Green", "Yellow", "Orange" };

    // ���� ���� ����
    random_device rd;
    mt19937 gen(rd());

    // ���� ���� (0 �̻� 4 ������ ����)
    uniform_int_distribution<int> dist(0, 4);
    while (true)
    {
        int RedCnt = 0, BlueCnt = 0, GreenCnt = 0, YellowCnt = 0, OrangeCnt = 0;
        for (int i = 0; i < 5; i++) {
            int randomNum = dist(gen);
            cout << i << "�� ���� : " << colors[randomNum] << endl;
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
        cout << "=====���� ����=====" << endl;

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
        cout << "������ ����Ͻðڽ��ϱ�? (Y/N): ";
        cin >> choice;
        if (choice == 'Y' || choice == 'y') {
            cout << "������ �ݺ��մϴ�" << endl;
            continue;
        }
        else if (choice == 'N' || choice == 'n') {
            cout << "���Ӹ޴��� ���ư��ϴ�" << endl;
            break;
        }
    }
}

int main() {
    srand(time(0)); // Seed the random number generator

    string start;
    while (true) {
        cout << "===================================" << endl;
        cout << "������ �����Ͻðڽ��ϱ�?(Y/N) : ";
        cin >> start;
        cout << "===================================" << endl;


        if (start == "Y" || start == "y") {
            cout << "������ �����մϴ�" << endl<<endl;
            cout << "�ش� ������ [���� ��Ī�� ����]���� ���� ������ 5���� 10��, 4���� 5��, 3���� 2��, 2���� 0��, ��� �ٸ����̸� -10�� �Դϴ�" << endl
                << "������ 0�� �̸��� �Ǹ� ���� ���� ȭ������ ���ư��� �˴ϴ�. ������ �������ּ���" << endl << endl;
            playGame();
        }
        else if (start == "N" || start == "n") {
            cout << "������ �����մϴ�" << endl;
            break;
        }
        else {
            cout << "�߸� �Է��ϼ̽��ϴ�" << endl;
            continue;
        }
    }

    return 0;
}