using UnityEngine;
using System.Collections.Generic;

public class ScrollingBackgroundManager : MonoBehaviour
{
    // Objects that repeatedly scrolling scrolling
    public List<GameObject> Background_Objects;
    public float scrolling_speed = -15f;
    public float back_edge = -40;

    void Start ()
    {
	    
	}


    public Vector3 PlaceAtBack(GameObject obj)
    {
        // Place obj at the back of the queue
        Background_Objects.Remove(obj);

        // Calculate where to place this object at the back
        Vector3 pos = Background_Objects[0].transform.position;
        foreach(GameObject element in Background_Objects)
        {
            pos.x += GetDimensionInPX(element).x;
        }
        obj.transform.position = pos;
        Background_Objects.Add(obj);

        return pos;
    }


    private Vector2 GetDimensionInPX(GameObject obj)
    {
        Vector2 tmpDimension;

        tmpDimension.x = obj.transform.localScale.x * obj.GetComponent<SpriteRenderer>().sprite.bounds.size.x;  // this is gonna be our width
        tmpDimension.y = obj.transform.localScale.y * obj.GetComponent<SpriteRenderer>().sprite.bounds.size.y;  // this is gonna be our height

        return tmpDimension;
    }


    void Update()
    {
        // Move each element
        foreach (GameObject element in Background_Objects)
        {
            element.transform.position += new Vector3(scrolling_speed * Time.deltaTime, 0, 0);


        }

        for(int x = 0; x < Background_Objects.Count; x++)
        {
            if (Background_Objects[x].transform.position.x <= back_edge)
                PlaceAtBack(Background_Objects[x]);
        }
    }
}
