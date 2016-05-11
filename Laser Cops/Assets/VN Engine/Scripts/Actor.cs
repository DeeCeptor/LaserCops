using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Actor : MonoBehaviour
{
	public string actor_name;		// Actor name, use this name to reference this actor in dialogue 
	public Actor_Positions position;
	private float brigthness_change = 0.05f;

    [HideInInspector]
    public RectTransform rect;
    bool remove_when_out_of_sight = false;  // Called when sliding the actor out
    [HideInInspector]
    public Vector3 desired_position = Vector3.zero;    // Smoothly move actor to this position
    [HideInInspector]
    public Image fading_child_image;    // Used when switching character images
    [HideInInspector]
    public Image cur_image;
    [HideInInspector]
    public Vector3 original_scale;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        cur_image = this.GetComponent<Image>();
        original_scale = rect.localScale;

        if (this.transform.childCount > 0)
            fading_child_image = this.transform.GetChild(0).GetComponent<Image>();
    }
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

	// Makes this actor play the designated animation from its animator controller
	public void Play_Animation(string animation_name)
	{

	}


	// Calls a coroutine to fade in this actor over a number of seconds specified by over_time
	public void Fade_In(float over_time)
	{
		StartCoroutine(Fade_In_Coroutine(over_time));
	}
	IEnumerator Fade_In_Coroutine(float over_time)
	{
		float value = 0;
		// Set it to completely transparent
		this.GetComponent<Image>().color = new Color(
			this.GetComponent<Image>().color.r,
			this.GetComponent<Image>().color.g,
			this.GetComponent<Image>().color.b,
			0);

        float smoothness = 0.02f;   // Smaller, the smoother
        float increment = smoothness / over_time;
		// Incrementally make it less transparent
		while (this.GetComponent<Image>().color.a != 1)
		{
            value += increment;//(over_time / 100f);
			this.GetComponent<Image>().color = new Color(
				this.GetComponent<Image>().color.r,
				this.GetComponent<Image>().color.g,
				this.GetComponent<Image>().color.b,
				Mathf.Lerp(0, 1, value));
            yield return new WaitForSeconds(smoothness);//over_time / 100f);
        }
		yield break;
	}
    // Calls a coroutine to fade out this actor over a number of seconds specified by over_time.
    // Removes the actor from the scene after fading out
    public void Fade_Out(float over_time)
    {
        StartCoroutine(Fade_Out_Coroutine(over_time));
    }
    IEnumerator Fade_Out_Coroutine(float over_time)
    {
        if (!ActorManager.exiting_actors.Contains(this))
            ActorManager.exiting_actors.Add(this);

        float value = 0;
        // Set to completely opaque
        this.GetComponent<Image>().color = new Color(
            this.GetComponent<Image>().color.r,
            this.GetComponent<Image>().color.g,
            this.GetComponent<Image>().color.b,
            1);

        // Incrementally make it less transparent
        while (this.GetComponent<Image>().color.a != 0)
        {
            value += over_time / 100f;
            this.GetComponent<Image>().color = new Color(
                this.GetComponent<Image>().color.r,
                this.GetComponent<Image>().color.g,
                this.GetComponent<Image>().color.b,
                Mathf.Lerp(1, 0, value));
            yield return new WaitForSeconds(over_time / 100);
        }

        // Remove this actor after we're done
        Debug.Log("Removing actor " + actor_name);
        ActorManager.Remove_Actor(this.actor_name);

        yield break;
    }


    // Calls a coroutine to slide in this actor over a number of seconds specified by over_time
    public void Slide_In(Actor_Positions destination, float over_time)
	{
        rect = GetComponent<RectTransform>();

        ActorManager.Add_Actor_To(this, destination);

        // Set our starting point so we slide into our desired position
        ActorManager.actor_manager.Slide_Start_Position(this, position);
    }
    // Slides the actor out, and remove the actor after it has gone
    public void Slide_Out(float over_time)
    {
        rect = GetComponent<RectTransform>();

        if (!remove_when_out_of_sight)
        {
            if (this.position == Actor_Positions.LEFT)
            {
                ActorManager.left_actors.Remove(this);
                desired_position = ActorManager.actor_manager.offscreen_left.localPosition;
                if (!ActorManager.exiting_actors.Contains(this))
                    ActorManager.exiting_actors.Add(this);
            }
            else if (this.position == Actor_Positions.RIGHT)
            {
                ActorManager.right_actors.Remove(this);
                desired_position = ActorManager.actor_manager.offscreen_right.localPosition;
                if (!ActorManager.exiting_actors.Contains(this))
                    ActorManager.exiting_actors.Add(this);
            }
            desired_position.y = ActorManager.Get_Actor_Y_Position(this);

            remove_when_out_of_sight = true;
        }
    }

    public void Darken()
	{
		StopAllCoroutines();
		StartCoroutine(Darken_Coroutine());
	}
	IEnumerator Darken_Coroutine()
	{
		float value = 0;
		while (this.GetComponent<Image>().color != Color.gray)
		{
			value += brigthness_change;
			this.GetComponent<Image>().color = Color.Lerp(Color.white, Color.black, value);
			yield return new WaitForSeconds(0.03f);
		}
		yield break;
	}


	public void Lighten()
	{
		StopAllCoroutines();
		StartCoroutine(Lighten_Coroutine());
	}
	IEnumerator Lighten_Coroutine()
	{
		float value = 0;
		while (this.GetComponent<Image>().color != Color.white)
		{
			value += brigthness_change;
			this.GetComponent<Image>().color = Color.Lerp(Color.gray, Color.white, value);
			yield return new WaitForSeconds(0.03f);
		}
		yield break;
	}


	// Instantly places the actor at the designated position
	public void Place_At_Position(Actor_Positions destination)
	{
        Debug.Log("Placing actor at position");
		position = destination;

        ActorManager.Add_Actor_To(this, destination);
        this.rect = this.GetComponent<RectTransform>();
        this.rect.localPosition = this.desired_position;
    }


    public void Update()
    {
        // Move towards desired position if we are not at our desired position
        if (desired_position != Vector3.zero
            && this.rect.localPosition != desired_position)
        {
            this.rect.localPosition = Vector3.Lerp(this.rect.localPosition, desired_position, 3f * Time.deltaTime);//0.04f
            //Debug.Log(desired_position);
        }

        // If sliding out, check if we're visible. If we're not visible, remove the actor
        if (remove_when_out_of_sight
            &&
            //(Mathf.Abs(this.rect.position.x + (this.rect.sizeDelta.x / 2) * this.rect.localScale.x) >= Mathf.Abs(Screen.width) 
            //|| true || 
            Mathf.Abs(this.rect.localPosition.x - desired_position.x) <= 30
            //)
            )
        {
            ActorManager.Remove_Actor(this.actor_name);
        }
    }
}
