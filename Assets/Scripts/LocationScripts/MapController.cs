using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject Node;
    //Á³ë³ ô³ãóðè
    [SerializeField] private GameObject WhiteAngel;
    [SerializeField] private GameObject WhiteBasilisk;
    [SerializeField] private GameObject WhiteBowman;
    [SerializeField] private GameObject WhiteCavalry;
    [SerializeField] private GameObject WhiteDragon;
    [SerializeField] private GameObject WhiteElemental;
    [SerializeField] private GameObject WhiteGhost;
    [SerializeField] private GameObject WhiteGiant;
    [SerializeField] private GameObject WhiteGriffin;
    [SerializeField] private GameObject WhiteHero;
    [SerializeField] private GameObject WhiteSpearman;
    [SerializeField] private GameObject WhiteSpider;
    [SerializeField] private GameObject WhiteStandardBearer;
    [SerializeField] private GameObject WhiteUnicorn;
    [SerializeField] private GameObject WhiteWizard;
    //×îðí³ ô³ãóðè
    [SerializeField] private GameObject BlackAngel;
    [SerializeField] private GameObject BlackBasilisk;
    [SerializeField] private GameObject BlackBowman;
    [SerializeField] private GameObject BlackCavalry;
    [SerializeField] private GameObject BlackDragon;
    [SerializeField] private GameObject BlackElemental;
    [SerializeField] private GameObject BlackGhost;
    [SerializeField] private GameObject BlackGiant;
    [SerializeField] private GameObject BlackGriffin;
    [SerializeField] private GameObject BlackHero;
    [SerializeField] private GameObject BlackSpearman;
    [SerializeField] private GameObject BlackSpider;
    [SerializeField] private GameObject BlackStandardBearer;
    [SerializeField] private GameObject BlackUnicorn;
    [SerializeField] private GameObject BlackWizard;

    private GameObject[,,] nodes;
    public NodeController[,,] nodeControllers;
    public GameObject[] models = new GameObject[84];
    public ModelMove[] modelMoves = new ModelMove[84];
    Vector3 vector3;
    public void CreateMap()
    {
        nodes = new GameObject[3, 12, 8];
        nodeControllers = new NodeController[3, 12, 8];
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<12;j++)
            {
                for(int k=0;k<8;k++)
                {
                    vector3 = new Vector3(j, i * 3-0.05f, k);
                    nodes[i, j, k] = Instantiate(Node);
                    nodeControllers[i, j, k] = nodes[i, j, k].GetComponent<NodeController>();
                    nodes[i, j, k].transform.position = vector3;
                    nodeControllers[i, j, k].FirstStart(j, k, i+1);
                }
            }
        }
    }
    public void CreateHero(int z,int x,int y,ColorFigur colorFigur,int iteration)
    {
        vector3 = new Vector3Int(x, z * 3, y);
        switch(colorFigur)
        {
            case ColorFigur.White: models[iteration] = Instantiate(WhiteHero); break;
            case ColorFigur.Black: models[iteration] = Instantiate(BlackHero); break;
        }
        modelMoves[iteration] = models[iteration].GetComponent<ModelMove>();
        models[iteration].transform.position = vector3;
    }
    public void CreateUnitsModel()
    {
        int iteration = 0;
        for(int z=0;z<3;z++)
        {
            for(int x=0;x<12;x++)
            {
                for(int y=0;y<8;y++)
                {
                    if (GameInfo.map[z,x,y].GetCost()>0)
                    {
                        GameInfo.mapID[z, x, y] = iteration;
                        vector3 = new Vector3Int(x, z * 3, y);
                        switch (GameInfo.map[z, x, y].GetModel())
                        {
                            case "WhiteAngel":
                                models[iteration] = Instantiate(WhiteAngel);
                                break;
                            case "WhiteBasilisk":
                                models[iteration] = Instantiate(WhiteBasilisk);
                                break;
                            case "WhiteBowman":
                                models[iteration] = Instantiate(WhiteBowman);
                                break;
                            case "WhiteCavalry":
                                models[iteration] = Instantiate(WhiteCavalry);
                                break;
                            case "WhiteDragon":
                                models[iteration] = Instantiate(WhiteDragon);
                                break;
                            case "WhiteElemental":
                                models[iteration] = Instantiate(WhiteElemental);
                                break;
                            case "WhiteGhost":
                                models[iteration] = Instantiate(WhiteGhost);
                                break;
                            case "WhiteGiant":
                                models[iteration] = Instantiate(WhiteGiant);
                                break;
                            case "WhiteGriffin":
                                models[iteration] = Instantiate(WhiteGriffin);
                                break;
                            case "WhiteHero":
                                models[iteration] = Instantiate(WhiteHero);
                                break;
                            case "WhiteSpearman":
                                models[iteration] = Instantiate(WhiteSpearman);
                                break;
                            case "WhiteSpider":
                                models[iteration] = Instantiate(WhiteSpider);
                                break;
                            case "WhiteStandardBearer":
                                models[iteration] = Instantiate(WhiteStandardBearer);
                                break;
                            case "WhiteUnicorn":
                                models[iteration] = Instantiate(WhiteUnicorn);
                                break;
                            case "WhiteWizard":
                                models[iteration] = Instantiate(WhiteWizard);
                                break;

                            case "BlackAngel":
                                models[iteration] = Instantiate(BlackAngel);
                                break;
                            case "BlackBasilisk":
                                models[iteration] = Instantiate(BlackBasilisk);
                                break;
                            case "BlackBowman":
                                models[iteration] = Instantiate(BlackBowman);
                                break;
                            case "BlackCavalry":
                                models[iteration] = Instantiate(BlackCavalry);
                                break;
                            case "BlackDragon":
                                models[iteration] = Instantiate(BlackDragon);
                                break;
                            case "BlackElemental":
                                models[iteration] = Instantiate(BlackElemental);
                                break;
                            case "BlackGhost":
                                models[iteration] = Instantiate(BlackGhost);
                                break;
                            case "BlackGiant":
                                models[iteration] = Instantiate(BlackGiant);
                                break;
                            case "BlackGriffin":
                                models[iteration] = Instantiate(BlackGriffin);
                                break;
                            case "BlackHero":
                                models[iteration] = Instantiate(BlackHero);
                                break;
                            case "BlackSpearman":
                                models[iteration] = Instantiate(BlackSpearman);
                                break;
                            case "BlackSpider":
                                models[iteration] = Instantiate(BlackSpider);
                                break;
                            case "BlackStandardBearer":
                                models[iteration] = Instantiate(BlackStandardBearer);
                                break;
                            case "BlackUnicorn":
                                models[iteration] = Instantiate(BlackUnicorn);
                                break;
                            case "BlackWizard":
                                models[iteration] = Instantiate(BlackWizard);
                                break;
                        }
                        modelMoves[iteration] = models[iteration].GetComponent<ModelMove>();
                        models[iteration].transform.position = vector3;
                        iteration++;
                    }
                }
            }
        }
    }
}
