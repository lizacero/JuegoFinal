using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    [SerializeField] private GameObject btnMenu;
    [SerializeField] private GameObject btnSalir;

    //Se suscribe al evento Esc del InputManager al habilitase el componente.
    private void OnEnable()
    {
        inputManager.OnEsc += Esc;
    }

    //Se cancela la suscripción al evento Esc cyabdi se deshabilita el componente.
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnEsc -= Esc;
        }
    }
    //Inicializa la escena de créditos: oculta botones y asegura que el tiempo fluya.
    void Start()
    {
        btnMenu.SetActive(false);
        btnSalir.SetActive(false);
        Time.timeScale = 1;
    }

    //Cambia a la escena del menú principal cuando el usuario presiona Esc.
    private void Esc()
    {
        SceneManager.LoadScene(0);
    }

    //Muestra los botones de menú y salir. Se llama al finaliza la animación de los créditos.
    private void ActivarBotones()
    {
        btnMenu.SetActive(true);
        btnSalir.SetActive(true);
    }

    //Cambia a la escena del menú principal al seleccionar el botón.
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    //Cierra la aplicación al seleccionar el botón.
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }
}
