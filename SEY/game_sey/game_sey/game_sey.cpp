#include <iostream>
#include <fstream>
#include <cstdlib>
#include <ctime>

using namespace std;

// 게임 데이터 구조체
struct GameData {
    int stage;
    int coin;
};

// 게임 시작 화면 출력
void printTitleScreen() {
    system("cls"); // 화면 지우기
    cout << "******************************************" << endl;
    cout << "*                                        *" << endl;
    cout << "*           데이트 코스 강화하기         *" << endl;
    cout << "*   1. 게임 시작                         *" << endl;
    cout << "*   2. 게임 불러오기                     *" << endl;
    cout << "*   3. 게임 종료                         *" << endl;
    cout << "******************************************" << endl;
}

// 게임 화면 출력
void printGameScreen(int stage, int coin) {
    system("cls");
    cout << "******************************************" << endl;
    cout << "*                                        *" << endl;
    cout << "스테이지 " << stage << " - ";
    switch (stage) {
    case 1:
        cout << "편의점 삼각김밥 데이트";
        break;
    case 2:
        cout << "편의점 삼각김밥 + 라면 데이트";
        break;
    case 3:
        cout << "김밥천국 데이트";
        break;
    case 4:
        cout << "롯데리아 데이트";
        break;
    case 5:
        cout << "쿠우쿠우 데이트";
        break;
    case 6:
        cout << "카페 투어 데이트";
        break;
    case 7:
        cout << "롯데월드 데이트";
        break;
    case 8:
        cout << "유명 쉐프의 레스토랑 데이트";
        break;
    case 9:
        cout << "5성급 호캉스 데이트";
        break;
    case 10:
        cout << "고백하기";
        break;
    }
    cout << endl;
    cout << "*                                        *" << endl;
    cout << "*   1. 강화                              *" << endl;
    cout << "*   2. 팔기                              *" << endl;
    cout << "*   3. 게임 저장                         *" << endl;
    cout << "*   4. 게임 종료                         *" << endl;
    cout << "*                                        *" << endl;
    cout << "코인: " << coin << "원" << endl;
    cout << "******************************************" << endl;
}

// 게임 데이터를 파일로 저장하는 함수
void saveGameData(const string& id, const GameData& data) {
    ofstream outFile(id + ".txt");
    if (outFile.is_open()) {
        outFile << "게임 스테이지 : " << data.stage << " " << "보유 코인 : " << data.coin << endl;
        outFile.close();
        cout << "게임 데이터가 저장되었습니다." << endl;
    }
    else {
        cout << "게임 데이터를 저장 실패했습니다." << endl;
    }
}

// 파일에서 게임 데이터를 읽어오는 함수
GameData loadGameData(const string& id) {
    GameData data;
    ifstream inFile(id + ".txt");
    if (inFile.is_open()) {
        if (inFile >> data.stage >> data.coin) {
            inFile.close();
            cout << "게임 데이터가 불러와졌습니다." << endl;
            return data;
        }
        else {
            cout << "저장된 게임 데이터가 올바르지 않습니다." << endl;
            inFile.close();
        }
    }
    else {
        cout << "저장된 게임 데이터가 없습니다." << endl;
    }
    // 기본값 설정
    data.stage = 1;
    data.coin = 50000;
    return data;
}

