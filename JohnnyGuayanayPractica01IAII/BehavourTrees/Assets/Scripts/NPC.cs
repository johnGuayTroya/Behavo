using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NPC : MonoBehaviour
{
    // Let's start with a simple wander
    public Transform wanderParent;
    public Transform foodParent;
    public TextAsset behaviourTree;
    public BTModel.BehaviourTree loadedTree;
    public List<Transform> wanderNodes = new List<Transform>();
    public List<Transform> foodNodes = new List<Transform>();
    public Transform nextPosition = null;
    public UnityEngine.AI.NavMeshAgent agent;

    private BehaviourTree.BaseNode root;


    //Johnny code----------------------------------------------------------
    [Header("Animations")]
    [SerializeField] public Animator myAnims;
    [SerializeField] public Material crafteoMaterial;
    [SerializeField] public GameObject crafteoFireWorks;


    [Header("Hungry vars")]
    [SerializeField]private bool iHungry=false;
    public Image hungryBar;
    public float hungryTime = 5f;
    public bool consumo = false;
    public Text items;
    private float initialHungry;

    [Header("Inventary")]
    public float maxInventary = 8;
    public List<Fruta> totalFruit = new List<Fruta>();

    public Fruta _apple;
    public Fruta _banana;
    public Fruta _orange;
    public Fruta _smoothie1;
    public bool crafteo;


    private Material firtsMaterial;
    private SkinnedMeshRenderer myMesh;
    [Header("trees")]
    public Fruta arbol;
    public Text txtAvisoArbol;


    public float apple = 0;
    public float banana = 0;
    public float orange = 0;
    public float smoothie1 = 0;
    //Contadores auxiliares
    int _intApple;
    int _intBanana;
    int _intOrange;

    public bool IHungry
    {
        get { return iHungry; }
        set { iHungry = value; }
    }

    enum States
    {
        Moving,
        Waiting,
        Eating,
    }
    States currentState;

    // Start is called before the first frame update
    void Start()
    {
        myMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        firtsMaterial = GetComponentInChildren<SkinnedMeshRenderer>().material;
        myAnims = GetComponent<Animator>();
        //
        _apple = new Fruta(2,apple,"apple",true);
        _banana = new Fruta(3,banana, "banana",true);
        _orange = new Fruta(4, orange, "orange",true);
        _smoothie1 = new Fruta(6, smoothie1, "SmoothieMagico",true);

        initialHungry = hungryTime;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        wanderNodes.AddRange(wanderParent.GetComponentsInChildren<Transform>());
        foodNodes.AddRange(foodParent.GetComponentsInChildren<Transform>());

        wanderNodes.Remove(wanderParent); //eliminamos el padre porque es un objeto que almacena a los demás hijos
        foodNodes.Remove(foodParent);

        ParseBehaviour();

    }

    // Update is called once per frame
    void Update()
    {
        items.text = "Apple: "+apple+" Orange:"+orange+" Banana:"+banana+" SmoothieM:"+smoothie1;
        if (root != null)
        {
            root.Execute();
        }
    }
    public void HungryBar()
    {
        hungryBar.fillAmount = hungryTime/ initialHungry;
    }
    public void  Eat()
    {
        //está hecho de tal manera que beberá el smoothie primero
        if (crafteo == true)
        {
            if (totalFruit.Contains(_smoothie1) && smoothie1 > 0 && consumo == false)
            {
                consumo = true;
                SacioHambre(_smoothie1.HambreSacio);
                smoothie1--;
                StartCoroutine(Avisos("Consumo SMOTHIE MÁGICOO!!"));
                Debug.Log("Como" + _smoothie1.TipoFruta + "gano" + _smoothie1.HambreSacio);
                totalFruit.Remove(_smoothie1);
                maxInventary = 1;
                
            }
        }

        else
        {
            if (orange > apple || orange > banana || orange >= 2 && consumo == false && totalFruit.Contains(_orange))
            {
                consumo = true;
                SacioHambre(_orange.HambreSacio); //?
                orange--;
                StartCoroutine(Avisos("Como:" + _orange.TipoFruta + " gano:" + _orange.HambreSacio));
                totalFruit.Remove(_orange);
                maxInventary += 1;
                
            }

            else if (apple > orange || apple > banana && apple >= 2 && consumo == false && totalFruit.Contains(_apple))
            {
                consumo = true;
                SacioHambre(_apple.HambreSacio);
                apple--;
                StartCoroutine(Avisos("Como" + _apple.TipoFruta + "gano" + _apple.HambreSacio));
                totalFruit.Remove(_apple);
                maxInventary += 1;
                

            }

            if (banana > orange || banana > apple && banana >= 2 && consumo == false && totalFruit.Contains(_banana))
            {
                consumo = true;
                SacioHambre(_banana.HambreSacio);
                banana--;
                StartCoroutine(Avisos("Como" + _banana.TipoFruta + "gano" + _banana.HambreSacio));
                totalFruit.Remove(_banana);
                maxInventary += 1;
                


            }

        }    

    }
    public void CreoBatido()
    {
        
        for(int i = 0; i < totalFruit.Count; i++)
        {
            if (totalFruit.Contains(_apple) && _intApple<1){
                _intApple = 1;
                totalFruit.Remove(_apple);
            }
            if (totalFruit.Contains(_banana) && _intBanana<1)
            {
                _intBanana =1 ;
                totalFruit.Remove(_banana);
            }
            if (totalFruit.Contains(_orange) && _intOrange<1)
            {
                _intOrange = 1;
                totalFruit.Remove(_orange);
            }
        }
        apple--;
        banana--;
        orange--;
        StartCoroutine(CrafteoAnim());
        smoothie1++;
        totalFruit.Add(_smoothie1);
        if (_intApple == 1 && _intBanana == 1 && _intOrange == 1)
        {
            _intOrange = _intBanana = _intApple = 0;
            Debug.Log("variables frutas auxiliares =" + _intApple);
        }

    }
    IEnumerator CrafteoAnim()
    {
        crafteoFireWorks.SetActive(true);
        myMesh.material = crafteoMaterial;
        yield return new WaitForSeconds(2.5f);
        myMesh.material = firtsMaterial;
        crafteoFireWorks.SetActive(false);
    }
    public IEnumerator Avisos(string mensaje)
    {

        txtAvisoArbol.text = mensaje;
        yield return new WaitForSeconds(3);
        txtAvisoArbol.text = "";
    }
   
    #region Behaviour tree loading
    private void ParseBehaviour()
    {
        //leemos el Json y creamos los  nodos
        JsonSerializer js = new JsonSerializer();
        loadedTree = js.Deserialize<BTModel.BehaviourTree>(new JsonTextReader(new System.IO.StringReader(behaviourTree.text)));
        loadedTree.debugNodes = loadedTree.nodes.Values.ToArray();

        // create real tree

        // create nodes
        List<BehaviourTree.BaseNode> nodes = new List<BehaviourTree.BaseNode>();
        foreach (var n in loadedTree.nodes)
        {
            nodes.Add(CreateNode(n.Value));
        }

        // assamble nodes
        root = nodes.Find(n => n.Id == loadedTree.root); //buscamos el id 

        // set relationships
        foreach (var n in loadedTree.nodes) //se mueve por los nodos cargados
        {
            var modelNode = n.Value;

            BehaviourTree.BaseNode currentNode = nodes.Find(n2 => n2.Id == modelNode.id);
            if (modelNode.child != null)
            {
                var child = nodes.Find(n3 => n3.Id == modelNode.child);
                AddChild(currentNode, child);
            }
            else if (modelNode.children.Count > 0)
            {
                foreach (string childId in modelNode.children)
                {
                    var child = nodes.Find(n3 => n3.Id == childId);

                    if (!(child is BehaviourTree.NoopNode))
                    {
                        AddChild(currentNode, child);
                    }
                    else
                    {
                        AddChild(currentNode, child);
                        // Debug.Log("New type: " + currentNode.GetType().ToString());
                    }
                }
            }
        }
      
        List<BehaviourTree.BaseNode> traverseNodes = new List<BehaviourTree.BaseNode>();

        root = nodes.Find(n => n.Id == loadedTree.root);
        traverseNodes.Add(root);

        if (root is BehaviourTree.RepeatUntilFail)
        {
            Debug.Log("Debugger went nuts!");
        }

        while (traverseNodes.Count > 0)
        {
            var topNode = traverseNodes[0];
            traverseNodes.RemoveAt(0);

            Debug.Log(topNode.GetType().ToString());

            if (topNode is BehaviourTree.RepeatUntilFail)
            {
                traverseNodes.Add(((BehaviourTree.RepeatUntilFail)topNode).Node);
            }
            else if (topNode is BehaviourTree.BaseNodeList)
            {
                foreach (var n in ((BehaviourTree.BaseNodeList)topNode).Nodes)
                {
                    traverseNodes.Add(n);
                }
            }
            else if (topNode is BehaviourTree.Inverter)
            {
                traverseNodes.Add(((BehaviourTree.Inverter)topNode).Node);
            }
            else if (topNode is BehaviourTree.Succeder)
            {
                traverseNodes.Add(((BehaviourTree.Succeder)topNode).Node);
            }
        }
    }

    private BehaviourTree.BaseNode CreateNode(BTModel.BehaviourTreeNode data)
    {
        switch (data.name.ToLower()) //toLower convierte el string en minusculas
        {
            case "repeatuntilfailure":
                return new BehaviourTree.RepeatUntilFail(data.id);

            case "sequence":
              /*  if (data.description == "Hungry") { return new BehaviourTree.Sequence("ae1d73ce - db2f - 4bf9 - bf8c - 0c2781954d3f"); }*/ //añadidomíoFALLA
                return new BehaviourTree.Sequence(data.id);

            case "priority":
                return new BehaviourTree.Selector(data.id);

            case "wait":
                int ms = System.Convert.ToInt32(data.properties["milliseconds"]);
                return new Wait(data.id, ms,this);

            case "getnextposition":
                Debug.Log("voy a buscar posicion");
                return new GetNextPosition(data.id, this);

            case "movetoposition":
                return new MoveToPosition(data.id, this);

            //hunugryyy....-----------.----------------.------------.----------------
            case "hungry?":
                return new Hungry(data.id,this);

            case "foodavailable?":
                return new FoodAvailable(data.id,this);

            case "findfood":
                return new FindFood(data.id, this);

            case "eat":
                return new Eat(data.id, this);

            case "createfruitmix":
                return new CreateFruitMix(data.id, this);


            default:
                return new BehaviourTree.NoopNode(data.id, data.name);
        }
    }
    private void SacioHambre(float gano)
    {
        hungryTime += gano;
    }
    private void AddChild(BehaviourTree.BaseNode parent, BehaviourTree.BaseNode n)
    {
        // throw new System.Exception("Error! Can't add child to node " + parent.Id + " (" + parent.GetType().ToString() + ")");

        // UGLY!!!!
        if (parent is BehaviourTree.RepeatUntilFail)
        {
            AddChild((BehaviourTree.RepeatUntilFail)parent, n);
        }
        else if (parent is BehaviourTree.BaseNodeList)
        {
            AddChild((BehaviourTree.BaseNodeList)parent, n);
        }
        else if (parent is BehaviourTree.Inverter)
        {
            AddChild((BehaviourTree.Inverter)parent, n);
        }
        else
        {
            throw new System.Exception("Error! Can't add child to node " + parent.Id + " (" + parent.GetType().ToString() + ")");
        }
    }

    private void AddChild(BehaviourTree.RepeatUntilFail parent, BehaviourTree.BaseNode n)
    {
        parent.Node = n;
    }

    private void AddChild(BehaviourTree.BaseNodeList parent, BehaviourTree.BaseNode n)
    {
        // Debug.Log("Trying to add child [" + n.Id + ":" + n.GetType().ToString() + "] to node " + parent.Id + ":" + parent.GetType().ToString());
        parent.AddNode(n);
    }

    private void AddChild(BehaviourTree.Inverter parent, BehaviourTree.BaseNode n)
    {
        parent.Node = n;
    }

    private void AddChild(BehaviourTree.Succeder parent, BehaviourTree.BaseNode n)
    {
        parent.Node = n;
    }
    #endregion
}
