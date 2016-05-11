using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Changes the sprite being displayed for this actor
public class ChangeActorImageNode : Node
{
    public string actor_name;   // Actor who is to change images
    public Sprite new_image;
    public bool fade_in_new_image = true;  // Whether or not to fade out the old image
    float fade_out_time = 0.4f;     // In seconds
    public bool lighten_actor;
    public bool bring_actor_to_front;

    public override void Run_Node()
    {
        if (ActorManager.Is_Actor_On_Scene(actor_name))
        {
            if (fade_in_new_image)
            {
                Actor actor = ActorManager.Get_Actor(actor_name);
                Sprite old_sprite = actor.GetComponent<Image>().sprite;

                if (lighten_actor)
                    actor.Lighten();
                if (bring_actor_to_front)
                    ActorManager.Bring_Actor_To_Front(actor);

                // Fade out old image
                actor.fading_child_image.gameObject.SetActive(true);
                actor.fading_child_image.sprite = old_sprite;
                // Set colour of old image to match current image
                actor.fading_child_image.color = actor.GetComponent<Image>().color;
                actor.fading_child_image.GetComponent<RectTransform>().sizeDelta = actor.rect.sizeDelta;
                StartCoroutine(Fade_Out_Coroutine(fade_out_time, actor.fading_child_image));

                // Fade in new image
                actor.GetComponent<Image>().sprite = new_image;
                StartCoroutine(Fade_In_Coroutine(fade_out_time, actor.GetComponent<Image>()));

                // Wait before we allow it to finish
                StartCoroutine(Wait(actor, fade_out_time * 2f));
            }
            else
            {
                ActorManager.Get_Actor(actor_name).GetComponent<Image>().sprite = new_image;
                Finish_Node();
            }
        }
        else
        {
            Finish_Node();

            Debug.Log(actor_name + " is not on the scene. Remember to correctly name your actor and use 'EnterActorNode'");
        }
    }


    IEnumerator Wait(Actor actor, float how_long)
    {
        yield return new WaitForSeconds(how_long);
        actor.fading_child_image.gameObject.SetActive(false);
        Finish_Node();
    }
    IEnumerator Fade_In_Coroutine(float over_time, Image image)
    {
        float value = 0;
        // Set it to completely transparent
        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            0);

        float smoothness = 0.02f;   // Smaller, the smoother
        float increment = smoothness / over_time;
        // Incrementally make it less transparent
        while (image.color.a != 1)
        {
            value += increment;//(over_time / 100f);
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
				Mathf.Lerp(0, 1, value * value * (3.0f - 2.0f * value)));
            yield return new WaitForSeconds(smoothness);//over_time / 100f);
        }
        yield break;
    }
    IEnumerator Fade_Out_Coroutine(float over_time, Image image)
    {
        float value = 0;
        // Set to completely opaque
        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            1);

        float smoothness = 0.02f;   // Smaller, the smoother
        float increment = smoothness / over_time;
        // Incrementally make it less transparent
        while (image.color.a != 0)
        {
            value += increment;
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
				Mathf.Lerp(1, 0, value * value * value * value * (3.0f - 2.0f * value)));
            yield return new WaitForSeconds(smoothness);
        }
        yield break;
    }


    public override void Finish_Node()
    {
        StopAllCoroutines();

        base.Finish_Node();
    }
}
