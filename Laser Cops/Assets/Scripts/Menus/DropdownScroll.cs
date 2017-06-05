using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public class DropdownScroll : MonoBehaviour 
{
    ScrollRect scroll;
    Dropdown dropdown;


    void Start()
    {
        scroll = this.GetComponent<ScrollRect>();
        dropdown = this.GetComponentInParent<Dropdown>();
    }


	void Update () 
	{
        float val = 1 - (float)dropdown.value / (float)dropdown.options.Count;
        scroll.verticalNormalizedPosition = val;
        Debug.Log(val);
    }
}
