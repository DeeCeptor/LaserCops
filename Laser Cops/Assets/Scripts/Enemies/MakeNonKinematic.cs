using UnityEngine;
using System.Collections;

public class MakeNonKinematic : MonoBehaviour
{
    SideToSide script;
    public Rigidbody2D other_obj;

    void Start ()
    {
        script = this.GetComponent<SideToSide>();
    }


    void Update()
    {
        if (script.active)
        {
            this.GetComponent<Rigidbody2D>().isKinematic = false;

            if (other_obj != null)
                other_obj.isKinematic = false;

            Destroy(this);
        }
    }
}
