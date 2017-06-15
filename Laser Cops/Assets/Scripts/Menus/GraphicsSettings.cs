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
    public Dropdown graphics_quality_dropdown;

    Resolution[] resolutions;

    bool ignore_first_resolution_change = true;
    bool ignore_first_quality_change = true;


    public void Resume()
    {
        GameState.game_state.Unpause();
    }


    void Awake () 
	{
        // Set windowed toggle
        windowed_toggle.isOn = !Screen.fullScreen;

        EvaluatePossibleResolutions();
        EvaluatePossibleQualityLevels();
	}
    public void EvaluatePossibleResolutions()
    {
        resolution_dropdown.ClearOptions();

        // Set possible resolution dropdown
        resolutions = Screen.resolutions;

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
        foreach (string r in list_of_res)
        {
            string text = r;
            string[] split_text = text.Split('x');
            int width = Int32.Parse(split_text[0]);
            int height = Int32.Parse(split_text[1]);

            if (width == Screen.width
                && height == Screen.height)
            {
                resolution_dropdown.captionText.text = width + "x" + height;
                break;
            }
            cur_resolution_index++;
        }

        resolution_dropdown.value = cur_resolution_index;
    }
    public void ResolutionChanged(int resolution_index)
    {
        if (ignore_first_resolution_change)
        {
            ignore_first_resolution_change = false;
            return;
        }

        string text = resolution_dropdown.options[resolution_index].text;
        string[] split_text = text.Split('x');
        int width = Int32.Parse(split_text[0]);
        int height = Int32.Parse(split_text[1]);
        Screen.SetResolution(width, height, !windowed_toggle.isOn);
        Debug.Log("Resolution index: " + resolution_index + " Resolution changed to " + width + "x" + height +". Current resolution: " + Screen.currentResolution);
    }


    public void EvaluatePossibleQualityLevels()
    {
        graphics_quality_dropdown.ClearOptions();
        graphics_quality_dropdown.AddOptions(QualitySettings.names.ToList<string>());
        graphics_quality_dropdown.value = QualitySettings.GetQualityLevel();
        QualitySettings.GetQualityLevel();
    }
    public void QualityLevelChanged(int quality_index)
    {
        if (ignore_first_quality_change)
        {
            ignore_first_quality_change = false;
            return;
        }

        QualitySettings.SetQualityLevel(quality_index, true);
        Debug.Log("Graphics quality changed: " + quality_index);
    }


    public void WindowedToggleChanged(bool enabled)
    {
        Debug.Log("Windowed changed " + enabled);
        Screen.fullScreen = enabled;
        Screen.SetResolution(Screen.width, Screen.height, !enabled);
    }


    void OnEnable()
    {
        /*
        EvaluatePossibleResolutions();
        EvaluatePossibleQualityLevels();*/
    }
    void OnDisable()
    {
        resolution_dropdown.Hide();
        graphics_quality_dropdown.Hide();
    }
}
