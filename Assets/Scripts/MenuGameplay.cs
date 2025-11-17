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

    //Se suscribe al evento Esc del input manager cuando el menú entra en escena.
    private void OnEnable()
    {
        inputManager.OnEsc += Esc;
    }

    //Cancela la suscripción al evento Esc al deshabilitar el menú.
    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnEsc -= Esc;
        }
    }

    //Se asegura que todos los paneles estén desactivados al cargar la escena.
    void Start()
    {
        if (panelWin != null)
            panelWin.SetActive(false);
        if (panelLose != null)
            panelLose.SetActive(false);
        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    //Alterna el panel de pausa, cursor, audio y timeScale al presionar Esc.
    private void Esc()
    {
        if (panelPausa != null)
        {
            if (pausado == false)
            {
                panelPausa.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                AudioListener.pause = true;
                Time.timeScale = 0;
                pausado = true;
            }
            else
            {
                panelPausa.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                AudioListener.pause = false;
                Time.timeScale = 1;
                pausado = false;
            }
        }
    }

    //Muestra el panel de victoria, muestra el cursor y congela el juego.
    public void MostrarPanelWin()
    {
        if (panelWin != null)
        {
            panelWin.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudioListener.pause = true;
            Time.timeScale = 0;
        }
    }

    //Muestra el panel de derrota, muestra el cursor y congela el juego.
    public void MostrarPanelLose()
    {
        if (panelLose != null)
        {
            panelLose.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudioListener.pause = true;
            Time.timeScale = 0;
        }
    }

    //Vuelve al menú principal restableciendo audio y timeScale.
    public void Volver()
    {
        SceneManager.LoadScene(0);
        AudioListener.pause = false;
    }

    //Recarga la escena de gameplay con el juego en tiempo normal.
    public void Reintentar()
    {
        AudioListener.pause = false;
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    //Cierra la aplicación del juego.
    public void Salir()
    {
        Debug.Log("Saliendo");
        Application.Quit();
    }

    //Reanuda el gameplay escondiendo el panel de pausa y bloqueando el cursor.
    public void Continuar()
    {
        panelPausa.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    //Carga la escena de créditos asegurando que audio y tiempo estén activos.
    public void Creditos()
    {
        SceneManager.LoadScene(2);
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

}
