using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FindFood : BehaviourTree.BaseNode
{
    private NPC npc;

    public FindFood(string id, NPC _npc) : base(id)
    {
        npc = _npc;
    }

    public override Status Execute()
    {

        //if (npc.smoothie1 > 1)
        //{
        //    Debug.Log("TengoSMothie" + npc.smoothie1);
        //    return Status.Failure;

        //}
        if (npc.foodNodes.Count > 0)
        {
            
            Debug.Log("BuscoComidaa");
            //aqui coje los nodos y los envía
            var nextPos2 = npc.foodNodes[Random.Range(0, npc.foodNodes.Count)];
            npc.nextPosition = nextPos2;

            return Status.Success;
        }
        //MIRAR esta parte
       
            
        return Status.Failure;

    }
}
