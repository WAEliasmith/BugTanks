using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : bullet
{
    public float size = 2f;
    public float fadeEnd = 100f;
    private float scale;
    public float maxLife = 200f;
    public Vector2 screenSize;
    public MazeHandler maze;

    // Start is called before the first frame update
    void Start()
    {
        maze = GameObject.Find("MazeHandler").GetComponent<MazeHandler>();
        screenSize = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize;
        sr = gameObject.GetComponent<SpriteRenderer>();
        life = maxLife;
        piercing = true;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        life -= 1;

        if (life <= fadeStart)
        {
            sr.color = new Color(1f, 0.2f, 0f, (life / fadeStart));
            scale = Mathf.Max((life / fadeStart) * size, 0.1f);

        }

        if (life >= fadeEnd)
        {
            sr.color = new Color(1f, 0.2f, 0f, ((maxLife - fadeEnd) - (life - fadeEnd)) / (maxLife - fadeEnd));
            scale = Mathf.Max((((maxLife - fadeEnd) - (life - fadeEnd)) / (maxLife - fadeEnd)) * size, 0.1f);
        }
        transform.localScale = new Vector3(scale, scale, 1f);
        if (life <= 0)
        {
            Destroy(gameObject);
        }
        if (life == fadeEnd)
        {
            spreadBlocksInExplosion(12, 12, "air", true, true);
        }

    }

    void spreadBlocksInExplosion(int width, int height, string type, bool edge = false, bool replace = false)
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
                var position = new Vector2(center.x - (width / 2) + i, center.y - (height / 2) + j);
                if ((position - (Vector2)transform.position).magnitude < size / 2)
                {
                    maze.changeWall(position + new Vector2(0.5f, -0.5f), type, true, replace, true);
                }
                //remove y walls
                position = new Vector2(center.x - (width / 2) + i, center.y - (height / 2) + j);
                if ((position - (Vector2)transform.position).magnitude < size / 2)
                {
                    maze.changeWall(position + new Vector2(0f, 0f), type, true, replace, true);
                }
                // }
            }
        }
        maze.screenWrapProofWalls();
        maze.updateGraph();
    }
}
