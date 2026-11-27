using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    public float speed;
    public Transform jugador;
    Vector3 movimiento;
    Animator animator;
    public float tiempoAtaque = 0;
    public int dano = 3;
    public float distanciaCaja;
    public Vector2 tamanno;
    public LayerMask capaJugador;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dano = 3 + ((DatosPartida.oleadaActual - 1) / 3) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        Animaciones();
        Ataque();
    }
    void Movimiento()
    {
        movimiento = jugador.position - transform.position;
        movimiento.Normalize();
    }
    void Animaciones()
    {
        if (movimiento.y > movimiento.x && movimiento.y > -movimiento.x)
        {
            animator.SetInteger("direccion", 0);
        }
        else if (movimiento.y < movimiento.x && movimiento.y < -movimiento.x)
        {
            animator.SetInteger("direccion", 1);

        }
        else if (movimiento.x < 0)
        {
            animator.SetInteger("direccion", 2);
        }
        else if (movimiento.x > 0)
        {
            animator.SetInteger("direccion", 3);
        }
    }
    void Ataque()
    {
        Vector2 vector = Vector2.up;
        int direccion = animator.GetInteger("direccion");

        if(direccion == 1)
        {
            vector = Vector2.down;
        } 
        else if(direccion == 2)
        {
            vector = Vector2.left;
        }
        else if(direccion == 3)
        {
            vector = Vector2.right;
        }
        
        RaycastHit2D rc = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, tamanno, 0, vector, distanciaCaja, capaJugador);
        if(rc)
        {
            tiempoAtaque -= Time.deltaTime;
            movimiento = Vector3.zero;
            if (tiempoAtaque <= 0){
                animator.SetBool("atacando", true);
                rc.collider.GetComponent<VidaJugador>().RecibirDano(dano);
                tiempoAtaque = 1f;
            } 
            else if (tiempoAtaque <= 0.5)
            {
                animator.SetBool("atacando", false);
            }
            
        } 
        else
        {
            animator.SetBool("atacando", false);
        }
    }
    void OnDrawGizmos(){
        Vector3 vector = Vector3.up;

        if (animator != null)
        {
            int direccion = animator.GetInteger("direccion");

            if (direccion == 1)
            {
                vector = Vector3.down;
            }
            else if (direccion == 2)
            {
                vector = Vector3.left;
            }
            else if (direccion == 3)
            {
                vector = Vector3.right;
            }
        }

        Gizmos.color = Color.red;
        Vector3 centro = GetComponent<Collider2D>().bounds.center;
        Gizmos.DrawWireCube(centro + vector * distanciaCaja, tamanno);
    }
    void FixedUpdate(){
        transform.position += movimiento * speed * Time.deltaTime;
    }
}
