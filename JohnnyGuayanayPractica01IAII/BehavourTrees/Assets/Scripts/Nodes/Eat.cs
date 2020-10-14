using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Eat : BehaviourTree.BaseNode
{
    private NPC npc;

    public Eat(string id, NPC _npc) : base(id)
    {
        npc = _npc;
    }

    public override Status Execute()
    {
        
        if (npc.totalFruit.Count>=1)
        {
            //Debug.Log("BeboSmoothieee");
            consumo();
     
            return Status.Success;
        }
        
        return Status.Failure;

    }

    public void consumo()
    {
        npc.Eat();
        npc.HungryBar();
        npc.IHungry = false;
       
    }
    
}
