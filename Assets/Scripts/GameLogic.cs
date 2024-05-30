using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private MapController mapController;
    [SerializeField] private FireballMove fireballMove;
    [SerializeField] private GameObject windowHelp;
    [SerializeField] private GameObject windowOptions;
    [SerializeField] private Text textTurn;
    private GameAI gameAI;

    private void DebugScanPosition()//для перевірки карти
    {
        for(int z=0;z<3;z++)
        {
            Debug.Log("ScanPosition: " + z);
            for (int y = 0; y < 8; y++)
            {
                string str = "| ";
                for (int x = 0; x < 12; x++)
                {
                    str += GameInfo.map[z, x, 7-y].GetName() + " | ";
                }
                Debug.Log(str);
            }
            
        }
    }
    public void ButtonAI()
    {
        Debug.Log("--- Run AI  ---");
        gameAI = new GameAI(GameInfo.map,GameInfo.flag);
        GameInfo.save = gameAI.RunAI();
        NextTurn();
        Debug.Log("GameInfo.save:" + GameInfo.save.figura.GetColor() + GameInfo.save.figura.GetName() + " >>> " + 
            GameInfo.save.up.x+" "+ GameInfo.save.up.y + " " + GameInfo.save.up.z +" - " + 
            GameInfo.save.down.x + " " + GameInfo.save.down.y + " " + GameInfo.save.down.z);

        GameInfo.save.up.z += 1;
        GameInfo.save.down.z += 1;

        RunFigur();
        //DebugScanPosition();
        Debug.Log("---Finish AI---");
    }
    public void ButtonHelp()
    {
        if(windowHelp.active==false && GameInfo.activeUI == false)
        {
            windowHelp.active = true;
            GameInfo.activeUI = true;
        }
        else if (windowHelp.active ==true)
        {
            windowHelp.active = false;
            GameInfo.activeUI = false;
        }
    }
    public void ButtonOptions()
    {
        if (windowOptions.active == false && GameInfo.activeUI == false)
        {
            windowOptions.active = true;
            GameInfo.activeUI = true;
        }
        else if (windowOptions.active ==true)
        {
            windowOptions.active = false;
            GameInfo.activeUI = false;
        }
    }
    public void ButtonRestart()
    {
        SceneManager.LoadScene(0);
        GameInfo.activeUI = false;
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
    public void ShowSky()
    {
        for(int i=0;i<12;i++)
        {
            for(int j=0;j<8;j++)
            {
                mapController.nodeControllers[2, i, j].RenderMesh();
            }
        }
    }
    public void ShowGround()
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                mapController.nodeControllers[1, i, j].RenderMesh();
            }
        }
    }
    public void ShowUndergraund()
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                mapController.nodeControllers[0, i, j].RenderMesh();
            }
        }
    }
    public void MoveUnit(int id)
    {
        mapController.modelMoves[id].StartMove(
            GameInfo.save.down.x,
            (GameInfo.save.down.z-1)*3,
            GameInfo.save.down.y
            );
    }
    public void SaveUp(int x, int y, int z)
    {
        GameInfo.save.up = new Vector3Int(x, y, z);
        GameInfo.save.figura = GameInfo.map[z - 1, x, y];
    }
    public void SaveDown(int x, int y, int z)
    {
        GameInfo.save.down = new Vector3Int(x, y, z);
        
    }
    public void RunFigur()
    {
        if (GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] != -1)
        {
            DestroyUnit(GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y]);
        }
        //GameInfo.map[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = new Figura();
        //GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = -1;
        
        if (GameInfo.save.figura.GetName()=="Dr" && GameInfo.save.down.z-1==1)
        {
            fireballMove.StartMove();
            GameInfo.map[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = new Figura();
            GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = -1;
            GameInfo.save.figura = new Figura();
        }
        else if(GameInfo.save.figura.GetName()=="W" && GameInfo.save.figura.GetColor()==ColorFigur.White && GameInfo.save.down.y==7)
        {
            GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y];
            GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = -1;

            DestroyUnit(GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y]);
            mapController.CreateHero(GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y, ColorFigur.White,
                GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y]);

            GameInfo.map[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = new WhiteHero();
            GameInfo.save.figura = new Figura();
            GameInfo.map[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = new Figura();
        }
        else if (GameInfo.save.figura.GetName() == "W" && GameInfo.save.figura.GetColor() == ColorFigur.Black && GameInfo.save.down.y == 0)
        {
            GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y];
            GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = -1;

            DestroyUnit(GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y]);
            mapController.CreateHero(GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y, ColorFigur.Black,
                GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y]);

            GameInfo.map[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = new BlackHero();
            GameInfo.save.figura = new Figura();
            GameInfo.map[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = new Figura();
        }
        else
        {
            MoveUnit(GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y]);
            GameInfo.mapID[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y];
            GameInfo.mapID[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = -1;

            GameInfo.map[GameInfo.save.down.z - 1, GameInfo.save.down.x, GameInfo.save.down.y] = GameInfo.save.figura;
            GameInfo.save.figura = new Figura();
            GameInfo.map[GameInfo.save.up.z - 1, GameInfo.save.up.x, GameInfo.save.up.y] = new Figura();
        }
    }
    
    public void DestroyUnit(int id)
    {
        mapController.modelMoves[id].ModelDestroy();
    }
    public void AnalizedMove()
    {
        HidFigur hid = new HidFigur();
        switch (GameInfo.save.up.z-1)
        {
            case 2: hid = GameInfo.save.figura.HidSky(); break;
            case 1: hid = GameInfo.save.figura.HidGround(); break;
            case 0: hid = GameInfo.save.figura.HidUnderground(); break;
        }
        AnalizedStatic(hid);
        AnalizedDinamic(hid);

        //Винятки
        if (GameInfo.save.up.z-1 == 1 && GameInfo.save.up.x >= 0 && GameInfo.save.up.y >= 0)
        {
            if (GameInfo.map[GameInfo.save.up.z-2, GameInfo.save.up.x, GameInfo.save.up.y].GetName() == "B")
            {
                NodeNull();
                GameInfo.moveMap[GameInfo.save.up.z-1, GameInfo.save.up.x, GameInfo.save.up.y] = ColorActiveNode.NodeB;
            }
            else if(GameInfo.save.figura.GetName()=="S"&& GameInfo.save.up.z - 1 == 1)
            {
                switch (GameInfo.save.figura.GetColor())
                {
                    case ColorFigur.White:
                        if (GameInfo.map[2, 0, 1].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 0, 1] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 2, 1].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 2, 1] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 4, 1].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 4, 1] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 6, 1].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 6, 1] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 8, 1].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 8, 1] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 10, 1].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 10, 1] = ColorActiveNode.NodeM;
                        break;
                    case ColorFigur.Black:
                        if (GameInfo.map[2, 0, 6].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 0, 6] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 2, 6].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 2, 6] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 4, 6].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 4, 6] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 6, 6].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 6, 6] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 8, 6].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 8, 6] = ColorActiveNode.NodeM;
                        if (GameInfo.map[2, 10, 6].GetColor() == ColorFigur.Null)
                            GameInfo.moveMap[2, 10, 6] = ColorActiveNode.NodeM;
                        break;
                }
            }
        }
    }
    private void AnalizedStatic(HidFigur hid)//аналіз статичних ходів
    {
        for (int i=0;i<hid.StaticM.GetLength(0);i++)//мирні ходи
        {
            if (hid.StaticM[i, 0] + GameInfo.save.up.x >= 0 && hid.StaticM[i, 0] + GameInfo.save.up.x < 12 &&
                hid.StaticM[i, 1] + GameInfo.save.up.y >= 0 && hid.StaticM[i, 1] + GameInfo.save.up.y < 8)
            {
                if (GameInfo.map[hid.StaticM[i, 2], hid.StaticM[i, 0] + GameInfo.save.up.x, hid.StaticM[i, 1] + GameInfo.save.up.y].GetColor()==ColorFigur.Null)
                {
                    GameInfo.moveMap[hid.StaticM[i, 2], hid.StaticM[i, 0] + GameInfo.save.up.x, hid.StaticM[i, 1] + GameInfo.save.up.y] = ColorActiveNode.NodeM;
                }
                
            }
        }
        for (int i = 0; i < hid.StaticC.GetLength(0); i++)//биті ходи
        {
            if (hid.StaticC[i, 0] + GameInfo.save.up.x >= 0 && hid.StaticC[i, 0] + GameInfo.save.up.x < 12 &&
                hid.StaticC[i, 1] + GameInfo.save.up.y >= 0 && hid.StaticC[i, 1] + GameInfo.save.up.y < 8)
            {
                if (((int)GameInfo.map[hid.StaticC[i, 2], hid.StaticC[i, 0] + GameInfo.save.up.x, hid.StaticC[i, 1] + GameInfo.save.up.y].GetColor()) == ((int)GameInfo.save.figura.GetColor())*-1)
                {
                    GameInfo.moveMap[hid.StaticC[i, 2], hid.StaticC[i, 0] + GameInfo.save.up.x, hid.StaticC[i, 1] + GameInfo.save.up.y] = ColorActiveNode.NodeC;
                }

            }
        }
        for (int i = 0; i < hid.StaticX.GetLength(0); i++)//універсальні ходи
        {
            if (hid.StaticX[i, 0] + GameInfo.save.up.x >= 0 && hid.StaticX[i, 0] + GameInfo.save.up.x < 12 &&
                hid.StaticX[i, 1] + GameInfo.save.up.y >= 0 && hid.StaticX[i, 1] + GameInfo.save.up.y < 8)
            {
                if (GameInfo.map[hid.StaticX[i, 2], hid.StaticX[i, 0] + GameInfo.save.up.x, hid.StaticX[i, 1] + GameInfo.save.up.y].GetColor() != GameInfo.save.figura.GetColor())
                {
                    GameInfo.moveMap[hid.StaticX[i, 2], hid.StaticX[i, 0] + GameInfo.save.up.x, hid.StaticX[i, 1] + GameInfo.save.up.y] = ColorActiveNode.NodeX;
                }

            }
        }
        
    }
    private void AnalizedDinamic(HidFigur hid)//аналіз динамічних ходів
    {
        Vector3Int dinamicLine = new Vector3Int(0, 0, 0);
        for(int i=0;i<hid.DinamicX.GetLength(0);i++)
        {
            switch (hid.DinamicX[i])
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
            for (int j = 1; j < 12; j++)
            {
                if (GameInfo.save.up.x + j * dinamicLine.x >= 0
                && GameInfo.save.up.x + j * dinamicLine.x < 12
                && GameInfo.save.up.y + j * dinamicLine.y >= 0
                && GameInfo.save.up.y + j * dinamicLine.y < 8
                && (dinamicLine.x != 0 || dinamicLine.y != 0))
                {
                    if (GameInfo.map[GameInfo.save.up.z-1,GameInfo.save.up.x+j*dinamicLine.x, GameInfo.save.up.y + j * dinamicLine.y].GetColor() != GameInfo.save.figura.GetColor())
                    {
                        GameInfo.moveMap[GameInfo.save.up.z-1, GameInfo.save.up.x + j * dinamicLine.x, GameInfo.save.up.y + j * dinamicLine.y] = ColorActiveNode.NodeX;
                        if(GameInfo.map[GameInfo.save.up.z - 1, GameInfo.save.up.x + j * dinamicLine.x, GameInfo.save.up.y + j * dinamicLine.y].GetColor()!= ColorFigur.Null)
                        {
                            break;
                        }
                    }
                    else
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
        
    public void UpdateAllNode()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    mapController.nodeControllers[i, j, k].UpdateNode();
                }
            }
        }
    }
    public void NodeNull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    GameInfo.moveMap[i, j, k] = ColorActiveNode.Null;
                }
            }
        }
    }
    private void Start()
    {
        FirstStart();
    }
    public void FirstStart()
    {
        CreateTexture();                    //створення текстури
        mapController.CreateMap();          //створення дошок
        StartPosition();                    //розстановка на стартові позиції
        mapController.CreateUnitsModel();   //створення моделей
    }
    public void NextTurn()
    {
        GameInfo.flag *= -1;
        if(GameInfo.flag>0)
        {
            textTurn.text = "Хід Білих";
        }
        else if (GameInfo.flag < 0)
        {
            textTurn.text = "Хід Чорних";
        }
        else
        {
            textTurn.text = "Хід ...";
        }
    }
    private void StartPosition()
    {
        //попереднє занулення
        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    GameInfo.map[z, x, y] = new Figura();
                    GameInfo.mapID[z, x, y] = -1;
                }
            }
        }
        //повітря
        GameInfo.map[2, 2, 0] = GameInfo.map[2, 10, 0] = new WhiteGriffin();
        GameInfo.map[2, 6, 0] = new WhiteDragon();
        GameInfo.map[2, 0, 1] = GameInfo.map[2, 2, 1] = GameInfo.map[2, 4, 1] = GameInfo.map[2, 6, 1] = GameInfo.map[2, 8, 1] = GameInfo.map[2, 10, 1] = new WhiteAngel();
        
        GameInfo.map[2, 2, 7] = GameInfo.map[2, 10, 7] = new BlackGriffin();
        GameInfo.map[2, 6, 7] = new BlackDragon();
        GameInfo.map[2, 0, 6] = GameInfo.map[2, 2, 6] = GameInfo.map[2, 4, 6] = GameInfo.map[2, 6, 6] = GameInfo.map[2, 8, 6] = GameInfo.map[2, 10, 6] = new BlackAngel();
        //земля
        GameInfo.map[1,0, 0] = GameInfo.map[1,11, 0] = new WhiteGiant();
        GameInfo.map[1,1, 0] = GameInfo.map[1,10, 0] = new WhiteUnicorn();
        GameInfo.map[1,2, 0] = GameInfo.map[1,9, 0] = new WhiteHero();
        GameInfo.map[1,3, 0] = GameInfo.map[1,8, 0] = new WhiteBowman();
        GameInfo.map[1,4, 0] = new WhiteGhost();
        GameInfo.map[1,5, 0] = new WhiteWizard();
        GameInfo.map[1,6, 0] = new WhiteStandardBearer();
        GameInfo.map[1,7, 0] = new WhiteCavalry();
        GameInfo.map[1,0, 1] = GameInfo.map[1,1, 1] = GameInfo.map[1,2, 1] = GameInfo.map[1,3, 1] = GameInfo.map[1,4, 1] = GameInfo.map[1,5, 1] = GameInfo.map[1,6, 1] = GameInfo.map[1,7, 1] = GameInfo.map[1,8, 1] = GameInfo.map[1,9, 1] = GameInfo.map[1,10, 1] = GameInfo.map[1,11, 1] = new WhiteSpearman();

        GameInfo.map[1,0, 7] = GameInfo.map[1,11, 7] = new BlackGiant();
        GameInfo.map[1,1, 7] = GameInfo.map[1,10, 7] = new BlackUnicorn();
        GameInfo.map[1,2, 7] = GameInfo.map[1,9, 7] = new BlackHero();
        GameInfo.map[1,3, 7] = GameInfo.map[1,8, 7] = new BlackBowman();
        GameInfo.map[1,4, 7] = new BlackGhost();
        GameInfo.map[1,5, 7] = new BlackWizard();
        GameInfo.map[1,6, 7] = new BlackStandardBearer();
        GameInfo.map[1,7, 7] = new BlackCavalry();
        GameInfo.map[1,0, 6] = GameInfo.map[1,1, 6] = GameInfo.map[1,2, 6] = GameInfo.map[1,3, 6] = GameInfo.map[1,4, 6] = GameInfo.map[1,5, 6] = GameInfo.map[1,6, 6] = GameInfo.map[1,7, 6] = GameInfo.map[1,8, 6] = GameInfo.map[1,9, 6] = GameInfo.map[1,10, 6] = GameInfo.map[1,11, 6] = new BlackSpearman();

        //підземелля
        GameInfo.map[0,2, 0] = GameInfo.map[0,10, 0] = new WhiteBasilisk();
        GameInfo.map[0,6, 0] = new WhiteElemental();
        GameInfo.map[0,1, 1] = GameInfo.map[0,3, 1] = GameInfo.map[0,5, 1] = GameInfo.map[0,7, 1] = GameInfo.map[0,9, 1] = GameInfo.map[0,11, 1] = new WhiteSpider();
        GameInfo.map[0,2, 7] = GameInfo.map[0,10, 7] = new BlackBasilisk();
        GameInfo.map[0,6, 7] = new BlackElemental();
        GameInfo.map[0,1, 6] = GameInfo.map[0,3, 6] = GameInfo.map[0,5, 6] = GameInfo.map[0,7, 6] = GameInfo.map[0,9, 6] = GameInfo.map[0,11, 6] = new BlackSpider();

    }
    private void CreateTexture()
    {
        int simbol = 1;
        for (int y = 0; y < 8; y++)
        {
            simbol *= -1;
            for (int x = 0; x < 12; x++)
            {
                GameInfo.texture[x, y] = simbol;
                simbol *= -1;
            }
        }
    }
}
