using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class craft : BehaviourTree.BaseNode
{
    private NPC npc;

    public craft(string id, NPC _npc) : base(id)
    {
        npc = _npc;
    }

    public override Status Execute()
    {

        //TEngo que poner a craftear cosas
        return Status.Failure;

    }


}
