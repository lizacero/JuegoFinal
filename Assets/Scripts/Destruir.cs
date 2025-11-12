using System.Collections;
using UnityEngine;

public class Destruir : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestruirObjeto());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DestruirObjeto()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
