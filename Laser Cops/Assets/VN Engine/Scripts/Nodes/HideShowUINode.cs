using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Used to either hide or show the UI panel at the bottom of the screen
public class HideShowUINode : Node
{
    public bool Make_UI_Visible = false;

    public override void Run_Node()
    {
        UIManager.ui_manager.entire_UI_panel.SetActive(Make_UI_Visible);

        Finish_Node();
    }


    public override void Button_Pressed()
    {
        
    }


    public override void Finish_Node()
    {
        StopAllCoroutines();

        base.Finish_Node();
    }
}
