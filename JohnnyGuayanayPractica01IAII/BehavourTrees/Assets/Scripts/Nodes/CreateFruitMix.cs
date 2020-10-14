using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CreateFruitMix : BehaviourTree.BaseNode
{

    private NPC npc;
   
    bool _blnStarted;
    float _flLastTime;
  
    public CreateFruitMix(string id, NPC _npc) : base(id)
    {
        npc = _npc;
    }

    public override Status Execute()
    {

        if (npc.orange >=1 && npc.apple>=1 && npc.banana>=1)
        {
            npc.crafteo = true;
            Debug.Log("Crafteo");
            npc.StartCoroutine(npc.Avisos("Crafting in process"));
            npc.CreoBatido();
            npc.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
            return Status.Success;
        }

        //return Status.Running;

        return Status.Failure;

        

    }
    
   
}
