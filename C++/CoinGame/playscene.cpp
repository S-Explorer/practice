#include "playscene.h"
#include <QDebug>
#include <QAction>
#include <QMenuBar>
#include <QPainter>
#include "mypushbutton.h"
#include <QLabel>
#include "mycoin.h"
#include "dataconfig.h"
#include <QPropertyAnimation>
#include <QSound>

PlayScene::PlayScene(int levelNum) : levelIndex(levelNum)
{
    qDebug()<<"get in level";
    this->setFixedSize(320,588);
    this->setWindowIcon(QPixmap(":/res/Coin0001.png"));
    this->setWindowTitle("关卡:" + QString::number(levelNum));

    QMenuBar * bar = menuBar();
    setMenuBar(bar);

    QMenu * startBar = bar->addMenu("选择");
    QAction *exitAction = startBar->addAction("退出");

    connect(exitAction,&QAction::triggered,[=](){
            this->close();
    });

    QSound *levelBackSound = new QSound(":/res/BackButtonSound.wav",this);
    QSound *coinSound = new QSound(":/res/ConFlipSound.wav",this);

    //return button
    MyPushButton * backButton = new MyPushButton(":/res/BackButton.png",":/res/BackButtonSelected.png");
    backButton->setParent(this);
    backButton->move(this->width() - backButton->width(),this->height() - backButton->height());

    connect(backButton,&QPushButton::clicked,[=](){
        qDebug()<<"back";

        levelBackSound->play();
        QTimer::singleShot(300,this,[=](){
            this->hide();
            emit this->levelSceneBack();
        });
    });

    //show the level number
    QLabel *levelBar =new QLabel;
    levelBar->setParent(this);
    QFont font;
    font.setFamily("楷体");
    font.setPointSize(15);
    levelBar->setFont(font);
    QString str = QString("关卡:%1").arg(levelNum);
    levelBar->setText(str);
    levelBar->setGeometry(30,this->height()-50,120,50);

    dataConfig config;
    for (int o = 0; o < 4; o++) {
        for (int j = 0; j < 4; j++) {
            this->gameArray[o][j] = config.mData[this->levelIndex][o][j];
        }
    }

    QLabel *winLabel = new QLabel;
    QPixmap temPix;
    temPix.load(":/res/LevelCompletedDialogBg.png");
    winLabel->setGeometry(0,0,temPix.width(),temPix.height());
    winLabel->setPixmap(temPix);
    winLabel->setParent(this);
    winLabel->move((this->width() - temPix.width())*0.5,-temPix.height());


    //show coin background
    for (int i = 0;i < 4 ; i++) {
        for (int j = 0; j < 4 ; j++) {
            QLabel *label = new QLabel;
            label->setGeometry(0,0,50,50);
            label->setPixmap(QPixmap(":/res/BoardNode.png"));
            label->setParent(this);
            label->move(60 + i * 50, 200 + j * 50);

            //create coin
            QString str;
            if(this->gameArray[i][j] == 1)
            {
                str = ":/res/Coin0001.png";
            }else {
                str = ":/res/Coin0008.png";
            }

            MyCoin *coin = new MyCoin(str);
            coin->setParent(this);
            coin->move(62 + i * 50, 203 + j * 50);

            coin->posX = i;
            coin->posY = j;
            coin->flag = this->gameArray[i][j];

            coinButton[i][j] = coin;

            //dian ji fan zhuan
            connect(coin,&MyCoin::clicked,[=](){

                coinSound->play();

                for (int i = 0 ; i < 4; i++) {
                    for (int j = 0 ;j < 4; j ++) {
                        this->coinButton[i][i]->isWin = true;
                    }
                }
                coin->changeFlag();
                this->gameArray[i][j] = this->gameArray[i][j]==0?1:0;

                QTimer::singleShot(300,this,[=](){
                    //周围金币的反转
                    if(coin->posX + 1<=3){
                        coinButton[coin->posX+1][coin->posY]->changeFlag();
                        this->gameArray[coin->posX+1][coin->posY] = this->gameArray[coin->posX+1][coin->posY]==0?1:0;
                    }
                    if(coin->posX-1 >= 0){
                        coinButton[coin->posX-1][coin->posY]->changeFlag();
                        this->gameArray[coin->posX-1][coin->posY] = this->gameArray[coin->posX-1][coin->posY]==0?1:0;
                    }
                    if(coin->posY + 1 <= 3){
                        coinButton[coin->posX][coin->posY + 1]->changeFlag();
                        this->gameArray[coin->posX][coin->posY + 1] = this->gameArray[coin->posX][coin->posY + 1]==0?1:0;
                    }
                    if(coin->posY - 1 >= 0){
                        coinButton[coin->posX][coin->posY - 1]->changeFlag();
                        this->gameArray[coin->posX][coin->posY - 1] = this->gameArray[coin->posX][coin->posY - 1]==0?1:0;
                    }
                    for (int i = 0 ; i < 4; i++) {
                        for (int j = 0 ;j < 4; j ++) {
                            this->coinButton[i][i]->isWin = false;
                        }
                    }
                    this->isWin = true;
                    for(int i = 0; i < 4; i++ ){
                        for (int j = 0; j < 4 ; j++) {
                            if(coinButton[i][j]->flag == false){
                                this->isWin = false;
                                break;
                            }
                        }
                    }
                    if(this->isWin == true){
                        for(int i = 0; i < 4; i++ ){
                            for (int j = 0; j < 4 ; j++) {
                                coinButton[i][j]->isWin = true;
                            }
                        }
                        QSound *winSound = new QSound(":/res/LevelWinSound.wav",this);
                        //move to show winLabel
                        QPropertyAnimation *animation = new QPropertyAnimation(winLabel,"geometry");
                        animation->setDuration(1000);

                        animation->setStartValue(QRect(winLabel->x(),winLabel->y(),winLabel->width(),winLabel->height()));
                        animation->setEndValue(QRect(winLabel->x(),winLabel->y()+2*temPix.height(),winLabel->width(),winLabel->height()));
                        animation->setEasingCurve(QEasingCurve::OutBounce);
                        winSound->play();
                        animation->start();

                    }
                });
            });
        }
    }


}

void PlayScene::paintEvent(QPaintEvent *en)
{
    QPainter painter(this);
    QPixmap pix;
    pix.load(":/res/PlayLevelSceneBg.png");
    painter.drawPixmap(0,0,this->width(),this->height(),pix);
    pix.load(":/res/Title.png");
    painter.drawPixmap(10,30,pix.width(),pix.height(),pix);
}
