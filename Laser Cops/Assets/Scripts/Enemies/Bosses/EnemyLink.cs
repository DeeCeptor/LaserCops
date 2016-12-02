﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class EnemyLink : MonoBehaviour
{
    public GameObject above;
    public GameObject below;

    public int position_from_top_in_rope;    // Where 0 is the top
    public int position_from_bottom_in_rope;    // Where 0 is the botom
    public GameObject top_most;     // Top of the rope
    public GameObject bottom_most;  // Bottom of the rope

    public List<GameObject> all_segments;

    public EnemyBossTetherScript rope;
}