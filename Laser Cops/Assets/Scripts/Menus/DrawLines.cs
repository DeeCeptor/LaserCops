using UnityEngine;
using System.Collections.Generic;
using System;

public class DrawLines : MonoBehaviour
{
    List<GameObject> levels = new List<GameObject>();
    public LineRenderer line_2;

    void Awake ()
    {
        LineRenderer line = this.GetComponent<LineRenderer>();
        //game_mode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<Mode>();

        foreach (Transform child in transform)
        {
            levels.Add(child.gameObject);
        }

        line.SetVertexCount((levels.Count * 2) - 1);
        line_2.SetVertexCount((levels.Count * 2) - 1);
        Vector3[] positions = new Vector3[levels.Count * 2 - 1];

        for (int x = 0; x < (levels.Count * 2) - 1; x++)
        {
            if (x % 2 == 0)     // Even number
            {
                positions[x] = new Vector3(levels[x / 2].transform.position.x, levels[x / 2].transform.position.y, 0.01f);
            }
            else if ((x / 2) + 1 < levels.Count)        // Odd number
            {
                positions[x] = new Vector3((levels[x / 2].transform.position.x + levels[(x / 2) + 1].transform.position.x) / 2,
                    (levels[x / 2].transform.position.y + levels[(x / 2) + 1].transform.position.y) / 2,
                    0.01f);
            }
        }
        line.SetPositions(positions);
        Array.Reverse(positions);
        line_2.SetPositions(positions);
    }



    void Start ()
    {
	
	}
}
