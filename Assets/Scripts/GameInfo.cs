using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    //текстура дошок
    static public int[,] texture = new int[12, 8];
    //ігрові поля
    static public Figura[,,] map = new Figura[3, 12, 8];
    static public int[,,] mapID = new int[3, 12, 8];
    //карта можливих ходів
    static public ColorActiveNode[,,] moveMap = new ColorActiveNode[3, 12, 8];
    //загальні параметри
    static public SaveFigure save = new SaveFigure();
    static public int flag = 1;//чий нині хід (1=білих, -1=чорних)
    //загальні функції
    static public bool activeUI = false;
}
public enum ColorFigur
{
    Null=0,
    White=1,
    Black=-1
}
public enum ColorActiveNode
{
    Null=0,
    NodeM=1,
    NodeX=2,
    NodeC=3,
    NodeB=4,
    NodeActive=5
}
public class Figura
{
    protected ColorFigur color;
    protected string name;
    protected float cost;
    protected string model;
    //ходи фігури на кожній з дошок
    protected HidFigur moveSky = new HidFigur();
    protected HidFigur moveGround = new HidFigur();
    protected HidFigur moveUnderground = new HidFigur();
    public Figura()
    {
        color = ColorFigur.Null;
        name = "";
        cost = 0;
        model = "";
    }
    public ColorFigur GetColor()
    {
        return color;
    }
    public string GetName()
    {
        return name;
    }
    public float GetCost()
    {
        return cost;
    }
    public string GetModel()
    {
        return model;
    }
    public HidFigur HidSky()
    {
        return moveSky;
    }
    public HidFigur HidGround()
    {
        return moveGround;
    }
    public HidFigur HidUnderground()
    {
        return moveUnderground;
    }

}
public class HidFigur
{
    public int[,] StaticM = new int[0, 0];
    public int[,] StaticX = new int[0, 0];
    public int[,] StaticC = new int[0, 0];
    public int[] DinamicX = new int[0];

    /* нумерація для Dinamic:
            (0 - розташування фігури)

            [8 1 2]
            [7 0 3]
            [6 5 4]
    */
}
public class SaveFigure//експерементальний
{
    public Vector3Int up, down;
    public Figura figura = new Figura();
}

