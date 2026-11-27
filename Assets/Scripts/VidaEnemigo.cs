using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int vida = 100;
    public float tiempoMuerte;
    Animator animator;
    
    public void RecibirDano(int dano){
        if(vida <= 0)
        {
            return;
        }
        vida -= dano;
        if(vida <= 0){
            GetComponent<Animator>().SetBool("muerto", true);
            GetComponent<EnemigoController>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, tiempoMuerte);
            DatosPartida.puntacion += 10;
        }
    }
}
