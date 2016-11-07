using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

// Actors positied on either the left or right hand side of the screen 
// Maximum 6 actors, 3 per side, using a 'stacked card' animations
public enum Actor_Positions { LEFT, RIGHT };

public class ActorManager : MonoBehaviour
{
    public static ActorManager actor_manager;

    public static List<Actor> actors_on_scene = new List<Actor>();		// List of all instantiated actors
    public static List<Actor> exiting_actors = new List<Actor>();   // Used so we don't switch conversations before actors have exited

    [HideInInspector]
    public static List<Actor> left_actors = new List<Actor>();     // Can have multiple actors on the left side. 0 is leftmost, 1 is next, 2 is next, etc.
                                                                   // Looks like: 0 1 2
    [HideInInspector]
    public static List<Actor> right_actors = new List<Actor>();     // 0 is rightmost, 1 is next
                                                                    // Looks like: 2 1 0
    static float x_actor_offset = 130;  // Distance between each actor on the left or right

    // Used for sliding actors in
    public RectTransform offscreen_left;
    public static RectTransform left;
    public RectTransform left_;
    public RectTransform offscreen_right;
    public static RectTransform right;
    public RectTransform right_;

    void Awake()
    {
        actor_manager = this;
        left = left_;
        right = right_;

        actors_on_scene = new List<Actor>();
        exiting_actors = new List<Actor>();
        left_actors = new List<Actor>();
        right_actors = new List<Actor>();
    }
    void Start()
    {

    }


    public static bool Is_Actor_On_Scene(string actor_name)
    {
        // Search for our actor on the scene
        Actor item = actors_on_scene.Find(obj => obj.actor_name == actor_name);

        return (item != null);
    }


    // Finds the actor if it on the scene.
    // Returns null if the actor is not present
    public static Actor Get_Actor(string actor_name)
    {
        // Search for our actor on the scene
        Actor item = actors_on_scene.Find(obj => obj.actor_name == actor_name);
        return item;
    }


    private static bool Name_Match(string actor_name, Actor current_actor)
    {
        return (current_actor != null && actor_name == current_actor.name);
    }


    // Returns the RectTransform positions of an actor position
    public static RectTransform Get_Position(Actor_Positions position)
    {
        return GameObject.Find(VNProperties.canvas_name + "/ActorPositions/" + Enum.GetName(typeof(Actor_Positions), position)).GetComponent<RectTransform>();
    }
    public static RectTransform Get_Side_Position(Actor_Positions position)
    {
        if (position == Actor_Positions.LEFT)
            return left;
        else
            return right;
    }

    // Instantiates an actor from the Resources/Actors folder with the name actor_name.
    // It then sets the object as a child of the Actors object in the canvas
    public static Actor Instantiate_Actor(string actor_name, Actor_Positions destination)
    {
        // Check to see if the actor is already on the scene
        Actor a = ActorManager.Get_Actor(actor_name);
        if (a != null)
        {
            Debug.Log("Actor " + actor_name + " already on scene");
            return a;
        }

        // Proceed with creating a gameobject
        GameObject actor = Instantiate(Resources.Load("Actors/" + actor_name, typeof(GameObject))) as GameObject;
        actor.transform.SetParent(GameObject.Find(VNProperties.canvas_name + "/Actors").transform, false);
        //actor.transform.localScale = Vector3.one;

        Actor actor_script = actor.GetComponent<Actor>();
        actors_on_scene.Add(actor_script);  // Add to list of actors

        ActorManager.Add_Actor_To(actor_script, destination);
        
        return actor_script;
    }


    // Makes all actors slightly dark
    public static void Darken_All_Actors()
    {
        foreach (Actor actor in actors_on_scene)
        {
            actor.Darken();
        }
    }
    // Makes all actors bright (normal colour)
    public static void Brighten_All_Actors()
    {
        foreach (Actor actor in actors_on_scene)
        {
            actor.Lighten();
        }
    }


    // Called by dialogue node to darken all actors but the one that's talking
    public static void Darken_All_Actors_But(Actor speaking_actor)
    {
        foreach (Actor actor in actors_on_scene)
        {
            // Don't darken this actor
            if (actor.name != speaking_actor.name)
            {
                actor.Darken();
            }
        }
    }


    // Brings the given actor to the front
    public static void Bring_Actor_To_Front(Actor actor)
    {
        actor.transform.SetAsLastSibling();
    }
    // Brings the given actor to the back
    public static void Bring_Actor_To_Back(Actor actor)
    {
        actor.transform.SetAsFirstSibling();
    }


    public static void Add_Actor_To(Actor actor, Actor_Positions position)
    {
        actor.position = position;
        List<Actor> side_list = null;

        switch (position)
        {
            case (Actor_Positions.LEFT):
                side_list = left_actors;
                break;
            case (Actor_Positions.RIGHT):
                side_list = right_actors;
                break;
        }

        // If actor is already present on that side, do nothing
        if (side_list.Contains(actor))
        {
            return;
        }

        // Actors are added starting from the edge, working their way in
        side_list.Add(actor);

        // Set their position on the screen
        Reevaluate_All_Actor_Positions();
    }


