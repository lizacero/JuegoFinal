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

    //Se suscribe al evento de interacción al habilitar el objeto.
    private void OnEnable()
    {
        inputManager.OnInteractuar += Interactuar;
    }

    //Cancela la suscripción al evento de interacción al deshabilitar el objeto.
    private void OnDisable()
    {
        inputManager.OnInteractuar -= Interactuar;
    }

    // Calcula cada frame la distancia al jugador, muestra u oculta el panel de interacción y determina si está dentro del rango permitido.
    void Update()
    {
        distancia = Vector3.Distance(player.transform.position, this.transform.position);

        if (distancia <= distanciaInteraccion)
        {
            GameManager.instance.PanelInteraccion.SetActive(true);
            //Debug.Log("dentro de la distancia");
            enRango = true;
        }
        else
        {
            GameManager.instance.PanelInteraccion.SetActive(false);
            //Debug.Log("fuera de la distancia");
            enRango = false;
        }
    }

    // Ejecuta la lógica de interacción cuando el jugador pulsa E dentro del rango. Inicia el ritual si aún no está activo.
    private void Interactuar()
    {

        if (enRango && !GameManager.instance.RitualActivo)
        {
            StartCoroutine(GameManager.instance.ActivarRitual());
        }
        
    }

    // Marca que el jugador ha entrado en el trigger del objeto interactuable.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            enRango = true;
        }
    }

    // Marca la salida del jugador del trigger y oculta el panel de interacción.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            enRango = false;
            GameManager.instance.PanelInteraccion.SetActive(false);
        }
    }
}
