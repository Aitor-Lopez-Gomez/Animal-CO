using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int vida = 100;
    // Start is called before the first frame update
    public void RecibirDano(int dano){
        vida -= dano;
        if (vida <= 0){
            vida = 0;
            Debug.Log("Has perdido");
        }
    }
}
