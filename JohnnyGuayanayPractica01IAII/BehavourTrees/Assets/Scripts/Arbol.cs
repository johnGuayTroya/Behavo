using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arbol: MonoBehaviour
{

    public float _fltTiempoRecarga = 10f;
    public Transform[] hijos;
    private NPC npc;
   
    void Start()
    {
        
        hijos = GetComponentsInChildren<Transform>();
        
        npc = FindObjectOfType<NPC>();
       
    }

    void Update()
    {
        BuscaArbolCargandoseManzana();
     
    }
    public void BuscaArbolCargandoseManzana()
    {

        foreach (Transform arbol in hijos)
        {

            if (hijos[1].gameObject.tag == "recargando")
            {

                StartCoroutine(ArbolManzanaRecarga(hijos[1]));

            }
            if (hijos[2].gameObject.tag == "recargando")
            {

                StartCoroutine(ArbolManzanaRecarga(hijos[2]));

            }
            if (hijos[3].gameObject.tag == "recargando")
            {

                StartCoroutine(ArbolManzanaRecarga(hijos[3]));

            }
            if (hijos[4].gameObject.tag == "recargando")
            {
                StartCoroutine(ArbolNaranjaRecarga(hijos[4]));
            }
            if (hijos[5].gameObject.tag == "recargando")
            {
                StartCoroutine(ArbolNaranjaRecarga(hijos[5]));
            }
            if (hijos[6].gameObject.tag == "recargando")
            {
                StartCoroutine(ArbolBananaRecarga(hijos[6]));
            }
            //if (hijos[7].gameObject.tag == "recargando")
            //{
            //    StartCoroutine(ArbolBananaRecarga(hijos[7]));
            //}
            //if (hijos[8].gameObject.tag == "recargando")
            //{
            //    StartCoroutine(ArbolBananaRecarga(hijos[8]));
            //}
            //if (hijos[hijos.Length - 1].gameObject.tag == "recargando")
            //{
            //    StartCoroutine(ArbolBananaRecarga(hijos[hijos.Length - 1]));

            //}
        }
        //public void BuscarArbolCargandoseNaranja()
        //{
        //    foreach (Transform arbol in hijos) {


        //    }


        //}
        //public void BuscarArbolCargandoseBanana()
        //{
        //    foreach (Transform arbol in hijos)
        //    {
        //       
        //        }

        //    }

        //}
        IEnumerator ArbolManzanaRecarga(Transform arbol)
        {

            yield return new WaitForSeconds(_fltTiempoRecarga);
            arbol.gameObject.tag = "arbolManzana";

        }
        IEnumerator ArbolBananaRecarga(Transform arbol)
        {
            yield return new WaitForSeconds(_fltTiempoRecarga);
            arbol.gameObject.tag = "arbolPlatano";
        }
        IEnumerator ArbolNaranjaRecarga(Transform arbol)
        {
            yield return new WaitForSeconds(_fltTiempoRecarga);
            arbol.gameObject.tag = "arbolNaranja";
        }






    }

}
public class Fruta
{
    float _fltHambreSacio = 0;
    string _strTipoFruta = "";
    float _fltCantidad = 0;

    bool _blnDisponible = false;

    public float HambreSacio
    {
        get { return _fltHambreSacio; }
        set { _fltHambreSacio = value; }
    }
    public string TipoFruta
    {
        get { return _strTipoFruta; }
        set { _strTipoFruta = value; }

    }

    public Fruta(float hambreSacio, float cantidad, string tipoFruta, bool disponible)
    {
        _fltHambreSacio = hambreSacio;
        _fltCantidad = cantidad;
        _strTipoFruta = tipoFruta;
        _blnDisponible = disponible;

    }
}