    // Moves the actor to the inwards most position of the side they're currently on
    public static void Move_Actor_Inwards(Actor actor)
    {
        List<Actor> side_list = null;

        switch (actor.position)
        {
            case (Actor_Positions.LEFT):
                side_list = left_actors;
                break;
            case (Actor_Positions.RIGHT):
                side_list = right_actors;
                break;
        }

        // Actors are added starting from the edge, working their way in
        side_list.Remove(actor);
        side_list.Add(actor);

        Reevaluate_All_Actor_Positions();
    }
    // Moves the actor to the outermost position of the side they're currently on
    public static void Move_Actor_Outwards(Actor actor)
    {
        List<Actor> side_list = null;

        switch (actor.position)
        {
            case (Actor_Positions.LEFT):
                side_list = left_actors;
                break;
            case (Actor_Positions.RIGHT):
                side_list = right_actors;
                break;
        }

        // Actors are added starting from the edge, working their way in
        side_list.Remove(actor);
        side_list.Insert(0, actor);

        Reevaluate_All_Actor_Positions();
    }


    // Gets the proper Y position for the actor so it always aligns with the bottom of the screen.
    public static float Get_Actor_Y_Position(Actor actor)
    {
        float y = (actor.rect.rect.height / 2f) * actor.rect.localScale.y - 300f;// (Screen.height / 2f);  // Make sure bottom always lines up with bottom of screen
        return y;
    }


    // If the order of the SIDES list has changed, call this to shuffle the actors to their new places
    public static void Reevaluate_All_Actor_Positions()
    {
        // Make sure screen dimensions are accurate
        float screen_width_modifier = Mathf.Clamp((float) Screen.width / 1680f, 0.7f, 1.5f);  // Use 1680 as a benchmark
        // Make sure actors are scaled correctly
        Vector3 new_scales = new Vector3((((float)Screen.width / 1780f)) * 0.04f, (((float)Screen.height / 1050.0f)) * 0.04f, 1);
        if (Mathf.Abs(screen_width_modifier - 1) <= 0.15f)
            new_scales.x = 0;   // Don't modify scale at all if screen is wide enough
        else if (new_scales.x > 1.0f)
            new_scales.x = -new_scales.x;
        if (new_scales.y > 1.0f)
            new_scales.y = -new_scales.y;
        
        // LEFT SIDE
        int offset_sign = 1;    // 1 or -1, depending on left or right
        List<Actor> side_list = null;
        side_list = left_actors;
        float local_x_offset = x_actor_offset;
        float starting_offset = 0;
        if (side_list.Count <= 2)
            local_x_offset = x_actor_offset * 1.7f;
        if (side_list.Count == 2)
            starting_offset = -50;
        if (side_list.Count > 2)
        {
            local_x_offset = x_actor_offset * 1.2f;
            starting_offset = -90;
        }
        foreach (Actor actor in side_list)
        {
            if (exiting_actors.Contains(actor))
            {
                // Don't reevaluate their position if exiting
            }
            else
            {
                // Set their position on the screen
                RectTransform rect = Get_Position(actor.position);
                actor.desired_position = new Vector3(
                    rect.localPosition.x + 
                    (side_list.IndexOf(actor) * offset_sign * local_x_offset * screen_width_modifier + 
                    starting_offset * offset_sign),// * screen_width_modifier),//rect.localPosition.x +
                    Get_Actor_Y_Position(actor),
                    rect.localPosition.z);
                actor.rect.localScale = new Vector3(actor.original_scale.x - new_scales.x, actor.original_scale.y, 1);// actor.original_scale.y - new_scales.y, 1);
            }
        }


        // RIGHT SIDE
        offset_sign = -1;
        side_list = right_actors;
        local_x_offset = x_actor_offset;
        starting_offset = 0;
        if (side_list.Count <= 2)
            local_x_offset = x_actor_offset * 1.7f;
        if (side_list.Count == 2)
            starting_offset = -50;
        if (side_list.Count > 2)
        {
            local_x_offset = x_actor_offset * 1.2f;
            starting_offset = -90;
        }
        foreach (Actor actor in side_list)
        {
            if (exiting_actors.Contains(actor))
            {
                // Don't reevaluate their position if exiting
            }
            else
            {
                // Set their position on the screen
                RectTransform rect = Get_Position(actor.position);
                actor.desired_position = new Vector3(
                    rect.localPosition.x + 
                    (side_list.IndexOf(actor) * offset_sign * local_x_offset * screen_width_modifier +
                    starting_offset * offset_sign),// * screen_width_modifier),//rect.localPosition.x +
                    Get_Actor_Y_Position(actor),
                    rect.localPosition.z);
                actor.rect.localScale = new Vector3(actor.original_scale.x - new_scales.x, actor.original_scale.y, 1);// actor.original_scale.y - new_scales.y, 1);
            }
        }
    }


    public void Slide_Start_Position(Actor actor, Actor_Positions position)
    {
        RectTransform rect = actor.GetComponent<RectTransform>();

        if (position == Actor_Positions.LEFT)
            rect.localPosition = offscreen_left.localPosition;
        if (position == Actor_Positions.RIGHT)
            rect.localPosition = offscreen_right.localPosition;

        rect.localPosition = new Vector3(rect.localPosition.x, ActorManager.Get_Actor_Y_Position(actor), rect.localPosition.z);
    }


    // Remove the actor from the scene, and destroys the game object
    public static void Remove_Actor(string actor_name)
    {
        Remove_Actor(Get_Actor(actor_name));
    }
    public static void Remove_Actor(Actor actor)
    {
        if (actor == null)
            return; // Didn't find an actor matching that name

        actors_on_scene.Remove(actor);
        left_actors.Remove(actor);
        right_actors.Remove(actor);
        exiting_actors.Remove(actor);
        actor.StopAllCoroutines();
        Destroy(actor.gameObject);

        Reevaluate_All_Actor_Positions();
    }
}
