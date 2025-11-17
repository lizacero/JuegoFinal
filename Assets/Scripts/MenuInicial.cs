using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    //Carga la escena de gameplay cuando el jugador pulsa Jugar.
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    //Cierra la aplicación del juego.
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }

    //Cambia a la escena de créditos y asegura que Time.timeScale se encuentre en 1.
    public void Creditos()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
}
