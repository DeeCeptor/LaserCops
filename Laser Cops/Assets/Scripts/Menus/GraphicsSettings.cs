using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GraphicsSettings : MonoBehaviour 
{
    public Toggle windowed_toggle;
    public Dropdown resolution_dropdown;

    Resolution[] resolutions;

	void Awake () 
	{
        // Set windowed toggle
        windowed_toggle.isOn = !Screen.fullScreen;

        EvaluatePossibleResolutions();
	}
    public void EvaluatePossibleResolutions()
    {
        resolution_dropdown.ClearOptions();

        // Set possible resolution dropdown
        resolutions = Screen.resolutions;

        //resolution_dropdown.te
        // Screen.currentResolution
        // Create a list of resolution width and height
        List<string> list_of_res = new List<string>();
        foreach (Resolution r in resolutions)
        {
            list_of_res.Add(r.width + "x" + r.height);
        }
        // Remove duplicates
        list_of_res = list_of_res.Distinct().ToList();

        resolution_dropdown.AddOptions(list_of_res);

        // Set whatever our current resolution is to be the current value
        int cur_resolution_index = 0;
        foreach (Resolution r in resolutions)
        {
            if (r.width == Screen.currentResolution.width
                && r.height == Screen.currentResolution.height)
                break;
            cur_resolution_index++;
        }

        resolution_dropdown.value = cur_resolution_index;
    }


    public void ResolutionChanged(int resolution_index)
    {
        string text = resolution_dropdown.options[resolution_dropdown.value].text;
        string[] split_text = text.Split('x');
        int width = Int32.Parse(split_text[0]);
        int height = Int32.Parse(split_text[1]);
        Screen.SetResolution(width, height, !windowed_toggle.isOn);
    }


    public void WindowedToggleChanged(bool enabled)
    {
        Screen.fullScreen = enabled;
        Screen.SetResolution(Screen.width, Screen.height, !enabled);
    }


    public void ResolutionChanged()
    {

    }
}
