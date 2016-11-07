using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


// Should we use stats to decide what this choice button does?
public enum Choice_Stat_Requirement { No_Requirement, Float_Stat_Requirement, Bool_Stat_Requirement, Object_Is_Null };

// If the stat requirement is not met, should we hide the button, or just disable it?
public enum Requirement_Not_Met_Action { Hide_Choice, Disable_Button };

public enum Float_Stat_Comparator {  Greater_than, Less_than };


// Displays the choices outlined. Does not continue to the next node.
// Each choice leads to a prescribed conversation.
public class ChoiceNode : Node 
{
    // DO NOT CHANGE: is the maximum number of choices. Dictated by the number of ChoiceButtons listed in the UIManager. You shouldn't ever need more than 20 buttons.
    public static int max_number_of_buttons = 20;

    [HideInInspector]
    public string Name_Of_Choice;   // Text that appears at the top of the choices menu. Ex: I'm at a crossroads. Which way should I go?
    [HideInInspector]
    public int Number_Of_Choices = 6;

    // Arrays of values used for the buttons. Makes the code able to loop through requirements
    [HideInInspector]
    public string[] Button_Text = new string[max_number_of_buttons];
    [HideInInspector]
    public Choice_Stat_Requirement[] Requirement_Type = new Choice_Stat_Requirement[max_number_of_buttons];
    [HideInInspector]
    public Requirement_Not_Met_Action[] Requirement_Not_Met_Actions = new Requirement_Not_Met_Action[max_number_of_buttons];
    [HideInInspector]
    public string[] Disabled_Text = new string[max_number_of_buttons];
    [HideInInspector]
    public Float_Stat_Comparator[] Float_Stat_Is = new Float_Stat_Comparator[max_number_of_buttons];
    [HideInInspector]
    public string[] Stat_Name = new string[max_number_of_buttons];
    [HideInInspector]
    public float[] Float_Compare_Value = new float[max_number_of_buttons];
    [HideInInspector]
    public bool[] Bool_Compare_Value = new bool[max_number_of_buttons];
    [HideInInspector]
    public GameObject[] Check_Null_Object = new GameObject[max_number_of_buttons];

    // The events associated with the buttons
    public Button.ButtonClickedEvent[] Button_Events = new Button.ButtonClickedEvent[max_number_of_buttons];


    public override void Run_Node()
	{
        StartCoroutine(Running());
	}
	public IEnumerator Running()
    {
        // Wait a frame so we can evaluate if objects we check for requirements have been destroyed
        yield return 0;

        // Display the choices on the UI
        UIManager.ui_manager.choice_panel.SetActive(true);
        UIManager.ui_manager.choice_text_banner.text = Name_Of_Choice;

        // Loop through each button
        // Make buttons that have events visible, set their text,
        // add call to Finish_Node() on the OnClick() listener and hook up the choices buttons to the events on this node
        for (int x = 0; x < Number_Of_Choices; x++)
        {
            if (Button_Events[x].GetPersistentEventCount() > 0)
            {
                UIManager.ui_manager.choice_buttons[x].gameObject.SetActive(true);    // Make visible
                UIManager.ui_manager.choice_buttons[x].interactable = true;
                bool requirement_met = true;
                UIManager.ui_manager.choice_buttons[x].GetComponentInChildren<Text>().text = Button_Text[x];   // Set button text

                // Check stat requirements
                switch (Requirement_Type[x])
                {
                    case Choice_Stat_Requirement.No_Requirement:
                        break;


                    case Choice_Stat_Requirement.Float_Stat_Requirement:
                        requirement_met = StatsManager.Compare_Float_Stat(Stat_Name[x], Float_Stat_Is[x], Float_Compare_Value[x]);
                        break;


                    case Choice_Stat_Requirement.Bool_Stat_Requirement:
                        requirement_met = StatsManager.Compare_Bool_Stat_To(Stat_Name[x], Bool_Compare_Value[x]);
                        break;


                    case Choice_Stat_Requirement.Object_Is_Null:
                        // Check if object exists
                        // If the object doesn't exist, and the box is checked
                        // OR  the object exists and the box is not checked
                        requirement_met = Check_Null_Object[x] && !Bool_Compare_Value[x];
                        break;
                }

                // Stat requirements have been met. Display the choice
                if (requirement_met)
                {
                    Button_Events[x].AddListener(Clear_Choices); // Add call to finish this node and hide UI to event listener
                    UIManager.ui_manager.choice_buttons[x].onClick = Button_Events[x]; // Set events
                }
                else
                {
                    switch (Requirement_Not_Met_Actions[x])
                    {
                        case Requirement_Not_Met_Action.Disable_Button:
                            UIManager.ui_manager.choice_buttons[x].interactable = false;
                            UIManager.ui_manager.choice_buttons[x].GetComponentInChildren<Text>().text = Disabled_Text[x];   // Set button text
                            break;
                        case Requirement_Not_Met_Action.Hide_Choice:
                            UIManager.ui_manager.choice_buttons[x].gameObject.SetActive(false);    // Make inivisible
                            break;
                    }
                }
            }
            else
            {
                UIManager.ui_manager.choice_buttons[x].gameObject.SetActive(false);
            }
        }

        // Disable all other buttons
        for (int x = Number_Of_Choices; x < max_number_of_buttons; x++)
        {
            UIManager.ui_manager.choice_buttons[x].gameObject.SetActive(false);
        }
    }

	public override void Button_Pressed()
	{

	}


	public void Clear_Choices()
	{
		if (VNSceneManager.current_conversation.Get_Current_Node().GetType() != this.GetType())	// Don't clear the choices if the next node is a Choice node
        {
            // Loop through every button
            for (int x = 0; x < ChoiceNode.max_number_of_buttons; x++)
            {
                // Remove event listeners from buttons
                UIManager.ui_manager.choice_buttons[x].onClick.RemoveAllListeners();
                // Set all choice buttons to inactive
                UIManager.ui_manager.choice_buttons[x].gameObject.SetActive(false);
            }

			// Hide choice UI
			UIManager.ui_manager.choice_panel.SetActive(false);
		}
	}


	public override void Finish_Node()
	{
		Clear_Choices();		// Hide the UI
		base.Finish_Node();		// Continue conversation
	}
}