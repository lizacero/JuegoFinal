using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameplay : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;

    [Header("Paneles de Game Over")]
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;

    [Header("Panel pausa")]
    [SerializeField] private GameObject panelPausa;
    private bool pausado = false;

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
    void Start()
    {
        // Asegurarse de que los paneles estén desactivados al inicio
        if (panelWin != null)
            panelWin.SetActive(false);
        if (panelLose != null)
            panelLose.SetActive(false);
        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    private void Esc()
    {
        if (panelPausa != null)
        {
            if (pausado == false)
            {
                panelPausa.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                pausado = true;
            }
            else
            {
                panelPausa.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                pausado = false;
            }
        }
    }

    public void MostrarPanelWin()
    {
        if (panelWin != null)
        {
            panelWin.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    public void MostrarPanelLose()
    {
        if (panelLose != null)
        {
            panelLose.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    public void Volver()
    {
        SceneManager.LoadScene(0);
    }

    public void Reintentar()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Salir()
    {
        Debug.Log("Saliendo");
        Application.Quit();
    }

    public void Continuar()
    {
        panelPausa.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void Creditos()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

}
