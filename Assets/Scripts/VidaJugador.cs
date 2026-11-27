using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VidaJugador : MonoBehaviour
{
    public int vida = 50;
    public int vidaMaxima = 50;
    public Slider barraVida;
    public TextMeshProUGUI  textoVida;
    public GameObject panelPuntuacion;
    bool estaMuerto = false;
    public GameObject panelDerrota;
    public float probabilidadEsquiva = 0.15f;
    public float tiempoInvulnerabilidad = 0.5f;
    float temporizadorInvulnerabilidad = 0f;

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

    void Update()
    {
        if (temporizadorInvulnerabilidad > 0)
        {
            temporizadorInvulnerabilidad -= Time.deltaTime;
        }
    }
    public void RecibirDano(int dano)
    {
        if (estaMuerto || temporizadorInvulnerabilidad > 0)
        {
            return;
        }

        if (Random.value < probabilidadEsquiva)
        {
            Debug.Log("Ataque esquivado");
            return;
        }

        vida -= dano;
        temporizadorInvulnerabilidad = tiempoInvulnerabilidad;

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
        vidaMaxima += 10;
        vida += 10;
        ActualizarHUD();
    }

    public void RecuperarVida()
    {
        vida = Mathf.Min(vidaMaxima, vida + 10);
        ActualizarHUD();
    }
}
