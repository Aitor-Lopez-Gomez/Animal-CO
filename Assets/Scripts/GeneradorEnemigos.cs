using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject enemigo;
    public Transform jugador;
    public float tiempoGeneracion;
    public int oleadaActual = 1;
    public float duracionOleada = 30f;
    float temporizadorOleada;
    float temporizador;
    public Transform[] puntos;
    void Start()
    {
        temporizadorOleada = duracionOleada;
    }
    void Update()
    {
        temporizador -= Time.deltaTime;

        if(temporizador <= 0)
        {
            GenerarEnemigos();
            temporizador = tiempoGeneracion;
        }

        temporizadorOleada -= Time.deltaTime;

        if(temporizadorOleada <= 0)
        {
            if (oleadaActual >= 20)
            {
                Debug.Log("¡Has completado las 20 oleadas!");
                enabled=false;
                return;
            }
            oleadaActual++;
            temporizadorOleada = duracionOleada;
            if (oleadaActual <= 10)
            {
                tiempoGeneracion = Mathf.Max(0.5f, tiempoGeneracion - 0.05f);
            }
            else
            {
                tiempoGeneracion = Mathf.Max(0.5f, tiempoGeneracion - 0.20f);
            }
            Debug.Log("Oleada " + oleadaActual + " Tiempo generacion - " + tiempoGeneracion);
        }
    }
    // Update is called once per frame
    void GenerarEnemigos()
    {
        int aleatorio = Random.Range(0, puntos.Length);
        GameObject nuevoEnemigo = Instantiate(enemigo, puntos[aleatorio].position, Quaternion.identity);
        nuevoEnemigo.GetComponent<EnemigoController>().jugador = jugador;  
    }


}
