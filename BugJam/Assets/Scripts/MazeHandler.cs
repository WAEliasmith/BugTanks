using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Pathfinding;

[Flags]
public enum WallState
{
    // 0000 -> NO WALLS
    // 1111 -> All walls
    LEFT = 1,
    RIGHT = 2,
    UP = 4,
    DOWN = 8,
    VISITED = 128,
}

public struct Neighbour
{
    public Vector2Int Position;
    public WallState SharedWall;
}

public class MazeHandler : MonoBehaviour
{
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }
    private static WallState[,] ApplyrecursiveBacktracker(WallState[,] maze, int width, int height, System.Random rng)
    {
        var positionStack = new Stack<Vector2Int>();
        Vector2Int position = new Vector2Int(rng.Next(0, width), rng.Next(0, height));

        maze[position.x, position.y] |= WallState.VISITED;
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.x, current.y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.x, nPosition.y] &= ~GetOppositeWall(randomNeighbour.SharedWall);

                maze[nPosition.x, nPosition.y] |= WallState.VISITED;

                positionStack.Push(nPosition);
            }
        }

        return maze;
    }
    private static List<Neighbour> GetUnvisitedNeighbours(Vector2Int p, WallState[,] maze, int width, int height)
    {
        var list = new List<Neighbour>();

        if (p.x > 0)
        {
            if (!maze[p.x - 1, p.y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Vector2Int(p.x - 1, p.y),
                    SharedWall = WallState.LEFT
                });
            }
        }
        if (p.y > 0)
        {
            if (!maze[p.x, p.y - 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Vector2Int(p.x, p.y - 1),
                    SharedWall = WallState.DOWN
                });
            }
        }
        if (p.y < height - 1)
        {
            if (!maze[p.x, p.y + 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Vector2Int(p.x, p.y + 1),
                    SharedWall = WallState.UP
                });
            }
        }
        if (p.x < width - 1)
        {
            if (!maze[p.x + 1, p.y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Vector2Int(p.x + 1, p.y),
                    SharedWall = WallState.RIGHT
                });
            }
        }

        return list;
    }

    public static WallState[,] Generate(int width, int height, System.Random rng)
    {
        WallState[,] maze = new WallState[width, height];

        WallState initial = WallState.LEFT | WallState.RIGHT | WallState.UP | WallState.DOWN; // All Walls
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = initial;
            }
        }

        return ApplyrecursiveBacktracker(maze, width, height, rng);
    }

    public float holeMod = 0.2f;
    public float thickMod = 0.2f;
    public bool holesInEdge = false;
    public bool generateMaze = false;


    public TileBase XWT;
    public TileBase YWT;
    public TileBase TXWT;
    public TileBase TYWT;

    private Tilemap XW;
    private Tilemap YW;
    private Tilemap TXW;
    private Tilemap TYW;
    private Vector2Int screenSize;

    public GameObject LazerWall = null;

    // Awake is called before any start
    void Awake()
    {
        screenSize = Vector2Int.RoundToInt(GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize);
        Vector2 wrapOffset = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().wrapOffset;

        XW = GameObject.Find("XWalls").GetComponent<Tilemap>();
        YW = GameObject.Find("YWalls").GetComponent<Tilemap>();
        TXW = GameObject.Find("ThickXWalls").GetComponent<Tilemap>();
        TYW = GameObject.Find("ThickYWalls").GetComponent<Tilemap>();

        //generate lazer wrapper -x
        GameObject wall = Instantiate(LazerWall, new Vector3(-screenSize.x * 0.5f + wrapOffset.x - 0.51f, 0f, 0f), Quaternion.identity);
        wall.transform.localScale = new Vector3(1f, screenSize.y + 2, 1f);
        wall.transform.parent = gameObject.transform;
        //generate lazer wrapper x
        wall = Instantiate(LazerWall, new Vector3(screenSize.x * 0.5f + wrapOffset.x + 0.51f, 0f, 0f), Quaternion.identity);
        wall.transform.localScale = new Vector3(1f, screenSize.y + 2, 1f);
        wall.transform.parent = gameObject.transform;
        //generate lazer wrapper -y
        wall = Instantiate(LazerWall, new Vector3(0f, -screenSize.y * 0.5f + wrapOffset.y - 0.51f, 0f), Quaternion.identity);
        wall.transform.localScale = new Vector3(screenSize.x + 2, 1f, 1f);
        wall.transform.parent = gameObject.transform;
        //generate lazer wrapper y
        wall = Instantiate(LazerWall, new Vector3(0f, screenSize.y * 0.5f + wrapOffset.y + 0.51f, 0f), Quaternion.identity);
        wall.transform.localScale = new Vector3(screenSize.x + 2, 1f, 1f);
        wall.transform.parent = gameObject.transform;

        if (generateMaze)
        {
            var rng = new System.Random(/*seed*/);

            var edge = 1;
            if (holesInEdge)
            {
                edge = 0;
            }
            var maze = Generate(screenSize.x, screenSize.y, rng);
            DrawMaze(maze, screenSize.x, screenSize.y);
            spreadBlocksInMaze(screenSize.x, screenSize.y, rng, holeMod, "air", edge);
            spreadBlocksInMaze(screenSize.x, screenSize.y, rng, thickMod, "TXW", 0, true);
            spreadBlocksInMaze(screenSize.x, screenSize.y, rng, thickMod, "TYW", 0, true);
        }
        screenWrapProofWalls();
        StartCoroutine(updateGraphDelayed());
    }

    IEnumerator updateGraphDelayed()
    {
        yield return new WaitForSeconds(0.01f);
        updateGraph();
    }


    void spreadBlocksInMaze(int width, int height, System.Random rng, float mod, string type, int edge = 0, bool replace = false)
    {
        for (int i = edge; i < width - edge; i++)
        {
            for (int j = edge; j < height - edge; j++)
            {
                //remove x walls
                if (rng.Next(0, 100) < mod * 100)
                {
                    var position = new Vector2(-width / 2 + i, -height / 2 + j);
                    changeWall(position + new Vector2(0.5f, -0.5f), type, true, replace);
                }
                //remove y walls
                if (rng.Next(0, 100) < mod * 100)
                {
                    var position = new Vector2(-width / 2 + i, -height / 2 + j);
                    changeWall(position + new Vector2(0f, 0f), type, true, replace);
                }
            }
        }
    }

    void DrawMaze(WallState[,] maze, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var cell = maze[i, j];
                var position = new Vector2(-width / 2 + i, -height / 2 + j);

                if (cell.HasFlag(WallState.UP))
                {
                    // Debug.DrawRay(position + new Vector2(-0.5f + 0.5f, 0.5f)
                    //     , Vector2.right, Color.red, 50f);
                    changeWall(position + new Vector2(0, 0.5f), "XW", true);
                }
                if (cell.HasFlag(WallState.LEFT))
                {
                    // Debug.DrawRay(position + new Vector2(-0.5f + 0.5f, -0.5f)
                    //     , Vector2.up, Color.green, 50f);
                    changeWall(position + new Vector2(0, 0), "YW", true);
                }
                // if (cell.HasFlag(WallState.RIGHT))
                // {
                //     changeWall(position + new Vector2(0.5f, 0), "YW", true);
                // }
                // if (cell.HasFlag(WallState.DOWN))
                // {
                //     changeWall(position + new Vector2(0, -0.5f), "XW", true);
                // }
            }
        }
    }

    public void changeWall(Vector3 position3d, string wallType = "air", bool batch = false, bool replace = false, bool weakReplace = false)
    {
        bool validX = false;
        bool validY = false;
        float ything = Mathf.Abs((position3d.x) % 1);
        if (ything < 0.3f || ything > 0.7f)
        {
            validY = true;
        }
        float xthing = Mathf.Abs((position3d.y + 0.5f) % 1);
        if (xthing < 0.3f || xthing > 0.7f)
        {
            validX = true;
        }

        Vector3Int xposition = Vector3Int.FloorToInt(position3d);
        Vector3Int yposition = Vector3Int.FloorToInt(position3d + new Vector3(0.5f, 0.5f, 0));

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
            yposition = new Vector3Int((yposition.x - screenSize.x), yposition.y, 0);
        }

        if (xposition.y <= (-screenSize.y / 2) - 1)
        {
            xposition = new Vector3Int(xposition.x, xposition.y + screenSize.y, 0);
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


        if ((wallType == "XW" || (wallType == "air")) && validX)
        {
            if (replace == false || (XW.GetTile(xposition) != null || (weakReplace == false && TXW.GetTile(xposition) != null)))
            {
                XW.SetTile(xposition, tile);
            }
        }
        if ((wallType == "YW" || (wallType == "air")) && validY)
        {
            if (replace == false || (YW.GetTile(yposition) != null || (weakReplace == false && TYW.GetTile(yposition) != null)))
            {
                YW.SetTile(yposition, tile);
            }
        }
        if ((wallType == "TXW" || (wallType == "air")) && validX)
        {
            if (replace == false || (XW.GetTile(xposition) != null || (weakReplace == false && TXW.GetTile(xposition) != null)))
            {
                XW.SetTile(xposition, null);
                TXW.SetTile(xposition, tile);
            }
        }
        if ((wallType == "TYW" || (wallType == "air")) && validY)
        {
            if (replace == false || (YW.GetTile(yposition) != null || (weakReplace == false && TYW.GetTile(yposition) != null)))
            {
                YW.SetTile(yposition, null);
                TYW.SetTile(yposition, tile);
            }
        }
        if (batch == false && (validY || validX))
        {
            screenWrapProofWalls();
            updateGraph();
        }
    }

    public void updateGraph()
    {
        GameObject XWO = GameObject.Find("XWalls");
        GameObject YWO = GameObject.Find("YWalls");
        GameObject TXWO = GameObject.Find("ThickXWalls");
        GameObject TYWO = GameObject.Find("ThickYWalls");
        Bounds b = new Bounds(new Vector2(0, 0), new Vector3(screenSize.x, screenSize.y, 0));
        AstarPath.active.UpdateGraphs(b);
    }

    public void screenWrapProofWalls()
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
