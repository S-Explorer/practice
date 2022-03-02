#include "mypushbutton.h"
#include <QDebug>
#include <QPropertyAnimation>

MyPushButton::MyPushButton(QString normalPng, QString pressPng)
    :normalPng(normalPng),pressPng(pressPng)
{
    QPixmap pix;
    bool res = pix.load(normalPng);
    if(!res){
        qDebug()<< "load normalPng erro";
        return;
    }
    //fixsize
    this->setFixedSize(pix.width(),pix.height());
    //style
    this->setStyleSheet("QPushButton{border:0px}");

    //icon
    this->setIcon(pix);
    //size
    this->setIconSize(QSize(pix.width(),pix.height()));
}

MyPushButton::MyPushButton(QString normalPng, int add)
    :normalPng(normalPng),add(add)
{
    QPixmap pix;
    bool res = pix.load(normalPng);
    if(!res){
        qDebug()<< "load normalPng erro";
        return;
    }
    //fixsize
    this->setFixedSize(pix.width(),pix.height());
    //style
    this->setStyleSheet("QPushButton{border:0px}");

    //icon
    this->setIcon(pix);
    //size
    this->setIconSize(QSize(pix.width(),pix.height()));
    setMouseTracking(true);
}

void MyPushButton::zoom()
{
    QPropertyAnimation *animation = new QPropertyAnimation(this,"geometry");
    animation->setDuration(200);

    //start
    animation->setStartValue(QRect(this->x(),this->y(),this->width(),this->height()));
    //end
    animation->setEndValue(QRect(this->x(),this->y()+10,this->width(),this->height()));
    //start
    animation->setStartValue(QRect(this->x(),this->y()+10,this->width(),this->height()));
    //end
    animation->setEndValue(QRect(this->x(),this->y(),this->width(),this->height()));
    //bind curve
    animation->setEasingCurve(QEasingCurve::OutBounce);
    //start animation
    animation->start();
}

void MyPushButton::mousePressEvent(QMouseEvent *event)
{
    if(this->pressPng != "")
    {
        QPixmap pix;
        bool res = pix.load(this->pressPng);
        if(!res){
            qDebug()<< "load normalPng erro";
            return;
        }
        //fixsize
        this->setFixedSize(pix.width(),pix.height());
        //style
        this->setStyleSheet("QPushButton{border:0px}");

        //icon
        this->setIcon(pix);
        //size
        this->setIconSize(QSize(pix.width(),pix.height()));
    }
    return QPushButton::mousePressEvent(event);
}

void MyPushButton::mouseReleaseEvent(QMouseEvent *event)
{
    if(this->pressPng != "")
    {
        QPixmap pix;
        bool res = pix.load(this->normalPng);
        if(!res){
            qDebug()<< "load normalPng erro";
            return;
        }
        //fixsize
        this->setFixedSize(pix.width(),pix.height());
        //style
        this->setStyleSheet("QPushButton{border:0px}");

        //icon
        this->setIcon(pix);
        //size
        this->setIconSize(QSize(pix.width(),pix.height()));
    }
    return QPushButton::mouseReleaseEvent(event);
}

void MyPushButton::enterEvent(QEvent *e)
{
    if(add != 0)
    {
        QPixmap pix,befor;
        befor.load(normalPng);
        bool res = pix.load(":/res/Coin0001.png");
        if(!res){
            qDebug()<<"load menuButton png erro";
            return;
        }
        qDebug()<<"get in ";

        //fixsize
        this->setFixedSize(befor.width(),befor.height());
        //style
        this->setStyleSheet("QPushButton{border:0px}");

        //icon
        this->setIcon(pix);
        this->setIconSize(QSize(befor.width(),befor.height()));
    }
    QPushButton::enterEvent(e);
}

void MyPushButton::leaveEvent(QEvent *e)
{
    if(add != 0)
    {
        QPixmap pix;
        bool res = pix.load(this->normalPng);
        if(!res){
            qDebug()<<"load menuButton png erro";
            return;
        }
        qDebug()<<"get in ";

        //fixsize
        this->setFixedSize(pix.width(),pix.height());
        //style
        this->setStyleSheet("QPushButton{border:0px}");
        //icon
        this->setIcon(pix);
        this->setIconSize(QSize(pix.width(),pix.height()));
    }
    QPushButton::leaveEvent(e);
}

