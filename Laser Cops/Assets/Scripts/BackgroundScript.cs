using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private Vector3 backPos;
    public float width = 14.22f;
    public float height = 0f;

    private float X;
    private float Y;

    void Awake()
    {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        width = sprite.bounds.size.x;
        //width = 0;
    }

    void Update()
    {
        this.transform.position = this.transform.position + Physics.gravity * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        //calculate current position
        backPos = gameObject.transform.position;
        //calculate new position
        //X = backPos.x + width * 2;
        //Y = backPos.y + height * 2;
        X = width / 2 + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        //move to new position when invisible
        gameObject.transform.position = new Vector3(X, Y, 0f);
    }

}