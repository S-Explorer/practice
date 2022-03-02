#include "chooselevelscene.h"
#include <QMenuBar>
#include <QPainter>
#include <QDebug>
#include "mypushbutton.h"
#include <QLabel>
#include <QSound>

chooseLevelScene::chooseLevelScene(QWidget *parent) : QMainWindow(parent)
{
    this->setFixedSize(320,588);
    this->setWindowIcon(QPixmap(":/res/Coin0001.png"));
    this->setWindowTitle("选关");

    QMenuBar * bar = menuBar();
    setMenuBar(bar);

    QMenu * startBar = bar->addMenu("选择");
    QAction *exitAction = startBar->addAction("退出");

    connect(exitAction,&QAction::triggered,[=](){
            this->close();
    });

    QSound *chooseSound = new QSound(":/res/BackButtonSound.wav",this);

    //return button
    MyPushButton * backButton = new MyPushButton(":/res/BackButton.png",":/res/BackButtonSelected.png");
    backButton->setParent(this);
    backButton->move(this->width() - backButton->width(),this->height() - backButton->height());

    connect(backButton,&QPushButton::clicked,[=](){
        qDebug()<<"back";
        chooseSound->play();
        emit this->chooseSceneBack();
    });

    QSound *selectSound = new QSound(":/res/TapButtonSound.wav",this);

    for (int buttonNum = 0; buttonNum < 20 ; buttonNum++) {
        MyPushButton * menubtn = new MyPushButton(":/res/LevelIcon.png",1);
        menubtn->setParent(this);
        menubtn->move(25 + buttonNum % 4 * 70,130 + buttonNum / 4 * 70);

        connect(menubtn,&QPushButton::clicked,[=](){
            qDebug()<<"this is clicked !";
            QTimer::singleShot(100,this,[=](){
                selectSound->play();
                this->hide();
                play = new PlayScene(buttonNum+1);

                play->setGeometry(this->geometry());

                play->show();

                connect(play,&PlayScene::levelSceneBack,[=](){
                    this->setGeometry(play->geometry());
                    this->show();
                    play->hide();
//                    delete play;
                    play = NULL;
                });
            });
        });

        QLabel * menuLabel = new QLabel;
        menuLabel->setParent(menubtn);
        menuLabel->setFixedSize(menubtn->width(),menubtn->height());
        menuLabel->setAlignment(Qt::AlignCenter);
        menuLabel->setText(QString::number(buttonNum+1));
    }

}

void chooseLevelScene::paintEvent(QPaintEvent *)
{
    QPainter painter(this);
    QPixmap pix;
    pix.load(":/res/OtherSceneBg.png");
    painter.drawPixmap(0,0,this->width(),this->height(),pix);
    pix.load(":/res/Title.png");
    painter.drawPixmap((this->width() - pix.width())*0.5,30,pix.width(),pix.height(),pix);
}
