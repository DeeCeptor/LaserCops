using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class DrawLines : MonoBehaviour
{
    List<GameObject> levels = new List<GameObject>();
    public LineRenderer line_2;
    public GameObject Check_If_Active;
    public GameObject Add_If_Active;

    void Start ()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return 5;

        LineRenderer line = this.GetComponent<LineRenderer>();
        //game_mode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<Mode>();

        foreach (Transform child in transform)
        {
            levels.Add(child.gameObject);

            PlayableLevelNode n = child.GetComponent<PlayableLevelNode>();
            if (n.required_to_beat && !n.beat_level)
                break;
        }
        if (Check_If_Active != null && Check_If_Active.activeSelf && Add_If_Active != null)
            levels.Add(Add_If_Active);

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
}
