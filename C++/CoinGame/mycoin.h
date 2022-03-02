#ifndef MYCOIN_H
#define MYCOIN_H

#include <QWidget>
#include <QPushButton>
#include <QTimer>

class MyCoin : public QPushButton
{
    Q_OBJECT
public:
//    explicit MyCoin(QWidget *parent = nullptr);
    MyCoin(QString btnImg);

    QString btnImg;

    bool flag;//正反面
    int posX;
    int posY;//坐标位置

    void changeFlag();
    QTimer (*timer1),(*timer2);
    int min = 1,max = 8;
    bool isAnimation = false;

    bool isWin = false;

    void mousePressEvent(QMouseEvent *ev);

signals:

};

#endif // MYCOIN_H
