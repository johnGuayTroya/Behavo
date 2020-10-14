using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Wait : BehaviourTree.BaseNode
{
    private float waitingTime = 0;

    private bool started = false;
    private float lastTime = 0;
    private NPC npc;
    private Fruta fruit;


    public Wait(string id, float t,NPC _npc) : base(id)
    {
        npc = _npc;
        waitingTime = t / 1000.0f;
        Debug.Log("Wait: " + t);
    }

    public override Status Execute()
    {
       
        if (!started)
        {
            started = true;
            lastTime = Time.time;
            //npc.myAnims.SetBool("Pickup", true);
            return Status.Running;
        }

        var wt = Time.time - lastTime;
        if (wt > waitingTime)
        {
            started = false;
            if (npc.crafteo == true)
            {

                return Status.Success;
            }
            return Status.Failure; //Lo pongo a false para salir de la rama
        }

        else {
            
            return Status.Running;
        }

    }

    public override void Reset()
    {
        started = false;
        lastTime = 0;

        base.Reset();
    }
}
