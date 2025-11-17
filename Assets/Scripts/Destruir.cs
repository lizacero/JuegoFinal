using System.Collections;
using UnityEngine;

public class Destruir : MonoBehaviour
{
    // Inicia la corrutina que destruye el objeto.
    void Start()
    {
        StartCoroutine(DestruirObjeto());
    }
    //Corrutina que espera un tiempo para destruir al objeto.
    private IEnumerator DestruirObjeto()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
