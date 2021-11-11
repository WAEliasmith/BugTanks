using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

public class CameraHolder : MonoBehaviour
{
    public Transform target;

    public bool follow = true;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private Tilemap XW;
    private Tilemap YW;
    private Tilemap TXW;
    private Tilemap TYW;

    public Vector2 screenSize;
    public Vector2 wrapOffset;

    public GameObject NW;
    public GameObject N;
    public GameObject NE;
    public GameObject W;
    public GameObject C;
    public GameObject E;
    public GameObject SW;
    public GameObject S;
    public GameObject SE;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;

    }

    void Start()
    {

        XW = GameObject.Find("XWalls").GetComponent<Tilemap>();
        YW = GameObject.Find("YWalls").GetComponent<Tilemap>();
        TXW = GameObject.Find("ThickXWalls").GetComponent<Tilemap>();
        TYW = GameObject.Find("ThickYWalls").GetComponent<Tilemap>();

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




        // NW.GetComponent<Camera>().enabled = false;
        // N.GetComponent<Camera>().enabled = false;
        // NE.GetComponent<Camera>().enabled = false;
        // W.GetComponent<Camera>().enabled = false;
        // E.GetComponent<Camera>().enabled = false;
        // SW.GetComponent<Camera>().enabled = false;
        // S.GetComponent<Camera>().enabled = false;
        // SE.GetComponent<Camera>().enabled = false;

        NW.transform.localPosition = new Vector3(-w, h, 0);
        N.transform.localPosition = new Vector3(0, h, 0);
        NE.transform.localPosition = new Vector3(w, h, 0);
        W.transform.localPosition = new Vector3(-w, 0, 0);
        C.transform.localPosition = new Vector3(0, 0, 0);
        E.transform.localPosition = new Vector3(w, 0, 0);
        SW.transform.localPosition = new Vector3(-w, -h, 0);
        S.transform.localPosition = new Vector3(0, -h, 0);
        SE.transform.localPosition = new Vector3(w, -h, 0);

        // Debug.DrawLine(NW.transform.localPosition / 2, NE.transform.localPosition / 2, Color.white, 999f);
        // Debug.DrawLine(NE.transform.localPosition / 2, SE.transform.localPosition / 2, Color.white, 999f);
        // Debug.DrawLine(SE.transform.localPosition / 2, SW.transform.localPosition / 2, Color.white, 999f);
        // Debug.DrawLine(SW.transform.localPosition / 2, NW.transform.localPosition / 2, Color.white, 999f);
    }
    // LateUpdate is called once per frame after update
    void FixedUpdate()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("main");
        }
        if (follow)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
            target.position + offset, ref velocity, smoothSpeed);
        }
    }
}
