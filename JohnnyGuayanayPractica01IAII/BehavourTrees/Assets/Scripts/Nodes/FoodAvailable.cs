using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FoodAvailable : BehaviourTree.BaseNode
{

    NPC npc;
    public FoodAvailable(string id,NPC _npc) : base(id)
    {
        npc = _npc;
        
    }

    public override Status Execute()
    {


        
        if (npc.smoothie1 >= 1)
        {
            return Status.Success;
        }
        else if (npc.totalFruit.Count>npc.maxInventary) 
        {

            
            return Status.Success;

        }
        if (npc.apple == 1 && npc.banana == 1 && npc.apple == 1) //condición auxiliar 
        {
            return Status.Success;
        }

        else
        {
            Debug.Log("No hay comida");
            return Status.Failure;
        }
        

    }
}
