using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 2f;
    public float distanciaAtaque = 3;
    public int danoAtaque = 20;
    public float tiempoEntreAtaques = 3f;
    float temporizadorAtaque;
    public float tiempoAviso = 0.6f;
    bool atacando = false;
    
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Movimiento();
        Rotacion();
        Ataque();
    }
    void Movimiento()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);
        if (atacando)
        {
            animator.SetBool("movimiento", false);
            return;
        }
        if (distancia > distanciaAtaque)
        {
            transform.position = Vector3.MoveTowards(transform.position,jugador.position,velocidad * Time.deltaTime
            );

            animator.SetBool("movimiento", true);
        }
        else
        {
            animator.SetBool("movimiento", false);
        }
    }

    void Rotacion()
    {
        Vector3 direccion = jugador.position - transform.position;

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angulo - 90f);
    }

    void Ataque()
    {
        temporizadorAtaque -= Time.deltaTime;

        float distancia = Vector2.Distance(
            transform.position, jugador.position);

        if (distancia <= distanciaAtaque &&
            temporizadorAtaque <= 0 && !atacando)
        {
            StartCoroutine(RealizarAtaque());
        }
    }
    IEnumerator RealizarAtaque()
    {
        atacando = true;

        animator.SetBool("movimiento", false);
        animator.SetTrigger("atacando");

        yield return new WaitForSeconds(tiempoAviso);

        float distancia = Vector2.Distance(
            transform.position, jugador.position);

        if (distancia <= distanciaAtaque && enabled)
        {
            jugador.GetComponent<VidaJugador>().RecibirDano(danoAtaque);
        }

        temporizadorAtaque = tiempoEntreAtaques;
        atacando = false;
}
}
