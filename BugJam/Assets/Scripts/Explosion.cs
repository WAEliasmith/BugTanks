using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : bullet
{
    public bullet carrier;
    public float size = 2f;
    public float fadeEnd = 100f;
    protected float scale;
    public float maxLife = 200f;
    public Vector2 screenSize;
    public MazeHandler maze;

    // Start is called before the first frame update
    void Start()
    {
        ownerScoreNumber = carrier.ownerScoreNumber;
        maze = GameObject.Find("MazeHandler").GetComponent<MazeHandler>();
        screenSize = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize;
        sr = gameObject.GetComponent<SpriteRenderer>();
        lifeLeft = maxLife;
        piercing = true;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        lifeLeft -= 1;
        sr.color = color;
        if (lifeLeft <= fadeStart)
        {
            color.a = (lifeLeft / fadeStart);
            scale = Mathf.Max((lifeLeft / fadeStart) * size, 0.1f);

        }

        if (lifeLeft >= fadeEnd)
        {
            color.a = ((maxLife - fadeEnd) - (lifeLeft - fadeEnd)) / (maxLife - fadeEnd);
            scale = Mathf.Max((((maxLife - fadeEnd) - (lifeLeft - fadeEnd)) / (maxLife - fadeEnd)) * size, 0.1f);
        }
        transform.localScale = new Vector3(scale, scale, 1f);
        if (lifeLeft <= 0)
        {
            maze.updateGraph();
            Destroy(gameObject);
        }
        if (lifeLeft == fadeEnd)
        {
            spreadBlocksInExplosion((int)Mathf.Round(size + 1), (int)Mathf.Round(size + 1f), "air", true);
        }
    }

    protected void spreadBlocksInExplosion(int width, int height, string type, bool replace = false)
    {
        Vector2Int center = Vector2Int.RoundToInt((Vector2)transform.position);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // if (edge || (-(width / 2) + i + center.x < screenSize.x / 2 && -(height / 2) + j + center.y < screenSize.y / 2
                // && -(width / 2) + i + center.x > -screenSize.x / 2 && -(height / 2) + j + center.y > -screenSize.y / 2))
                // {
                //remove x walls
                var position = new Vector2(center.x - (width / 2) + i, center.y - (height / 2) + j) + new Vector2(0.5f, -0.5f);
                if ((position - (Vector2)transform.position).magnitude < (size / 2 + 0.1f))
                {
                    maze.changeWall(position, type, true, replace, true);
                }
                //remove y walls
                position = new Vector2(center.x - (width / 2) + i, center.y - (height / 2) + j);
                if ((position - (Vector2)transform.position).magnitude < (size / 2 + 0.1f))
                {
                    maze.changeWall(position, type, true, replace, true);
                }
                // }
            }
        }
        maze.screenWrapProofWalls();
    }
}
