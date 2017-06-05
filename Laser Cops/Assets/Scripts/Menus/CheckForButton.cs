using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckForButton : MonoBehaviour 
{
    public string Button_To_Check;
    public bool check_start_button = true;
    public bool check_A_button = true;
    public bool check_B_button = false;
    public bool destroy_after_press = false;

    public Button.ButtonClickedEvent Actions;


    void Update()
    {
        if (Input.GetButtonDown(Button_To_Check) 
            || (check_start_button && InputManager.ActiveDevice.CommandWasPressed)
            || (check_A_button && InputManager.ActiveDevice.Action1.WasPressed)
            || (check_B_button && InputManager.ActiveDevice.Action2.WasPressed))
        {
            Actions.Invoke();
            /*
            if (check_A_button && InputManager.ActiveDevice.Action1.WasPressed)
                Debug.Log("A Action1");
            if (Input.GetButtonDown(Button_To_Check))
                Debug.Log("Submit");
            if (check_B_button && InputManager.ActiveDevice.Action2.WasPressed)
                Debug.Log("B Action2");
            if (check_start_button && InputManager.ActiveDevice.CommandWasPressed)
                Debug.Log("Start");
            */
            if (destroy_after_press)
                Destroy(this.gameObject);
        }
    }
}