public class WhiteAngel : Figura    //Янгол
{
    public WhiteAngel()
    {
        cost = 1;
        name = "S";
        color = ColorFigur.White;
        model = "WhiteAngel";

        moveGround = new HidFigur
        {
            StaticM = new int[,]
            {
                {0,0,2 }
            }
        };
        moveSky = new HidFigur
        {
            StaticC = new int[,]
            {
                { 0, 1, 2 },
                { 0, 0, 1 }
            },
            StaticM = new int[,]
            {
                {-1, 1, 2 },
                { 1, 1, 2 }
            }
        };
    }
}
public class WhiteBasilisk : Figura //Василіск
{
    public WhiteBasilisk()
    {
        cost = 2;
        name = "B";
        color = ColorFigur.White;
        model = "WhiteBasilisk";

        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1, 1,0 },
                        { 0, 1,0 },
                        { 1, 1,0 }
                    },
            StaticM = new int[,]
                    {
                        { 0,-1,0 }
                    }
        };
    }
}
public class WhiteBowman : Figura   //Лучник
{
    public WhiteBowman()
    {
        cost = 4;
        name = "T";
        color = ColorFigur.White;
        model = "WhiteBowman";

        moveGround = new HidFigur
        {
            DinamicX = new int[]
                    {2,4,6,8}
        };
    }
}
public class WhiteCavalry : Figura  //Кавалерист
{
    public WhiteCavalry()
    {
        cost = 10;
        name = "P";
        color = ColorFigur.White;
        model = "WhiteCavalry";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,2 },
                        {-1, 0,2 },
                        {-1, 1,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 1,-1,2 },
                        { 1, 0,2 },
                        { 1, 1,2 },

                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },

                        {-2, 0,1 },
                        { 2, 0,1 },
                        { 0,-2,1 },
                        { 0, 2,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },

                        {-2,-1,1 },
                        {-2, 1,1 },
                        {-1,-2,1 },
                        {-1, 2,1 },
                        { 1,-2,1 },
                        { 1, 2,1 },
                        { 2,-1,1 },
                        { 2, 1,1 },

                        {-2, 0,0 },
                        { 2, 0,0 },
                        { 0,-2,0 },
                        { 0, 2,0 },

                        {-2, 0,2 },
                        { 2, 0,2 },
                        { 0,-2,2 },
                        { 0, 2,2 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,0 },
                        {-1, 0,0 },
                        {-1, 1,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 1,-1,0 },
                        { 1, 0,0 },
                        { 1, 1,0 },

                        {-1, 0,2 },
                        { 1, 0,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },

                        {-2, 0,1 },
                        { 2, 0,1 },
                        { 0,-2,1 },
                        { 0, 2,1 }
            }
        };
    }
}
public class WhiteDragon : Figura   //Дракон
{
    public WhiteDragon()
    {
        cost = 8;
        name = "Dr";
        color = ColorFigur.White;
        model = "WhiteDragon";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
            {
                { -1, 0, 2 },
                { 1, 0, 2 },
                { 0, -1, 2 },
                { 0, 1, 2 }
            },
            StaticC = new int[,]
            {
                {0,0,1 },
                { -1, 0, 1 },
                { 1, 0, 1 },
                { 0, -1, 1 },
                { 0, 1, 1}
            },
            DinamicX = new int[]
            {2,4,6,8}
        };
    }
}
public class WhiteElemental : Figura    //Елементаль
{
    public WhiteElemental()
    {
        cost = 4;
        name = "E";
        color = ColorFigur.White;
        model = "WhiteElemental";

        moveUnderground = new HidFigur
        {
            StaticM = new int[,]
                    {
                        {-1,-1,0 },
                        { 1,-1,0 },
                        {-1, 1,0 },
                        { 1, 1,0 }
                    },
            StaticX = new int[,]
                    {
                        {-2, 0,0 },
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 2, 0,0 },
                        { 0,-2,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 0, 2,0 },
                    },
            StaticC = new int[,]
                    {
                        {-1, 0,1 },
                        { 1, 0,1 },
                        { 0,-1,1 },
                        { 0, 1,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 },
                        { 0, 1,0 }
            }
        };
    }
}
public class WhiteGhost : Figura    //Привид
{
    public WhiteGhost()
    {
        cost = 9;
        name = "C";
        color = ColorFigur.White;
        model = "WhiteGhost";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,2 },
                        {-1, 0,2 },
                        {-1, 1,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 1,-1,2 },
                        { 1, 0,2 },
                        { 1, 1,2 },

                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },
                        { 0, 0,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },

                        {-1,-1,2 },
                        {-1, 0,2 },
                        {-1, 1,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 1,-1,2 },
                        { 1, 0,2 },
                        { 1, 1,2 },
                        { 0, 0,2 },

                        {-1,-1,0 },
                        {-1, 0,0 },
                        {-1, 1,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 1,-1,0 },
                        { 1, 0,0 },
                        { 1, 1,0 },
                        { 0, 0,0 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,0 },
                        {-1, 0,0 },
                        {-1, 1,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 1,-1,0 },
                        { 1, 0,0 },
                        { 1, 1,0 },

                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },
                        { 0, 0,1 }
            }
        };
    }
}
public class WhiteGiant : Figura    //Велетень
{
    public WhiteGiant()
    {
        cost = 5;
        name = "O";
        color = ColorFigur.White;
        model = "WhiteGiant";

        moveGround = new HidFigur
        {
            DinamicX = new int[]
                    {1,3,5,7}
        };
    }
}
public class WhiteGriffin : Figura  //Грифон
{
    public WhiteGriffin()
    {
        cost = 5;
        name = "G";
        color = ColorFigur.White;
        model = "WhiteGriffin";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        { -3, -2, 2 },
                        { -3, 2, 2 },
                        { -2, -3, 2 },
                        { -2, 3, 2 },
                        { 2, -3, 2 },
                        { 2, 3, 2 },
                        { 3, -2, 2 },
                        { 3, 2, 2 },
                        { -1, -1, 1 },
                        { -1, 1, 1 },
                        { 1, -1, 1 },
                        { 1, 1, 1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        { -1, -1, 1 },
                        { -1, 1, 1 },
                        { 1, -1, 1 },
                        { 1, 1, 1 },
                        { -1, -1, 2 },
                        { -1, 1, 2 },
                        { 1, -1, 2 },
                        { 1, 1, 2 }
            }
        };
    }
}
public class WhiteHero : Figura //Герой
{
    public WhiteHero()
    {
        cost = 4.5f;
        name = "H";
        color = ColorFigur.White;
        model = "WhiteHero";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,1 },
                        {-1, 1,1 },
                        { 1,-1,1 },
                        { 1, 1,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,0 },
                        {-1, 1,0 },
                        { 1,-1,0 },
                        { 1, 1,0 },
                        {-1,-1,2 },
                        {-1, 1,2 },
                        { 1,-1,2 },
                        { 1, 1,2 },
                        {-1,-1,1 },
                        {-1, 1,1 },
                        { 1,-1,1 },
                        { 1, 1,1 },
                        {-2,-2,1 },
                        {-2, 2,1 },
                        { 2,-2,1 },
                        { 2, 2,1 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,1 },
                        {-1, 1,1 },
                        { 1,-1,1 },
                        { 1, 1,1 }
            }
        };
    }
}
public class WhiteSpearman : Figura //Піхотинець
{
    public WhiteSpearman()
    {
        cost = 1;
        name = "W";
        color = ColorFigur.White;
        model = "WhiteSpearman";

        moveGround = new HidFigur
        {
            StaticM = new int[,]
                    {
                        { 0, 1, 1 }
                    },
            StaticC = new int[,]
                    {
                        { 1, 1, 1 },
                        { -1, 1, 1 }
                    }
        };
    }
}
public class WhiteSpider : Figura   //Павук
{
    public WhiteSpider()
    {
        cost = 2;
        name = "D";
        color = ColorFigur.White;
        model = "WhiteSpider";

        moveUnderground = new HidFigur
        {
            StaticM = new int[,]
                    {
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0, 1,0 }
                    },
            StaticC = new int[,]
                    {
                        {-1, 1,0 },
                        { 1, 1,0 },
                        { 0, 0,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticM = new int[,]
            {
                        { 0, 0,0 }
            }
        };
    }
}
public class WhiteStandardBearer : Figura   //Король
{
    public WhiteStandardBearer()
    {
        cost = 100;
        name = "K";
        color = ColorFigur.White;
        model = "WhiteStandardBearer";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {0,0,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        { 0, 0,0 },
                        { 0, 0,2 },
                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {0,0,1 }
            }
        };
    }
}
public class WhiteUnicorn : Figura  //Єдиноріг
{
    public WhiteUnicorn()
    {
        cost = 2.5f;
        name = "U";
        color = ColorFigur.White;
        model = "WhiteUnicorn";

        moveGround = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-2,-1, 1 },
                        {-2, 1, 1 },
                        {-1,-2, 1 },
                        {-1, 2, 1 },
                        { 1,-2, 1 },
                        { 1, 2, 1 },
                        { 2,-1, 1 },
                        { 2, 1, 1 }
                    }
        };
    }
}
public class WhiteWizard : Figura   //Чарівник
{
    public WhiteWizard()
    {
        cost = 11;
        name = "M";
        color = ColorFigur.White;
        model = "WhiteWizard";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1, 0,2 },
                        { 1, 0,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 0, 0,1 },
                        { 0, 0,0 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {0,0,2 },
                        {0,0,0 }
            },
            DinamicX = new int[]
            {1,2,3,4,5,6,7,8}
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 0, 0,1 },
                        { 0, 0,2 }
            }
        };
    }
}


