#include "mycoin.h"
#include <QDebug>

//MyCoin::MyCoin(QWidget *parent) : QPushButton(parent)
//{

//}

MyCoin::MyCoin(QString btnImg)
    :btnImg(btnImg)
{
    QPixmap pix;
    bool res = pix.load(btnImg);
    if(!res)
    {
        qDebug()<<"load erro !";
        return ;
    }

    this->setFixedSize(pix.width(),pix.height());
    this->setStyleSheet("QPushButton{border:0px}");
    this->setIcon(pix);
    this->setIconSize(QSize(pix.width(),pix.height()));

    timer1 = new QTimer(this);
    timer2 = new QTimer(this);

    connect(timer1,&QTimer::timeout,[=](){
        QPixmap pix;
        QString str = QString(":/res/Coin000%1.png").arg(this->min++);
        pix.load(str);

        this->setFixedSize(pix.width(),pix.height());
        this->setStyleSheet("QPushButton{border:0px}");
        this->setIcon(pix);
        this->setIconSize(QSize(pix.width(),pix.height()));
        if(this->min > this->max){
            this->min = 1;
            timer1->stop();
            isAnimation = false;
        }
    });

    connect(timer2,&QTimer::timeout,[=](){
        QPixmap pix;
        QString str = QString(":/res/Coin000%1.png").arg(this->max--);
        pix.load(str);

        this->setFixedSize(pix.width(),pix.height());
        this->setStyleSheet("QPushButton{border:0px}");
        this->setIcon(pix);
        this->setIconSize(QSize(pix.width(),pix.height()));
        if(this->min > this->max){
            this->max = 8;
            timer2->stop();
            isAnimation = false;
        }
    });
}

void MyCoin::changeFlag()
{
    //如果是正面，反转到反面
    if(this->flag)
    {
        timer1->start(30);
        this->flag = false;
        isAnimation = true;
    }else{
        timer2->start(30);
        this->flag = true;
        isAnimation = true;
    }

}

void MyCoin::mousePressEvent(QMouseEvent *ev)
{
    if(this->isAnimation || this->isWin )return;
    else {
        QPushButton::mousePressEvent(ev);
    }
}
