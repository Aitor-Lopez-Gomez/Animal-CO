using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaJugador : MonoBehaviour
{
    public int vida = 100;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SalaJefe")
        {
            vida = DatosPartida.vidaJugador;
          
        }  
    }
    public void RecibirDano(int dano){
        vida -= dano;
        if (vida <= 0){
            vida = 0;
            Debug.Log("Has perdido");
        }
    }
}
