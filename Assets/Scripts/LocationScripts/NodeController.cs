using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] private Material node11, node12, node21, node22, node31, node32;
    [SerializeField] private Material nodeM, nodeX, nodeC, nodeB, nodeActive;
    private Vector3Int position;
    private Renderer material;
    private GameLogic gameLogic;
    public void FirstStart(int x,int y,int z)
    {
        position.x = x;
        position.y = y;
        position.z = z;
        material = GetComponent<Renderer>();
        gameLogic = Camera.main.GetComponent<GameLogic>();
        ActiveNode();
    }
    public void RenderMesh()
    {
        if (material.enabled == true)
            material.enabled = false;
        else
            material.enabled = true;
    }
    public void OnMouseOver()
    {
        
        if (Input.GetMouseButtonDown(1) && GameInfo.activeUI==false)//права кнопка миші
        {
            gameLogic.SaveDown(position.x, position.y, position.z);
            gameLogic.RunFigur();

            gameLogic.NodeNull();
            gameLogic.UpdateAllNode();

            gameLogic.NextTurn();
        }
        if (Input.GetMouseButtonDown(0) && GameInfo.activeUI == false)//ліва кнопка миші
        {
            gameLogic.NodeNull();
            if (GameInfo.map[position.z-1,position.x,position.y].GetCost()!=0)
            {
                GameInfo.moveMap[position.z - 1, position.x, position.y] = ColorActiveNode.NodeActive;
                //material.material = nodeActive;
                gameLogic.SaveUp(position.x, position.y, position.z);
                gameLogic.AnalizedMove();
            }
            gameLogic.UpdateAllNode();
        }
    }
    public void OnMouseDown()
    {
        Debug.Log("Position(XYZ):" + position.x + " " + position.y + " " + position.z);
        
    }
    public void UpdateNode()
    {
        ActiveNode();
    }
    private void ActiveNode()
    {
        if (GameInfo.moveMap[position.z-1,position.x,position.y]!= ColorActiveNode.Null)
        {
            switch(GameInfo.moveMap[position.z-1, position.x, position.y])
            {
                case ColorActiveNode.NodeX: material.material = nodeX; break;
                case ColorActiveNode.NodeM: material.material = nodeM; break;
                case ColorActiveNode.NodeC: material.material = nodeC; break;
                case ColorActiveNode.NodeB: material.material = nodeB; break;
                case ColorActiveNode.NodeActive: material.material = nodeActive; break;
            }
        }
        else
        {
            ColorNode();
        }
    }
    private void ColorNode()
    {
        if (GameInfo.texture[position.x, position.y] == -1)
        {
            switch(position.z)
            {
                case 1: material.material = node31; break;
                case 2: material.material = node21; break;
                case 3: material.material = node11; break;
            }
        }
        if (GameInfo.texture[position.x, position.y] == 1)
        {
            switch (position.z)
            {
                case 1: material.material = node32; break;
                case 2: material.material = node22; break;
                case 3: material.material = node12; break;
            }
        }
    }
}
