using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour, Daniable
{
    private NavMeshAgent agent;
    private Player target;
    private Animator anim;

    [Header("Sistema de combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danioAtaque;
    private float vidaEnemigo = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = FindAnyObjectByType<Player>();
        agent.SetDestination(target.transform.position);
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

    private void Atacar()  //Se llama en el evento de la animación
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

    private void FinDeAtaque()  //Se llama en el evento de la animación
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
            agent.isStopped = true;
            anim.SetTrigger("died");
            //Destroy(this.gameObject);
        }
    }
}
