using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialTextToBox : MonoBehaviour {
    public bool active = false;
    public string textToDisplay;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            Activate();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!active)
        {
            if (collider.gameObject.tag.Equals("MainCamera"))
            {
                Activate();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainCamera")
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
        GameObject textBox = GameObject.Find("TutorialText");
        textBox.GetComponent<Text>().text = textToDisplay;
        active = true;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
