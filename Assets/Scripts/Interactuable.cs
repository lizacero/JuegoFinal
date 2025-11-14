using System;
using TMPro;
using UnityEngine;

public class Interactuable : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    [SerializeField] private GameObject player;


    private bool enRango = false;
    private float distancia;
    [SerializeField] private float distanciaInteraccion = 8f;

    private void OnEnable()
    {
        inputManager.OnInteractuar += Interactuar;
    }

    private void OnDisable()
    {
        inputManager.OnInteractuar -= Interactuar;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distancia = Vector3.Distance(player.transform.position, this.transform.position);

        if (distancia <= distanciaInteraccion)
        {
            GameManager.instance.PanelInteraccion.SetActive(true);
            Debug.Log("dentro de la distancia");
            enRango = true;
        }
        else
        {
            GameManager.instance.PanelInteraccion.SetActive(false);
            Debug.Log("fuera de la distancia");
            enRango = false;
        }
    }
    private void Interactuar()
    {

        if (enRango && !GameManager.instance.RitualActivo)
        {
            StartCoroutine(GameManager.instance.ActivarRitual());
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            enRango = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            enRango = false;
            GameManager.instance.PanelInteraccion.SetActive(false);
        }
    }
}
