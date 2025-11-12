using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    [SerializeField] private GameObject btnMenu;
    [SerializeField] private GameObject btnSalir;

    private void OnEnable()
    {
        inputManager.OnEsc += Esc;
    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnEsc -= Esc;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        btnMenu.SetActive(false);
        btnSalir.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Esc()
    {
        SceneManager.LoadScene(0);
    }

    private void ActivarBotones()
    {
        btnMenu.SetActive(true);
        btnSalir.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }
}