int main() {
    int stage = 1; // 게임 스테이지
    int coin = 50000; // 게임 코인

    while (true) {
        printTitleScreen(); // 타이틀 화면 출력

        // 사용자 입력 받기
        int choice;
        cout << "선택하세요: ";
        cin >> choice;

        if (choice == 1) {
            // 게임 시작
            coin = 50000; // 코인 초기화
            stage = 1; // 스테이지 초기화

            srand(time(nullptr)); // 랜덤 시드 설정 
            // 게임 내에서 난수를 생성할 때 매번 다른 시드를 사용하여 난수를 생성함.  매번 같은 시드를 사용한다면 같은 순서의 난수가 생성되어 예상 가능한 결과가 
            // 나올 수 있기에 실행 할 때마다 다른 순서의 난수가 생성되어 예상 할 수 없음

            while (stage <= 10) {
                printGameScreen(stage, coin); // 게임 화면 출력

                // 사용자 입력 받기
                int action;
                cout << "선택하세요: ";
                cin >> action;

                if (action == 1) {
                    // 강화
                    // 강화 성공 여부 결정
                    int success = rand() % 100 + 1; // 1부터 100까지의 난수 생성
                    int successRate;
                    switch (stage) {
                    case 1:
                        successRate = 100;
                        break;
                    case 2:
                        successRate = 90;
                        break;
                    case 3:
                        successRate = 75;
                        break;
                    case 4:
                        successRate = 63;
                        break;
                    case 5:
                        successRate = 45;
                        break;
                    case 6:
                        successRate = 30;
                        break;
                    case 7:
                        successRate = 18;
                        break;
                    case 8:
                        successRate = 10;
                        break;
                    case 9:
                        successRate = 8;
                        break;
                    case 10:
                        successRate = 4;
                        break;
                    default:
                        successRate = 0; // 예외 처리
                    }
                    if (success <= successRate) { // 각 스테이지 강화 성공 확률 
                        cout << "강화에 성공했습니다!" << endl;
                        coin -= stage * 2000; // 각 스테이지 강화 비용
                        stage++; // 다음 스테이지
                    }
                    else {
                        cout << "강화에 실패했습니다." << endl;
                        stage = 1; // 실패하면 1스테이지로 돌아감
                    }
                }
                else if (action == 2) {
                    // 팔기
                    int price;
                    switch (stage) {
                    case 2:
                        price = 4000;
                        break;
                    case 3:
                        price = 8000;
                        break;
                    case 4:
                        price = 12000;
                        break;
                    case 5:
                        price = 18000;
                        break;
                    case 6:
                        price = 25000;
                        break;
                    case 7:
                        price = 32000;
                        break;
                    case 8:
                        price = 40000;
                        break;
                    case 9:
                        price = 45000;
                        break;
                    case 10:
                        price = 50000;
                        break;
                    default:
                        price = 0; // 예외 처리
                    }
                    cout << "판매 가격: " << price << "원" << endl;
                    cout << "정말로 팔겠습니까? (1: 예, 2: 아니오): ";
                    int sellChoice;
                    cin >> sellChoice;
                    if (sellChoice == 1) {
                        coin += price;
                        stage = 1; // 판매 후 1스테이지로 돌아감
                        cout << "판매되었습니다." << endl;
                    }
                }
                else if (action == 3) { // 게임 저장
                    string id;
                    cout << "저장할 아이디를 입력하세요: ";
                    cin >> id;
                    GameData data = { stage, coin };
                    saveGameData(id, data);
                }
                else if (action == 4) {
                    // 게임 종료
                    cout << "게임을 종료합니다." << endl;
                    return 0;
                }

                // 게임 오버 조건 확인
                if (coin <= 0) {
                    cout << "게임 오버입니다." << endl;
                    break;
                }
                // 게임 승리 조건 확인
                if (stage > 10) {
                    cout << "게임 승리입니다!" << endl;
                    break;
                }
            }
        }
        else if (choice == 2) { // 게임 불러오기
            string id;
            cout << "불러올 아이디를 입력하세요: ";
            cin >> id;
            GameData data = loadGameData(id);
            if (data.stage > 0 && data.stage <= 10 && data.coin >= 0) {
                stage = data.stage;
                coin = data.coin;
            }
            else {
                cout << "유효하지 않은 게임 데이터입니다." << endl;
                continue; // 다시 타이틀 화면으로 돌아감
            }
}
        else if (choice == 3) {
            // 게임 종료
            cout << "게임을 종료합니다." << endl;
            break;
        }
    }

    return 0;
}