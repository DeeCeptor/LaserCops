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

    public Button.ButtonClickedEvent Actions;


    void Update()
    {
        if (Input.GetButtonDown(Button_To_Check) 
            || (check_start_button && InputManager.ActiveDevice.CommandWasPressed)
            || (check_A_button && InputManager.ActiveDevice.Action1.WasPressed)
            || (check_A_button && InputManager.ActiveDevice.Action2.WasPressed))
        {
            Actions.Invoke();
        }
    }
}
