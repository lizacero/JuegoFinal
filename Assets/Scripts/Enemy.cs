using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour, Daniable
{
    private NavMeshAgent agent;
    private Player target;
    private Animator anim;
    [SerializeField] private Transform puntoDrop;
    //private bool delay;

    [Header("Sistema de combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danioAtaque;
    private float vidaEnemigo = 20;
    
    [Header("Sistema de Game Over")]
    [SerializeField] private MenuGameplay menuGameplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = FindAnyObjectByType<Player>();
        agent.SetDestination(target.transform.position);
        //delay = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("walking",true);
        agent.SetDestination(target.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            EnfocarOnjetivo();
            LanzarAtaque();
        }
    }

    private void EnfocarOnjetivo()
    {
        Vector3 direccionAObjetivo = (target.transform.position - transform.position).normalized;
        direccionAObjetivo.y = 0;
        Quaternion rotacionAObjetivo = Quaternion.LookRotation(direccionAObjetivo);
        transform.rotation = rotacionAObjetivo;
    }

    private void LanzarAtaque()
    {
        agent.isStopped = true;
        anim.SetBool("attacking", true);
    }

    private void Atacar()  //Se llama en el evento de la animaci�n
    {
        Collider[] colliderTocados = Physics.OverlapSphere(puntoAtaque.position, radioAtaque);
        foreach (Collider coll in colliderTocados)
        {
            if (coll.TryGetComponent(out Daniable danhable))
            {
                danhable.RecibirDanio(danioAtaque);
            }
        }
    }

    private void FinDeAtaque()  //Se llama en el evento de la animaci�n
    {
        agent.isStopped = false;
        anim.SetBool("attacking", false);
    }

    public void RecibirDanio(float danio)
    {
        vidaEnemigo -= danio;
        Debug.Log(vidaEnemigo);
        if (vidaEnemigo <= 0)
        {
            //elay =true;
            agent.isStopped = true;
            Destroy(GetComponent<CapsuleCollider>());
            anim.SetTrigger("died");

            if (GameManager.instance != null)
            {
                GameManager.instance.EnemigoEliminado();
            }
            
            int random = UnityEngine.Random.Range(0, 4);
            Instantiate(GameManager.instance.ObjetosDrop[random],puntoDrop.position,Quaternion.identity);
            StartCoroutine(DelayMuerte());

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }

    private IEnumerator DelayMuerte()
    {
        Debug.Log("Entré a la corrutina");
        yield return new WaitForSeconds(5f);
        //delay = false;
        Destroy(this.gameObject);
    }
}
