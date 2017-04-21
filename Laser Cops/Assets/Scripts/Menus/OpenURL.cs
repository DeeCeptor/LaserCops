using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour 
{
    string webpage = "https://goo.gl/forms/d5l9LYdSHljYV9423";


    public void OpenSurveyWebpage()
    {
        Application.OpenURL(webpage);
    }
}
