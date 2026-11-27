using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VidaBoss : MonoBehaviour
{
    public int vida = 1000;
    public int vidaMaxima = 1000;
    public Slider barraVida;
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoPuntacion;
    public float tiempoMuerte = 2f;
    public GameObject panelVictoria;
    public PlayerController jugador;

    bool estaMuerto = false;

    void Start()
    {
        panelVictoria.SetActive(false);
        textoPuntacion.text = DatosPartida.puntacion.ToString();
        ActualizarHUD();
    }

    void Update()
    {
        if (estaMuerto && !panelVictoria.activeSelf)
        {
            tiempoMuerte -= Time.deltaTime;

            if (tiempoMuerte <= 0)
            {
                panelVictoria.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void RecibirDano(int dano)
    {
        if (estaMuerto)
        {
            return;
        }

        vida -= dano;

        if (vida <= 0)
        {
            vida = 0;
            DatosPartida.puntacion += 500;
            Morir();
        }

        textoPuntacion.text = DatosPartida.puntacion.ToString();
        ActualizarHUD();
    }

    void ActualizarHUD()
    {
        barraVida.maxValue = vidaMaxima;
        barraVida.value = vida;
        textoVida.text = vida + "/" + vidaMaxima;
    }

    void Morir()
    {
        estaMuerto = true;

        GetComponent<Animator>().SetTrigger("muerto");
        GetComponent<BossController>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        jugador.BloquearJugador();
    }
}   