using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class GetNextPosition : BehaviourTree.BaseNode
{
    private NPC npc;
    
    public GetNextPosition(string id, NPC _npc) : base(id)
    {
        npc = _npc;
    }

    public override Status Execute()
    {
        npc.crafteo = false;
        if (npc.hungryTime < 5)
        {
            Debug.Log("tengoHmbreee");
            npc.IHungry = true;
            return Status.Failure;
        }
        if (npc.hungryTime >= 5) { npc.IHungry = false; }

        if (npc.hungryTime > 4) { npc.hungryTime--; } 
        //if (npc.hungryTime <= 4) { npc.IHungry = false; }
        if (npc.IHungry == true) { return Status.Failure; }
        npc.HungryBar();
        
      
        if (npc.wanderNodes.Count > 0 && npc.hungryTime > 4)
        {
            
            //aqui coje los nodos y los envía
            var nextPos = npc.wanderNodes[Random.Range(0, npc.wanderNodes.Count)];
            Debug.Log("getnexpositionEjecutandose");
          
            npc.nextPosition = nextPos;
           
            return Status.Success;
            
        }
        
        return Status.Failure;

    }

}
