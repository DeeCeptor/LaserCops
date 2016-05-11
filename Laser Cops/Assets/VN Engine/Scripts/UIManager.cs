using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager ui_manager;

	public Text dialogue_text_panel;
	public Text speaker_text_panel;
	public GameObject choice_panel;
	public Image background;	// Background image
	public Image foreground;    // Image appears in front of ALL ui elements
    public Text log_text;   // Log containing all dialogue spoken
    public GameObject entire_UI_panel;

    // Used by choice dialogue node
    public Button choice_1_button;
    public Button choice_2_button;
    public Button choice_3_button;
    public Button choice_4_button;

    public GameObject actor_positions;

    void Awake ()
    {
		ui_manager = this;
	}


    void Update ()
    {
	
	}
}
