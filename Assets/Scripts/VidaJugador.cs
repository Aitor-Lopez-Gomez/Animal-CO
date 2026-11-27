using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VidaJugador : MonoBehaviour
{
    public int vida = 100;
    public int vidaMaxima = 100;
    public Slider barraVida;
    public TextMeshProUGUI  textoVida;
    public GameObject panelPuntuacion;
    bool estaMuerto = false;
    public GameObject panelDerrota;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SalaJefe")
        {
            vida = DatosPartida.vidaJugador;
            vidaMaxima = DatosPartida.vidaMaxima;
        }

        ActualizarHUD();
        panelDerrota.SetActive(false);
    }
    public void RecibirDano(int dano){
        if (estaMuerto)
        {
            return;
        }
        vida -= dano;
       
        if (vida <= 0)
        {
            vida = 0;
            Morir();

        }
        ActualizarHUD();
    }

    void Morir()
    {
        estaMuerto = true;

        GetComponent<PlayerController>().BloquearJugador();

        if (SceneManager.GetActiveScene().name == "SalaPrincipal")
        {
            FindObjectOfType<GeneradorEnemigos>().enabled = false;
        }

        panelPuntuacion.SetActive(false);
        panelDerrota.SetActive(true);
        Time.timeScale = 0f;
    }

    void ActualizarHUD()
    {
        
        barraVida.maxValue = vidaMaxima;
        barraVida.value = vida;
        textoVida.text = vida + "/" + vidaMaxima;
    }
    
    public void MejorarVida()
    {
        vidaMaxima += 20;
        vida += 20;
        ActualizarHUD();
    }
}
