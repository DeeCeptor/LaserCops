using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectResumeOnEnable : MonoBehaviour 
{
    void OnEnable()
    {
        //this.GetComponent<Button>().Select();
        StartCoroutine(SelectContinueButtonLater());
    }

    IEnumerator SelectContinueButtonLater()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.Log("Reselecting first input");
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }
}
