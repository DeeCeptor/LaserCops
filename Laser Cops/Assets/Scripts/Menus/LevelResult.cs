using UnityEngine;
using System.Collections;

public class LevelResult : MonoBehaviour
{
    public bool coop_victory = false;
    public bool coop_defeat = true;
    public bool competitive = false;
    public bool competitive_blue_wins = false;
    public bool competitive_pink_wins = false;

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }
	void Start () 
	{
	
	}
}
