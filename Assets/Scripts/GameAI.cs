using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ArchiveAI
{
    public Vector3Int up, down;    //звідки і куди хід
    public Figura figUp;        //фігура для підйому
    public Figura figDown;      //фігура що заміщується
    public int cost;            //сумарна вартість ходу
}
public class GameAI : MonoBehaviour
{
    private int[,,] analiz = new int[3, 12, 8];
    private SaveFigure figureAnaliz = new SaveFigure();
    private HidFigur hidAnaliz = new HidFigur();
    private bool flag = true;

    private const int costMove = 1;         //клітинки куди можна піти
    private const int costDefend = 2;       //клітинка під захистом (множник вартості фігури)
    private const int costAttack = 4;        //клітинка під атакою (множник вартості фігури)
    private const int costControl = 2;      //клітинка під контролем
    private const int costTransform = 4;    //остання клітинка для пішака
    private const int costBattle = 10;      //множник вартості фігури, яку побито
    private const int standart = 256;       //кількість збережених варіантів
    
    private ArchiveAI[] hidAI = new ArchiveAI[standart];
    private int[,,] ahf = new int[3, 12, 8];//аналізна мапа
    private int iteration = 0;

    public GameAI(Figura[,,] map, int nowflag)
    {
        analiz = new int[3, 12, 8];
        ahf = new int[3, 12, 8];
        AnalizedPosition(map, nowflag,true);
    }
    private void AnalizedPosition(Figura[,,] map,int nowFlag,bool AHF)
    {
        analiz = new int[3, 12, 8];
        ColorFigur colorFlag;
        int myFigura = 1;
        if(nowFlag ==1)
        {
            colorFlag = ColorFigur.White;
        }
        else if (nowFlag==-1)
        {
            colorFlag = ColorFigur.Black;
        }
        else
        {
            colorFlag = ColorFigur.Null;
            Debug.Log("-- Error AI: 1 --");
        }
        for(int z=0;z<3;z++)
        {
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    flag = false;
                    if (map[z, x, y].GetColor() != ColorFigur.Null)
                    {
                        if (z==1 && map[0, x, y].GetName()=="B")
                        {

                        }
                        else
                        {
                            figureAnaliz.up.x = x;
                            figureAnaliz.up.y = y;
                            figureAnaliz.up.z = z;
                            figureAnaliz.figura = map[z, x, y];
                            switch (z)
                            {
                                case 2: hidAnaliz = figureAnaliz.figura.HidSky(); break;
                                case 1: hidAnaliz = figureAnaliz.figura.HidGround(); break;
                                case 0: hidAnaliz = figureAnaliz.figura.HidUnderground(); break;
                            }
                            if (colorFlag == map[z, x, y].GetColor())
                            {
                                myFigura = 1;
                            }
                            else
                            {
                                myFigura = -1;
                            }
                            flag = true;
                        }
                    }
                    if(flag==true)
                    {
                        AnalizM(map, AHF, myFigura);
                        AnalizC(map, AHF, myFigura);
                        AnalizX(map, AHF, myFigura);
                        if(AHF==true && myFigura==1)
                        {
                            AnalizedHidFigur(map, nowFlag);
                            ahf = new int[3, 12, 8];
                        }
                    }
                }
            }
        }
    }
    //Аналіз ходів
    private void AnalizM(Figura[,,] map, bool AHF, int myFigura)
    {
        for (int i = 0; i < hidAnaliz.StaticM.GetLength(0); i++)
        {
            if (hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x >= 0 &&
                hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x < 12 &&
                hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y >= 0 &&
                hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y < 8 && myFigura == 1)
            {
                switch (hidAnaliz.StaticM[i, 2])
                {
                    case 2:
                        if (map[2, hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y].GetColor() == ColorFigur.Null)
                        {
                            analiz[2, hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y] += costMove;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[2, hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y] = 1;
                            }
                        }
                        break;

                    case 1:
                        if (map[0,figureAnaliz.up.x, figureAnaliz.up.y].GetName() != "B")
                        {
                            if (map[1,figureAnaliz.up.x, figureAnaliz.up.y].GetName() == "S")
                            {
                                switch (map[1, figureAnaliz.up.x, figureAnaliz.up.y].GetColor())
                                {
                                    case ColorFigur.White:
                                        if (map[2,0, 6].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,0, 6] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,0, 6] = 1;
                                            }
                                        }
                                        if (map[2,2, 6].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,2, 6] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,2, 6] = 1;
                                            }
                                        }
                                        if (map[2,4, 6].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,4, 6] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,4, 6] = 1;
                                            }
                                        }
                                        if (map[2,6, 6].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,6, 6] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,6, 6] = 1;
                                            }
                                        }
                                        if (map[2,8, 6].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,8, 6] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,8, 6] = 1;
                                            }
                                        }
                                        if (map[2,10, 6].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,10, 6] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,10, 6] = 1;
                                            }
                                        }
                                        break;
                                    case ColorFigur.Black:
                                        if (map[2,0, 1].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,0, 1] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,0, 1] = 1;
                                            }
                                        }
                                        if (map[2,2, 1].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,2, 1] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,2, 1] = 1;
                                            }
                                        }
                                        if (map[2,4, 1].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,4, 1] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,4, 1] = 1;
                                            }
                                        }
                                        if (map[2,6, 1].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,6, 1] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,6, 1] = 1;
                                            }
                                        }
                                        if (map[2,8, 1].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,8, 1] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,8, 1] = 1;
                                            }
                                        }
                                        if (map[2,10, 1].GetColor() == ColorFigur.Null)
                                        {
                                            analiz[2,10, 1] += costMove;
                                            if (AHF == true)
                                            {
                                                ahf[2,10, 1] = 1;
                                            }
                                        }
                                        break;
                                }
                            }
                            else if (map[1,figureAnaliz.up.x, figureAnaliz.up.y].GetName() == "W")
                            {
                                switch (map[1,figureAnaliz.up.x, figureAnaliz.up.y].GetColor())
                                {
                                    case ColorFigur.White:
                                        if (map[1,figureAnaliz.up.x, 7].GetColor() == ColorFigur.Null && figureAnaliz.up.y == 6)
                                        {
                                            analiz[1,figureAnaliz.up.x, 7] += costTransform;
                                            if (AHF == true && myFigura == 1)
                                            {
                                                ahf[1,figureAnaliz.up.x, 7] = costTransform;
                                            }
                                        }
                                        break;
                                    case ColorFigur.Black:
                                        if (map[1,figureAnaliz.up.x, 0].GetColor() == ColorFigur.Null && figureAnaliz.up.y == 1)
                                        {
                                            analiz[1,figureAnaliz.up.x, 0] += costTransform;
                                            if (AHF == true && myFigura == 1)
                                            {
                                                ahf[1,figureAnaliz.up.x, 0] = costTransform;
                                            }
                                        }
                                        break;
                                }
                            }

                            if (map[1,hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y].GetColor() == ColorFigur.Null)
                            {
                                analiz[1,hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y] += costMove;
                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[1,hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y] = 1;
                                }
                            }
                        }
                        break;
                    case 0:
                        if (map[0, hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y].GetColor() == ColorFigur.Null)
                        {
                            analiz[0,hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y] += costMove;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[0,hidAnaliz.StaticM[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticM[i, 1] + figureAnaliz.up.y] = 1;
                            }
                        }
                        break;

                }
            }
        }
        
    }
    private void AnalizC(Figura[,,] map, bool AHF, int myFigura)
    {
        for (int i = 0; i < hidAnaliz.StaticC.GetLength(0); i++)
        {
            if (hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x >= 0 &&
                hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x < 12 &&
                hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y >= 0 &&
                hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y < 8)
            {
                switch (hidAnaliz.StaticC[i, 2])
                {
                    case 2:
                        if (map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != figureAnaliz.figura.GetColor() &&
                            map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] +=
                               (int)map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costAttack * myFigura;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] =
                                    (int)map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costBattle;
                            }
                        }
                        else if (map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() == figureAnaliz.figura.GetColor() &&
                            map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] +=
                               (int)map[2,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costDefend * myFigura;
                        }
                        break;
                    case 1:
                        if (map[0,figureAnaliz.up.x, figureAnaliz.up.y].GetName() != "B")
                        {
                            if (map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != figureAnaliz.figura.GetColor() &&
                                map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] +=
                                   (int)map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costAttack * myFigura;
                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] =
                                        (int)map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costBattle;
                                }
                            }
                            else if (map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() == figureAnaliz.figura.GetColor() &&
                                map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] +=
                                   (int)map[1,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costDefend * myFigura;
                            }
                        }
                        break;
                    case 0:
                        if (map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != figureAnaliz.figura.GetColor() &&
                            map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] +=
                               (int)map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costAttack * myFigura;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] =
                                    (int)map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costBattle;
                            }

                        }
                        else if (map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() == figureAnaliz.figura.GetColor() &&
                            map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y] +=
                               (int)map[0,hidAnaliz.StaticC[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticC[i, 1] + figureAnaliz.up.y].GetCost() * costDefend * myFigura;
                        }
                        break;
                }
            }

        }
    }
    private void AnalizX(Figura[,,] map, bool AHF, int myFigura)
    {
        for (int i = 0; i < hidAnaliz.StaticX.GetLength(0); i++)
        {
            if (hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x >= 0 &&
                hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x < 12 &&
                hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y >= 0 &&
                hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y < 8)
            {
                switch (hidAnaliz.StaticX[i, 2])
                {
                    case 2:
                        if (map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() == ColorFigur.Null && myFigura == 1)
                        {
                            analiz[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] += costControl;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] = 1;
                            }
                        }
                        else if (map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != figureAnaliz.figura.GetColor() &&
                            map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] +=
                               (int)map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costAttack * myFigura;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] =
                                    (int)map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costBattle;
                            }
                        }
                        else if (map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() == figureAnaliz.figura.GetColor() &&
                            map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] +=
                               (int)map[2, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costDefend * myFigura;
                        }
                        break;
                    case 1:
                        if (map[0, figureAnaliz.up.x, figureAnaliz.up.y].GetName() != "B")
                        {
                            if (map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() == ColorFigur.Null && myFigura == 1)
                            {
                                analiz[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] += costControl;
                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] = 1;
                                }
                            }
                            else if (map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != figureAnaliz.figura.GetColor() &&
                                map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] +=
                                   (int)map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costAttack * myFigura;
                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] =
                                        (int)map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costBattle;
                                }
                            }
                            else if (map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() == figureAnaliz.figura.GetColor() &&
                                map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] +=
                                   (int)map[1, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costDefend * myFigura;
                            }
                        }
                        break;
                    case 0:
                        if (map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() == ColorFigur.Null && myFigura == 1)
                        {
                            analiz[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] += costControl;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] = 1;
                            }
                        }
                        else if (map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != figureAnaliz.figura.GetColor() &&
                            map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] +=
                               (int)map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costAttack * myFigura;
                            if (AHF == true && myFigura == 1)
                            {
                                ahf[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] =
                                    (int)map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costBattle;
                            }
                        }
                        else if (map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() == figureAnaliz.figura.GetColor() &&
                            map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetColor() != ColorFigur.Null)
                        {
                            analiz[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y] +=
                               (int)map[0, hidAnaliz.StaticX[i, 0] + figureAnaliz.up.x, hidAnaliz.StaticX[i, 1] + figureAnaliz.up.y].GetCost() * costDefend * myFigura;
                        }
                        break;
                }
            }


        }

        Vector3Int dinamicLine = new Vector3Int(0, 0, 0);
        bool stop;
        for (int i = 0; i < hidAnaliz.DinamicX.GetLength(0); i++)
        {
            switch (hidAnaliz.DinamicX[i])
            {
                case 1:
                    dinamicLine.x = 0;
                    dinamicLine.y = -1;
                    break;
                case 2:
                    dinamicLine.x = 1;
                    dinamicLine.y = -1;
                    break;
                case 3:
                    dinamicLine.x = 1;
                    dinamicLine.y = 0;
                    break;
                case 4:
                    dinamicLine.x = 1;
                    dinamicLine.y = 1;
                    break;
                case 5:
                    dinamicLine.x = 0;
                    dinamicLine.y = 1;
                    break;
                case 6:
                    dinamicLine.x = -1;
                    dinamicLine.y = 1;
                    break;
                case 7:
                    dinamicLine.x = -1;
                    dinamicLine.y = 0;
                    break;
                case 8:
                    dinamicLine.x = -1;
                    dinamicLine.y = -1;
                    break;
                default:
                    dinamicLine.x = 0;
                    dinamicLine.y = 0;
                    break;
            }
            stop = false;
            for (int j = 1; j < 12; j++)
            {
                if (figureAnaliz.up.x + j * dinamicLine.x >= 0
                        && figureAnaliz.up.x + j * dinamicLine.x < 12
                        && figureAnaliz.up.y + j * dinamicLine.y >= 0
                        && figureAnaliz.up.y + j * dinamicLine.y < 8
                        && (dinamicLine.x != 0 || dinamicLine.y != 0))
                {
                    switch (figureAnaliz.up.z)
                    {
                        case 2:
                            if (map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() == ColorFigur.Null)
                            {
                                analiz[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] += costControl;
                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] = 1;
                                }
                            }
                            else if (map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != figureAnaliz.figura.GetColor() &&
                                map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] +=
                                   (int)map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costAttack * myFigura;
                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] =
                                        (int)map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costBattle;
                                }
                            }
                            else if (map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() == figureAnaliz.figura.GetColor() &&
                                map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] +=
                                   (int)map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costDefend * myFigura;
                            }
                            if (map[2,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                            {
                                stop = true;
                            }
                            break;
                        case 1:
                            if (map[0,figureAnaliz.up.x, figureAnaliz.up.y].GetName() != "B")
                            {
                                if (map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() == ColorFigur.Null)
                                {
                                    analiz[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] += costControl;

                                    if (AHF == true && myFigura == 1)
                                    {
                                        ahf[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] = 1;
                                    }
                                }
                                else if (map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != figureAnaliz.figura.GetColor() &&
                                    map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                                {
                                    analiz[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] +=
                                       (int)map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costAttack * myFigura;

                                    if (AHF == true && myFigura == 1)
                                    {
                                        ahf[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] =
                                            (int)map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costBattle;
                                    }
                                }
                                else if (map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() == figureAnaliz.figura.GetColor() &&
                                    map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                                {
                                    analiz[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] +=
                                       (int)map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costDefend * myFigura;
                                }
                                if (map[1,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                                {
                                    stop = true;
                                }
                            }
                            break;
                        case 0:
                            if (map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() == ColorFigur.Null)
                            {
                                analiz[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] += costControl;

                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] = 1;
                                }
                            }
                            else if (map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != figureAnaliz.figura.GetColor() &&
                                map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] +=
                                   (int)map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costAttack * myFigura;

                                if (AHF == true && myFigura == 1)
                                {
                                    ahf[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] =
                                        (int)map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costBattle;
                                }
                            }
                            else if (map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() == figureAnaliz.figura.GetColor() &&
                                map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                            {
                                analiz[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y] +=
                                   (int)map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetCost() * costDefend * myFigura;
                            }
                            if (map[0,figureAnaliz.up.x + j * dinamicLine.x, figureAnaliz.up.y + j * dinamicLine.y].GetColor() != ColorFigur.Null)
                            {
                                stop = true;
                            }
                            break;
                    }
                    if(stop==true)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

        }
    }

    private void AnalizedHidFigur(Figura[,,] map, int nowFlag)
    {
        hidAI[0].up = figureAnaliz.up;
        hidAI[0].figUp = figureAnaliz.figura;
        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (ahf[z, x, y] > 0)
                    {
                        hidAI[0].down.x = x;
                        hidAI[0].down.y = y;
                        hidAI[0].down.z = z;

                        map[hidAI[0].up.z, hidAI[0].up.x, hidAI[0].up.y] = new Figura();
                        
                        switch (hidAI[0].down.z)
                        {
                            case 2:
                                hidAI[0].figDown = map[2,hidAI[0].down.x, hidAI[0].down.y];
                                map[2,hidAI[0].down.x, hidAI[0].down.y] = hidAI[0].figUp;
                                break;
                            case 1:
                                hidAI[0].figDown = map[1,hidAI[0].down.x, hidAI[0].down.y];

                                if (hidAI[0].figUp.GetName() == "Dr")
                                {
                                    map[2,hidAI[0].up.x, hidAI[0].up.y] = hidAI[0].figUp;
                                    map[1,hidAI[0].down.x, hidAI[0].down.y] = new Figura();
                                }
                                else if (hidAI[0].figUp.GetName() == "W" && hidAI[0].figUp.GetColor() == ColorFigur.White && hidAI[0].down.y == 7)
                                {
                                    map[1,hidAI[0].down.x, hidAI[0].down.y] = new WhiteHero();
                                }
                                else if (hidAI[0].figUp.GetName() == "W" && hidAI[0].figUp.GetColor() == ColorFigur.Black && hidAI[0].down.y == 0)
                                {
                                    map[1,hidAI[0].down.x, hidAI[0].down.y] = new BlackHero();
                                }
                                else
                                {
                                    map[1,hidAI[0].down.x, hidAI[0].down.y] = hidAI[0].figUp;
                                }
                                break;
                            case 0:
                                hidAI[0].figDown = map[0,hidAI[0].down.x, hidAI[0].down.y];
                                map[0,hidAI[0].down.x, hidAI[0].down.y] = hidAI[0].figUp;
                                break;
                        }


                        AnalizedPosition(map, nowFlag, false);
                        hidAI[0].cost = SumPosition() + ahf[z, x, y];

                        map[hidAI[0].down.z, hidAI[0].down.x, hidAI[0].down.y] = hidAI[0].figDown;
                        map[hidAI[0].up.z,hidAI[0].up.x, hidAI[0].up.y] = hidAI[0].figUp;
                        
                        if (iteration > 0)
                        {
                            if (hidAI[0].cost > hidAI[1].cost)
                            {
                                iteration = 1;
                                hidAI[1] = hidAI[0];
                            }
                            else if (hidAI[0].cost == hidAI[1].cost)
                            {
                                iteration++;
                                hidAI[iteration] = hidAI[0];
                            }
                        }
                        else
                        {
                            iteration = 1;
                            hidAI[1] = hidAI[0];
                        }
                    }
                }
            }
        }
    }
    private int SumPosition()
    {
        int sum = 0;
        for (int x = 0; x < 12; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    sum += analiz[z, x, y];
                }
            }
        }
        return sum;
    }


    public SaveFigure RunAI()
    {
        SaveFigure resultAI = new SaveFigure();
        if(iteration==1)
        {
            resultAI.up = hidAI[1].up;
            resultAI.down = hidAI[1].down;
            resultAI.figura = hidAI[1].figUp;
        }
        else if(iteration>1)
        {
            int r = Random.Range(1, iteration + 1);
            resultAI.up = hidAI[r].up;
            resultAI.down = hidAI[r].down;
            resultAI.figura = hidAI[r].figUp;
        }
        else
        {
            Debug.Log("--Не бачу можливих ходів--");
        }
        return resultAI;
    }

}
