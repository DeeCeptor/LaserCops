using UnityEngine;
using System.Collections.Generic;

public class InputSettings : MonoBehaviour
{
    public static InputSettings input_settings;

    public List<List<string>> inputs;

    void Awake ()
    {
        if (InputSettings.input_settings == null)
        {
            InputSettings.input_settings = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
}
