using UnityEngine;
using System.Collections;

public class DisplayIfPrefIs : MonoBehaviour
{
    public GameObject object_to_display;
    public string bool_player_pref;
    public bool is_ = false;

	void Start ()
    {
        bool beat_level = System.Convert.ToBoolean(PlayerPrefs.GetInt(bool_player_pref, 0));

        if (beat_level == is_)
            object_to_display.SetActive(true);
    }
}
