using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class MoveToPosition : BehaviourTree.BaseNode
{
    private NPC npc;
    
    public MoveToPosition(string id, NPC _npc) : base(id)
    {
        npc = _npc;
    }

    public override Status Execute()
    {
        
        if (npc.nextPosition != null)
        {
            npc.consumo = false;
            npc.agent.SetDestination(npc.nextPosition.position);
            npc.GetComponent<Animator>().SetFloat("MoveSpeed", npc.agent.velocity.magnitude);

            var distance = npc.nextPosition.position - npc.transform.position;
            distance.y = 0;
          
            if (distance.sqrMagnitude < 0.5f*0.5f)
            {
                switch (npc.nextPosition.tag)
                {
                    case "arbolManzana":

                        npc.myAnims.SetBool("Pickup", true);
                        npc.totalFruit.Add(npc._apple);
                        npc.apple++;
                        npc.nextPosition.tag= "recargando";
                 
                        return Status.Success;
                    case "arbolPlatano":

                        npc.myAnims.SetBool("Pickup", true);
                        npc.totalFruit.Add(npc._banana);
                        npc.banana++;
                        npc.nextPosition.tag = "recargando";
                       
                        return Status.Success;
                    case "arbolNaranja":

                        npc.myAnims.SetBool("Pickup", true);
                        npc.totalFruit.Add(npc._orange);
                        npc.orange++;
                        npc.nextPosition.tag = "recargando";
                        
                        return Status.Success;

                    case "recargando":
                       
                        Debug.Log("ESPERA PARA COMERME x3");
                        npc.StartCoroutine(npc.Avisos("Árbol Cargándose"));
                        
                        return Status.Success;

                }
                
                npc.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
                return Status.Success;
            }

            return Status.Running;
        }

        return Status.Failure;
    }
}
