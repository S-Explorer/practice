#ifndef CHOOSELEVELSCENE_H
#define CHOOSELEVELSCENE_H

#include <QMainWindow>
#include "playscene.h"

class chooseLevelScene : public QMainWindow
{
    Q_OBJECT
public:
    explicit chooseLevelScene(QWidget *parent = nullptr);


    void paintEvent(QPaintEvent *);
    PlayScene *play;

signals:
    //custom signal for back
    void chooseSceneBack();

};

#endif // CHOOSELEVELSCENE_H