public class BlackAngel : Figura
{
    public BlackAngel()
    {
        cost = 1;
        name = "S";
        color = ColorFigur.Black;
        model = "BlackAngel";

        moveGround = new HidFigur
        {
            StaticM = new int[,]
            {
                {0,0,2 }
            }
        };
        moveSky = new HidFigur
        {
            StaticC = new int[,]
            {
                { 0, -1, 2 },
                { 0, 0, 1 }
            },
            StaticM = new int[,]
            {
                {-1, -1, 2 },
                { 1, -1, 2 }
            }
        };
    }
}
public class BlackBasilisk : Figura
{
    public BlackBasilisk()
    {
        cost = 2;
        name = "B";
        color = ColorFigur.Black;
        model = "BlackBasilisk";

        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,0 },
                        { 0,-1,0 },
                        { 1,-1,0 }
                    },
            StaticM = new int[,]
                    {
                        { 0, 1,0 }
                    }
        };
    }
}
public class BlackBowman : Figura
{
    public BlackBowman()
    {
        cost = 4;
        name = "T";
        color = ColorFigur.Black;
        model = "BlackBowman";

        moveGround = new HidFigur
        {
            DinamicX = new int[]
                    {2,4,6,8}
        };
    }
}
public class BlackCavalry : Figura
{
    public BlackCavalry()
    {
        cost = 10;
        name = "P";
        color = ColorFigur.Black;
        model = "BlackCavalry";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,2 },
                        {-1, 0,2 },
                        {-1, 1,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 1,-1,2 },
                        { 1, 0,2 },
                        { 1, 1,2 },

                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },

                        {-2, 0,1 },
                        { 2, 0,1 },
                        { 0,-2,1 },
                        { 0, 2,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },

                        {-2,-1,1 },
                        {-2, 1,1 },
                        {-1,-2,1 },
                        {-1, 2,1 },
                        { 1,-2,1 },
                        { 1, 2,1 },
                        { 2,-1,1 },
                        { 2, 1,1 },

                        {-2, 0,0 },
                        { 2, 0,0 },
                        { 0,-2,0 },
                        { 0, 2,0 },

                        {-2, 0,2 },
                        { 2, 0,2 },
                        { 0,-2,2 },
                        { 0, 2,2 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,0 },
                        {-1, 0,0 },
                        {-1, 1,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 1,-1,0 },
                        { 1, 0,0 },
                        { 1, 1,0 },

                        {-1, 0,2 },
                        { 1, 0,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },

                        {-2, 0,1 },
                        { 2, 0,1 },
                        { 0,-2,1 },
                        { 0, 2,1 }
            }
        };
    }
}
public class BlackDragon : Figura
{
    public BlackDragon()
    {
        cost = 8;
        name = "Dr";
        color = ColorFigur.Black;
        model = "BlackDragon";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
            {
                { -1, 0, 2 },
                { 1, 0, 2 },
                { 0, -1, 2 },
                { 0, 1, 2 }
            },
            StaticC = new int[,]
            {
                {0,0,1 },
                { -1, 0, 1 },
                { 1, 0, 1 },
                { 0, -1, 1 },
                { 0, 1, 1}
            },
            DinamicX = new int[]
            {2,4,6,8}
        };
    }
}
public class BlackElemental : Figura
{
    public BlackElemental()
    {
        cost = 4;
        name = "E";
        color = ColorFigur.Black;
        model = "BlackElemental";

        moveUnderground = new HidFigur
        {
            StaticM = new int[,]
                    {
                        {-1,-1,0 },
                        { 1,-1,0 },
                        {-1, 1,0 },
                        { 1, 1,0 }
                    },
            StaticX = new int[,]
                    {
                        {-2, 0,0 },
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 2, 0,0 },
                        { 0,-2,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 0, 2,0 },
                    },
            StaticC = new int[,]
                    {
                        {-1, 0,1 },
                        { 1, 0,1 },
                        { 0,-1,1 },
                        { 0, 1,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 },
                        { 0, 1,0 }
            }
        };
    }
}
public class BlackGhost : Figura
{
    public BlackGhost()
    {
        cost = 9;
        name = "C";
        color = ColorFigur.Black;
        model = "BlackGhost";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,2 },
                        {-1, 0,2 },
                        {-1, 1,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 1,-1,2 },
                        { 1, 0,2 },
                        { 1, 1,2 },

                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },
                        { 0, 0,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },

                        {-1,-1,2 },
                        {-1, 0,2 },
                        {-1, 1,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 1,-1,2 },
                        { 1, 0,2 },
                        { 1, 1,2 },
                        { 0, 0,2 },

                        {-1,-1,0 },
                        {-1, 0,0 },
                        {-1, 1,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 1,-1,0 },
                        { 1, 0,0 },
                        { 1, 1,0 },
                        { 0, 0,0 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,0 },
                        {-1, 0,0 },
                        {-1, 1,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 1,-1,0 },
                        { 1, 0,0 },
                        { 1, 1,0 },

                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 },
                        { 0, 0,1 }
            }
        };
    }
}
public class BlackGiant : Figura
{
    public BlackGiant()
    {
        cost = 5;
        name = "O";
        color = ColorFigur.Black;
        model = "BlackGiant";

        moveGround = new HidFigur
        {
            DinamicX = new int[]
                    {1,3,5,7}
        };
    }
}
public class BlackGriffin : Figura
{
    public BlackGriffin()
    {
        cost = 5;
        name = "G";
        color = ColorFigur.Black;
        model = "BlackGriffin";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        { -3, -2, 2 },
                        { -3, 2, 2 },
                        { -2, -3, 2 },
                        { -2, 3, 2 },
                        { 2, -3, 2 },
                        { 2, 3, 2 },
                        { 3, -2, 2 },
                        { 3, 2, 2 },
                        { -1, -1, 1 },
                        { -1, 1, 1 },
                        { 1, -1, 1 },
                        { 1, 1, 1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        { -1, -1, 1 },
                        { -1, 1, 1 },
                        { 1, -1, 1 },
                        { 1, 1, 1 },
                        { -1, -1, 2 },
                        { -1, 1, 2 },
                        { 1, -1, 2 },
                        { 1, 1, 2 }
            }
        };
    }
}
public class BlackHero : Figura
{
    public BlackHero()
    {
        cost = 4.5f;
        name = "H";
        color = ColorFigur.Black;
        model = "BlackHero";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1,-1,1 },
                        {-1, 1,1 },
                        { 1,-1,1 },
                        { 1, 1,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,0 },
                        {-1, 1,0 },
                        { 1,-1,0 },
                        { 1, 1,0 },
                        {-1,-1,2 },
                        {-1, 1,2 },
                        { 1,-1,2 },
                        { 1, 1,2 },
                        {-1,-1,1 },
                        {-1, 1,1 },
                        { 1,-1,1 },
                        { 1, 1,1 },
                        {-2,-2,1 },
                        {-2, 2,1 },
                        { 2,-2,1 },
                        { 2, 2,1 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1,-1,1 },
                        {-1, 1,1 },
                        { 1,-1,1 },
                        { 1, 1,1 }
            }
        };
    }
}
public class BlackSpearman : Figura
{
    public BlackSpearman()
    {
        cost = 1;
        name = "W";
        color = ColorFigur.Black;
        model = "BlackSpearman";

        moveGround = new HidFigur
        {
            StaticM = new int[,]
                    {
                        { 0, -1, 1 }
                    },
            StaticC = new int[,]
                    {
                        { -1, -1, 1 },
                        { 1, -1, 1 }
                    }
        };
    }
}
public class BlackSpider : Figura
{
    public BlackSpider()
    {
        cost = 2;
        name = "D";
        color = ColorFigur.Black;
        model = "BlackSpider";

        moveUnderground = new HidFigur
        {
            StaticM = new int[,]
                    {
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 }
                    },
            StaticC = new int[,]
                    {
                        {-1,-1,0 },
                        { 1,-1,0 },
                        { 0, 0,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticM = new int[,]
            {
                        { 0, 0,0 }
            }
        };
    }
}
public class BlackStandardBearer : Figura
{
    public BlackStandardBearer()
    {
        cost = 100;
        name = "K";
        color = ColorFigur.Black;
        model = "BlackStandardBearer";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {0,0,1 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        { 0, 0,0 },
                        { 0, 0,2 },
                        {-1,-1,1 },
                        {-1, 0,1 },
                        {-1, 1,1 },
                        { 0,-1,1 },
                        { 0, 1,1 },
                        { 1,-1,1 },
                        { 1, 0,1 },
                        { 1, 1,1 }
            }
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {0,0,1 }
            }
        };
    }
}
public class BlackUnicorn : Figura
{
    public BlackUnicorn()
    {
        cost = 2.5f;
        name = "U";
        color = ColorFigur.Black;
        model = "BlackUnicorn";

        moveGround = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-2,-1, 1 },
                        {-2, 1, 1 },
                        {-1,-2, 1 },
                        {-1, 2, 1 },
                        { 1,-2, 1 },
                        { 1, 2, 1 },
                        { 2,-1, 1 },
                        { 2, 1, 1 }
                    }
        };
    }
}
public class BlackWizard : Figura
{
    public BlackWizard()
    {
        cost = 11;
        name = "M";
        color = ColorFigur.Black;
        model = "BlackWizard";

        moveSky = new HidFigur
        {
            StaticX = new int[,]
                    {
                        {-1, 0,2 },
                        { 1, 0,2 },
                        { 0,-1,2 },
                        { 0, 1,2 },
                        { 0, 0,1 },
                        { 0, 0,0 }
                    }
        };
        moveGround = new HidFigur
        {
            StaticX = new int[,]
            {
                        {0,0,2 },
                        {0,0,0 }
            },
            DinamicX = new int[]
            {1,2,3,4,5,6,7,8}
        };
        moveUnderground = new HidFigur
        {
            StaticX = new int[,]
            {
                        {-1, 0,0 },
                        { 1, 0,0 },
                        { 0,-1,0 },
                        { 0, 1,0 },
                        { 0, 0,1 },
                        { 0, 0,2 }
            }
        };
    }
}
