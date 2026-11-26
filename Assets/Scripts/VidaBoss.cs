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
    bool estaMuerto = false;

    void Start()
    {
        ActualizarHUD();
    }

    public void RecibirDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            vida = 0;
            Morir();
        }

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
    }
}