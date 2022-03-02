#ifndef PLAYSCENE_H
#define PLAYSCENE_H

#include <QMainWindow>
#include "mycoin.h"

class PlayScene : public QMainWindow
{
    Q_OBJECT
public:
    explicit PlayScene(int levelNum);
    int levelIndex;

    void paintEvent(QPaintEvent *en);

    int gameArray[4][4];
    MyCoin * coinButton[4][4];

    bool isWin = false;

signals:
    void levelSceneBack();

};

#endif // PLAYSCENE_H
