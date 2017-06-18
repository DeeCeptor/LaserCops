using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConnectedControllers : MonoBehaviour 
{
    bool detecting_controllers = true;
    public static List<InputDevice> devices = new List<InputDevice>();

    public AudioSource Select;


    void Start () 
	{
        //DontDestroyOnLoad(this);
        ClearPlayerControls();
        ControlsManager.ClearControls();
    }
	

	void Update () 
	{
        DetectControllers();
    }


    public void DetectControllers()
    {
        /*
        if (checking_for_next_level
            && (Input.GetButtonDown("Submit") || InputManager.ActiveDevice.Action1.WasPressed))
        {
            checking_for_next_level = false;
            ToSceneSelectActions.Invoke();
        }
        */
        // Check for left stick & right stick and any button
        if (Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) > 0.5f || Mathf.Abs(InputManager.ActiveDevice.LeftStickY.Value) > 0.5f
                || Mathf.Abs(InputManager.ActiveDevice.RightStickX.Value) > 0.5f || Mathf.Abs(InputManager.ActiveDevice.RightStickY.Value) > 0.5f
                || InputManager.ActiveDevice.CommandWasPressed || InputManager.ActiveDevice.Action3.WasPressed || InputManager.ActiveDevice.Action4.WasPressed
                && !devices.Contains(InputManager.ActiveDevice))
            AddPlayerController(InputManager.ActiveDevice);
    }
    public void AddPlayerController(InputDevice device)
    {
        if (!devices.Contains(device))
        {
            devices.Add(device);
            Select.Play();
        }
    }
    public void ClearPlayerControls()
    {
        devices.Clear();
    }



    public void GoToSceneSelect()
    {
        PlayerJoin.player_join.SetControls();
        Debug.Log("A");
    }
}


// Used globally throughout the game
public static class ControlsManager
{
    // Queried by Player controllers to get controls
    public static Dictionary<int, PlayerControlsProfile> Player_Controls = new Dictionary<int, PlayerControlsProfile>();


    // Can return null
    public static PlayerControlsProfile GetControls(int player_num)
    {
        if (Player_Controls.ContainsKey(player_num))
            return Player_Controls[player_num];

        // No controls found
        return null;
    }


    public static int GetNumberUniqueControllers()
    {
        List<InputDevice> unique_controllers = new List<InputDevice>();

        foreach (PlayerControlsProfile i in Player_Controls.Values)
        {
            if (i.device != null && !unique_controllers.Contains(i.device))
                unique_controllers.Add(i.device);
        }
        return unique_controllers.Count;
    }


    public static void AddControls(PlayerControlsProfile profile)
    {
        Player_Controls[profile.player_number] = profile;
    }


    public static void ClearControls()
    {
        Player_Controls.Clear();
    }


    public static void Print_Out_Current_Controls()
    {
        foreach (PlayerControlsProfile p in Player_Controls.Values)
        {
            Debug.Log(p.player_number + ": " + p.left_or_right_side);
        }
    }
}


// Each player gets one of these
public class PlayerControlsProfile
{
    public int player_number = 0;
    public InputDevice device;
    public bool left_or_right_side = true;
    public bool uses_keyboard = false;
    public bool uses_wads = false;  // Left or right side of keyboard

    public PlayerControlsProfile(int player_num, InputDevice in_device, bool left_controller, bool keyboard, bool wads)
    {
        player_number = player_num;
        device = in_device;
        left_or_right_side = left_controller;
        uses_keyboard = keyboard;
        uses_wads = wads;
    }
}