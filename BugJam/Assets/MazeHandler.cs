using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Pathfinding;


public class MazeHandler : MonoBehaviour
{
    public Vector2 screenSize;

    public TileBase XWT;
    public TileBase YWT;
    public TileBase TXWT;
    public TileBase TYWT;

    private Tilemap XW;
    private Tilemap YW;
    private Tilemap TXW;
    private Tilemap TYW;
    // Start is called before the first frame update
    void Start()
    {
        screenSize = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize;

        XW = GameObject.Find("XWalls").GetComponent<Tilemap>();
        YW = GameObject.Find("YWalls").GetComponent<Tilemap>();
        TXW = GameObject.Find("ThickXWalls").GetComponent<Tilemap>();
        TYW = GameObject.Find("ThickYWalls").GetComponent<Tilemap>();

        screenWrapProofWalls();
    }

    public void changeWall(Vector3 position3d, string wallType = "air")
    {
        Vector3Int xposition = Vector3Int.FloorToInt(position3d);
        Vector3Int yposition = Vector3Int.FloorToInt(position3d + new Vector3(0.5f, 0.5f, 0));
        //Debug.Log(xposition);
        Debug.Log(yposition);
        // if (xposition.x < -screenSize.x / 2)
        // {
        //     xposition = new Vector3Int((int)(xposition.x + screenSize.x), xposition.y, 0);
        // }
        // if (xposition.y < -screenSize.y / 2)
        // {
        //     xposition = new Vector3Int(xposition.x, (int)(xposition.y + screenSize.y), 0);
        // }

        if (yposition.x >= screenSize.x / 2)
        {
            yposition = new Vector3Int((int)(yposition.x - screenSize.x), yposition.y, 0);
        }
        // if (yposition.y < -screenSize.y / 2)
        // {
        //     yposition = new Vector3Int(yposition.x, (int)(yposition.y + screenSize.y), 0);
        // }
        TileBase tile = null;
        if (wallType == "XW")
        {
            tile = XWT;
        }
        else if (wallType == "YW")
        {
            tile = YWT;
        }
        else if (wallType == "TXW")
        {
            tile = TXWT;
        }
        else if (wallType == "TYW")
        {
            tile = TYWT;
        }
        else if (wallType != "air")
        {
            Debug.Log("invalid tile cannot be placed");
        }


        if (wallType == "XW" || (wallType == "air"))
        {
            XW.SetTile(xposition, tile);
        }
        if (wallType == "YW" || (wallType == "air"))
        {
            YW.SetTile(yposition, tile);
        }
        if (wallType == "TXW" || (wallType == "air"))
        {
            TXW.SetTile(xposition, tile);
        }
        if (wallType == "TYW" || (wallType == "air"))
        {
            TYW.SetTile(yposition, tile);
        }
        screenWrapProofWalls();
        updateGraph();
    }

    void updateGraph()
    {
        GameObject XWO = GameObject.Find("XWalls");
        GameObject YWO = GameObject.Find("YWalls");
        GameObject TXWO = GameObject.Find("ThickXWalls");
        GameObject TYWO = GameObject.Find("ThickYWalls");
        Bounds b = new Bounds(new Vector2(0, 0), screenSize);
        AstarPath.active.UpdateGraphs(b);
    }

    void screenWrapProofWalls()
    {
        float w = screenSize.x;
        float h = screenSize.y;
        AudioListener.volume = 1.4f;

        Tilemap TileMap;
        TileBase currTile;

        //Duplicate X edge tiles
        TileMap = XW;
        //currTile = TileMap.GetTile(new Vector3Int(0,0,0));

        for (int i = 0; i < 2; i++)
        {
            int x = -Convert.ToInt32(w / 2);
            for (int y = -Convert.ToInt32(h / 2); y <= Convert.ToInt32(h / 2) - 1; y++)
            {
                currTile = TileMap.GetTile(new Vector3Int(x, y, 0));
                TileMap.SetTile(new Vector3Int(x + Convert.ToInt32(w), y, 0), currTile);
            }
            TileMap = TXW;
        }

        TileMap = XW;
        for (int i = 0; i < 2; i++)
        {
            int x = Convert.ToInt32(w / 2) - 1;
            for (int y = -Convert.ToInt32(h / 2); y <= Convert.ToInt32(h / 2) - 1; y++)
            {
                currTile = TileMap.GetTile(new Vector3Int(x, y, 0));
                TileMap.SetTile(new Vector3Int(x - Convert.ToInt32(w), y, 0), currTile);
            }
            TileMap = TXW;
        }

        //Duplicate X edge tiles on the top edge
        TileMap = XW;
        //currTile = TileMap.GetTile(new Vector3Int(0,0,0));

        for (int i = 0; i < 2; i++)
        {
            int y = Convert.ToInt32(h / 2) - 1;
            for (int x = -Convert.ToInt32(w / 2); x <= Convert.ToInt32(w / 2); x++)
            {
                currTile = TileMap.GetTile(new Vector3Int(x, y, 0));
                TileMap.SetTile(new Vector3Int(x, y - Convert.ToInt32(h), 0), currTile);
            }
            TileMap = TXW;
        }

        //Duplicate Y edge tiles
        TileMap = YW;
        //currTile = TileMap.GetTile(new Vector3Int(0,0,0));
        for (int i = 0; i < 2; i++)
        {
            int y = -Convert.ToInt32(h / 2);
            for (int x = -Convert.ToInt32(w / 2); x <= Convert.ToInt32(w / 2); x++)
            {
                currTile = TileMap.GetTile(new Vector3Int(x, y, 0));
                TileMap.SetTile(new Vector3Int(x, y + Convert.ToInt32(h), 0), currTile);
            }
            TileMap = TYW;
        }

        TileMap = YW;
        for (int i = 0; i < 2; i++)
        {
            int y = Convert.ToInt32(h / 2) - 1;
            for (int x = -Convert.ToInt32(w / 2); x <= Convert.ToInt32(w / 2); x++)
            {
                currTile = TileMap.GetTile(new Vector3Int(x, y, 0));
                TileMap.SetTile(new Vector3Int(x, y - Convert.ToInt32(h), 0), currTile);
            }
            TileMap = TYW;
        }

        //Duplicate Y edge tiles on the left edge
        TileMap = YW;
        //currTile = TileMap.GetTile(new Vector3Int(0,0,0));

        for (int i = 0; i < 2; i++)
        {
            int x = -Convert.ToInt32(w / 2);
            for (int y = -Convert.ToInt32(h / 2); y <= Convert.ToInt32(h / 2); y++)
            {
                currTile = TileMap.GetTile(new Vector3Int(x, y, 0));
                TileMap.SetTile(new Vector3Int(x + Convert.ToInt32(w), y, 0), currTile);
            }
            TileMap = TYW;
        }
    }
}
