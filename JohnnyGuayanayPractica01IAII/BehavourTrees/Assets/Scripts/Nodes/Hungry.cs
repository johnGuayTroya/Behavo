using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;


public class Hungry : BehaviourTree.BaseNode
{

    NPC player;

    public Hungry(string id,NPC _npc) : base(id)
    {
        player = _npc;
        
    }

    public override Status Execute()
    {
        
        if (player.hungryTime<5 && player.IHungry==true) //Mirar aaqui
        {
            
            player.txtAvisoArbol.text = "tengoHambre!!!";
            Debug.Log("voy a buscar food");
            player.IHungry = true;
            return Status.Success;
           
        }
        else
        {
            
            return Status.Failure;
        }

    }

   
}
