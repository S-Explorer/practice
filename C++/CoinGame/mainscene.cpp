#include "mainscene.h"
#include "ui_mainscene.h"
#include <QPainter>
#include <QDebug>
#include "mypushbutton.h"
#include <QTimer>
#include <QSound>

MainScene::MainScene(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainScene)
{
    ui->setupUi(this);

    connect(ui->exitBar,&QAction::triggered,[=](){
        this->close();
    });

    QSound *startSound = new QSound(":/res/TapButtonSound.wav",this);


    //start button
    MyPushButton * startButton  = new MyPushButton(":/res/MenuSceneStartButton.png");
    startButton->setParent(this);
    startButton->move(this->width()*0.5-startButton->width()*0.5,this->height()*0.7);

    chooselevelscene = new chooseLevelScene;
    connect(chooselevelscene,&chooseLevelScene::chooseSceneBack,[=](){
        this->setGeometry(chooselevelscene->geometry());
        QTimer::singleShot(250,this,[=](){
        this->show();
        chooselevelscene->hide();});
    });

    connect(startButton,&QPushButton::clicked,[=](){
        qDebug()<<"zoom";
        startButton->zoom();
        startSound->play();

    //delay to the select menu
        QTimer::singleShot(250,this,[=](){
            chooselevelscene->setGeometry(this->geometry());
            chooselevelscene->show();
            this->hide();
        });
    });
}

MainScene::~MainScene()
{
    delete ui;
}

void MainScene::paintEvent(QPaintEvent *)
{
    QPainter painter(this);

    QPixmap pix;
    pix.load(":/res/PlayLevelSceneBg.png");

    painter.drawPixmap(0,0,this->width(),this->height(),pix);

    //background pixmap
    pix.load(":/res/Title.png");
    pix = pix.scaled(pix.width()*0.5,pix.height()*0.5);
    painter.drawPixmap(10,25,pix);
}

