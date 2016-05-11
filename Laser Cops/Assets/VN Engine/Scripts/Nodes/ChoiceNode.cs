using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;


// Displays the choices outlined. Does not continue to the next node.
// Each choice leads to a prescribed conversation.
public class ChoiceNode : Node 
{
	public string button_1_text;
	public Button.ButtonClickedEvent Button_1_Events;
	public string button_2_text;
	public Button.ButtonClickedEvent Button_2_Events;
	public string button_3_text;
	public Button.ButtonClickedEvent Button_3_Events;
	public string button_4_text;
	public Button.ButtonClickedEvent Button_4_Events;

	
	public override void Run_Node()
	{
		// Display the choices on the UI
		UIManager.ui_manager.choice_panel.SetActive(true);

		// Make buttons that have events visible, set their text,
		// add call to Finis_Node() on the OnClick() listener and hook up the choices buttons to the events on this node
		if (Button_1_Events.GetPersistentEventCount() > 0)
		{
			UIManager.ui_manager.choice_1_button.gameObject.SetActive(true);    // Make visible
            UIManager.ui_manager.choice_1_button.GetComponentInChildren<Text>().text = button_1_text;	// Set button text
			Button_1_Events.AddListener(Clear_Choices); // Add call to finish this node and hide UI to event listener
            UIManager.ui_manager.choice_1_button.onClick = Button_1_Events;	// Set events
		}
        else
        {
            UIManager.ui_manager.choice_1_button.gameObject.SetActive(false);
        }


        if (Button_2_Events.GetPersistentEventCount() > 0)
		{
            UIManager.ui_manager.choice_2_button.gameObject.SetActive(true);
            UIManager.ui_manager.choice_2_button.GetComponentInChildren<Text>().text = button_2_text;
			Button_2_Events.AddListener(Clear_Choices);
            UIManager.ui_manager.choice_2_button.onClick = Button_2_Events;
		}
        else
        {
            UIManager.ui_manager.choice_2_button.gameObject.SetActive(false);
        }


        if (Button_3_Events.GetPersistentEventCount() > 0)
		{
            UIManager.ui_manager.choice_3_button.gameObject.SetActive(true);
            UIManager.ui_manager.choice_3_button.GetComponentInChildren<Text>().text = button_3_text;
			Button_3_Events.AddListener(Clear_Choices);
            UIManager.ui_manager.choice_3_button.onClick = Button_3_Events;
		}
        else
        {
            UIManager.ui_manager.choice_3_button.gameObject.SetActive(false);
        }


        if (Button_4_Events.GetPersistentEventCount() > 0)
		{
            UIManager.ui_manager.choice_4_button.gameObject.SetActive(true);
            UIManager.ui_manager.choice_4_button.GetComponentInChildren<Text>().text = button_4_text;
			Button_4_Events.AddListener(Clear_Choices);
            UIManager.ui_manager.choice_4_button.onClick = Button_4_Events;
		}
        else
        {
            UIManager.ui_manager.choice_4_button.gameObject.SetActive(false);
        }
	}
	

	public override void Button_Pressed()
	{

	}


	public void Clear_Choices()
	{
		if (SceneManager.current_conversation.Get_Current_Node().GetType() != this.GetType())	// Don't clear the choices if the next node is a Choice node
        {
			// Remove event listeners from buttons
            UIManager.ui_manager.choice_1_button.onClick.RemoveAllListeners();
            UIManager.ui_manager.choice_2_button.onClick.RemoveAllListeners();
            UIManager.ui_manager.choice_3_button.onClick.RemoveAllListeners();
            UIManager.ui_manager.choice_4_button.onClick.RemoveAllListeners();

			// Set all choice buttons to inactive
			UIManager.ui_manager.choice_1_button.gameObject.SetActive(false);
			UIManager.ui_manager.choice_2_button.gameObject.SetActive(false);
			UIManager.ui_manager.choice_3_button.gameObject.SetActive(false);
			UIManager.ui_manager.choice_4_button.gameObject.SetActive(false);


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
