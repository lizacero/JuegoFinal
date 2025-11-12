using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo");
    }

    public void Creditos()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
}
