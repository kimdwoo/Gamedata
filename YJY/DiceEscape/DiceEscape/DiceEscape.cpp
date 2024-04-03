#include <iostream>
#include <cstdlib> // for rand() and srand()
#include <ctime> // for time()
#include <fstream> // for file operations
#include <string> // for string operations
#include <chrono> // for delay
#include <thread> // for thread operations

using namespace std;

// Function prototypes
int rollDice();
void playerTurn(int& playerPos, int& killerPos, int turn);
void killerTurn(int& playerPos, int& killerPos, int turn);
void saveGameData(const string& username, int playerPos, int killerPos, int turn);
bool loadGameData(const string& username, int& playerPos, int& killerPos, int& turn);
void delay();
bool askRollDiceOrSave();

int main() {
    srand(static_cast<unsigned int>(time(0))); // 현재 시간을 초단위로 반환하여 시드값으로 활용하여 난수 생성

    string username;
    cout << "다이스 이스케이프에 오신 것을 환영합니다!\n";
    cout << "아이디를 입력하세요: ";
    cin >> username;

    int playerPos = 30, killerPos = 0, turn = 0;
    if (!loadGameData(username, playerPos, killerPos, turn)) {
        cout << "새로운 게임을 시작합니다.\n";
    }
    else {
        cout << "이전에 저장된 게임을 불러왔습니다.\n";
    }

    const int policeStation = 180;

    // Main game loop
    while (true) {
        ++turn; // Increment turn counter

        // Player's turn
        if (askRollDiceOrSave()) {
            cout << "플레이어 턴! (" << turn << "턴)\n";
            delay();
            playerTurn(playerPos, killerPos, turn);
            if (playerPos >= policeStation) {
                cout << "경찰서에 도착하여 승리하셨습니다!\n";
                break;
            }
            if (playerPos <= killerPos) {
                cout << "살인자에게 잡혔습니다! 게임 오버.\n";
                break;
            }
        }
        else {
            // Save game data and exit
            saveGameData(username, playerPos, killerPos, turn);
            cout << "게임 데이터가 저장되었습니다. 종료합니다.\n";
            return 0;
        }

        // Killer's turn
        cout << "살인자의 턴! (" << turn << "턴)\n";
        delay();
        killerTurn(playerPos, killerPos, turn);
        if (playerPos <= killerPos) {
            cout << "살인자에게 잡혔습니다! 게임 오버.\n";
            break;
        }
    }

    return 0;
}

// 주사위 굴리기
int rollDice() {
    return rand() % 6 + 1; // 1 ~ 6의 수를 무작위로 설정
}

// Function for player's turn
void playerTurn(int& playerPos, int& killerPos, int turn) {
    int roll1 = rollDice();
    int roll2 = rollDice();
    cout << "주사위를 굴렸습니다: " << roll1 << " 와 " << roll2 << endl;
    delay();
    playerPos += (roll1 + roll2);
    cout << "이동한 거리: " << (roll1 + roll2) << " 미터\n";
    cout << "현재 위치: " << playerPos - 30 << " 미터\n";
    cout << "살인자와의 거리: " << (playerPos - killerPos) << " 미터\n";
}

// Function for killer's turn
void killerTurn(int& playerPos, int& killerPos, int turn) {
    int roll1 = rollDice();
    int roll2 = rollDice();
    cout << "살인자가 주사위를 굴렸습니다: " << roll1 << " 와 " << roll2 << endl;
    delay();
    killerPos += (roll1 + roll2); // 플레이어를 쫓아가므로 killerPos를 증가.
    cout << "살인자가 이동한 거리: " << (roll1 + roll2) << " 미터\n";
    cout << "플레이어와의 거리: " << (playerPos - killerPos) << " 미터\n";
}

// Function to save game data
void saveGameData(const string& username, int playerPos, int killerPos, int turn) {
    ofstream file(username + ".txt");
    if (file.is_open()) {
        file << playerPos << " " << killerPos << " " << turn;
        file.close();
        cout << "게임 데이터가 저장되었습니다.\n";
    }
    else {
        cout << "게임 데이터를 저장하는 데 문제가 발생했습니다.\n";
    }
}

// Function to load game data
bool loadGameData(const string& username, int& playerPos, int& killerPos, int& turn) {
    ifstream file(username + ".txt");
    if (file.is_open()) {
        file >> playerPos >> killerPos >> turn;
        file.close();
        return true;
    }
    else {
        return false;
    }
}

// Function to add delay
void delay() {
    chrono::milliseconds timespan(1000); // 1초
    this_thread::sleep_for(timespan);
}

// Function to ask the player if they want to roll the dice or save and exit
bool askRollDiceOrSave() {
    char input;
    cout << "주사위를 굴리시겠습니까? (Y/N): ";
    cin >> input;
    if (input == 'Y' || input == 'y') {
        return true;
    }
    else if (input == 'N' || input == 'n') {
        return false;
    }
    else {
        cout << "잘못된 입력입니다. Y 또는 N을 입력하세요.\n";
        return askRollDiceOrSave();
    }
}

