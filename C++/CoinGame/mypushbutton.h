#ifndef MYPUSHBUTTON_H
#define MYPUSHBUTTON_H

#include <QWidget>
#include <QPushButton>

class MyPushButton : public QPushButton
{
    Q_OBJECT
public:

    MyPushButton(QString normalPng,QString pressPng = "");
    MyPushButton(QString normalPng,int add );


    QString normalPng;
    QString pressPng;
    int add = 0;

    //bind
    void zoom();//go

    void mousePressEvent(QMouseEvent *event);

    void mouseReleaseEvent(QMouseEvent *event);

    void enterEvent(QEvent *e);
    void leaveEvent(QEvent *e);

    //explicit MyPushButton(QWidget *parent = nullptr);

signals:

};

#endif // MYPUSHBUTTON_H
