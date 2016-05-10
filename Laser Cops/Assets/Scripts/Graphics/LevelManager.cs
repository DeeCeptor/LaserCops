using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{
    List<GameObject> levels = new List<GameObject>();
    LineRenderer line;

    void Start()
    {
        line = this.GetComponent<LineRenderer>();

        foreach (Transform child in transform)
            levels.Add(child.gameObject);

        line.SetVertexCount(levels.Count);
        for (int x = 0; x < levels.Count; x++)
        {
            line.SetPosition(x, levels[x].transform.position);
        }
	}
	

	void Update () 
	{
	
	}
}
