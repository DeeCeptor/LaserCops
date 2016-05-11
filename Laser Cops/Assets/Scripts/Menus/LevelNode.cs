using UnityEngine;
using System.Collections;

public class LevelNode : MonoBehaviour 
{
    public string level_name;
    public string level_to_load;

    public LevelNode next_node;
    public LevelNode previous_node;

    bool hovering = false;
    bool selected = false;
    public bool is_cutscene = false;

	void Start () 
	{
	
	}
	

    public void HoverOverLevel()
    {
        if (!hovering)
        {
            hovering = true;

            LevelManager.level_manager.HoverOverLevel(this);
        }
    }
    public void SelectLevel()
    {
        if (!selected)
        {
            selected = true;

            LevelManager.level_manager.SelectLevel(this);
        }
    }
    public virtual void LevelIsSelected()
    {

    }
    public void DeselectLevel()
    {
        selected = false;
        hovering = false;

        //LevelManager.level_manager.DeselectLevels();
    }


    // Button was clicked while hovering over level
    public void Button_Clicked()
    {
        if (hovering && !selected)
        {
            SelectLevel();
        }
    }
}
