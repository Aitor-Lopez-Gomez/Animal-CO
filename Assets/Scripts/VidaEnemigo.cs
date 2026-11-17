using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int vida = 100;
    
    public void RecibirDano(int dano){
        vida -= dano;
        if(vida <= 0){
            Destroy(gameObject);
        }
    }
}
