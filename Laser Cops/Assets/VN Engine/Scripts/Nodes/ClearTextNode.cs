using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClearTextNode : Node
{
    public override void Run_Node()
    {
        UIManager.ui_manager.dialogue_text_panel.text = "";
        UIManager.ui_manager.speaker_text_panel.text = "";
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
