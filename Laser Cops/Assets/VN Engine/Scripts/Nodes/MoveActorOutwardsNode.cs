using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Moves the actor towards the center of the screen, while still staying on its current side (LEFT or RIGHT)
public class MoveActorOutwardsNode : Node
{
    public string actor_name;

    public override void Run_Node()
    {
        ActorManager.Move_Actor_Outwards(ActorManager.Get_Actor(actor_name));

        Finish_Node();
    }


    public override void Finish_Node()
    {
        StopAllCoroutines();

        base.Finish_Node();
    }
}
