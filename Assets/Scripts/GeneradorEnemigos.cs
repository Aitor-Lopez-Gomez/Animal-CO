using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject enemigo;
    public Transform jugador;
    public float tiempoGeneracion;
    public int oleadaActual = 1;
    public float duracionOleada = 30f;
    float temporizadorOleada;
    float temporizador;
    public float tiempoDescanso;
    float temporizadorDescanso;
    bool esDescanso;
    public Transform[] puntos;
    List<GameObject> enemigosGenerados = new List<GameObject>();
    public TextMeshProUGUI textoOleada;
    public TextMeshProUGUI textoTemporizador;
    public GameObject panelDescanso;
    public TextMeshProUGUI textoCuentaAtras;


    void Start()
    {
        temporizadorOleada = duracionOleada;
        textoOleada.text = "OLEADA " + oleadaActual;
        textoTemporizador.text = temporizadorOleada.ToString();
        panelDescanso.SetActive(false);
        
    }
    void Update()
    {
        if (esDescanso)
        {
            temporizadorDescanso -= Time.deltaTime;
            textoCuentaAtras.text = "" + Mathf.CeilToInt(Mathf.Max(0f, temporizadorDescanso));

            if (temporizadorDescanso <= 0)
            {

                esDescanso = false;
                temporizador = tiempoGeneracion;
                panelDescanso.SetActive(false);
            }
            return;

        }

        temporizador -= Time.deltaTime;

        if(temporizador <= 0)
        {
            GenerarEnemigos();
            temporizador = tiempoGeneracion;
            
        }

        temporizadorOleada -= Time.deltaTime;
        textoTemporizador.text = Mathf.CeilToInt(temporizadorOleada).ToString();
        if(temporizadorOleada <= 0)
        {
            EliminarEnemigos();
            
            oleadaActual++;
            temporizadorOleada = duracionOleada;
            textoOleada.text = "OLEADA " + oleadaActual; 

            
            if (oleadaActual == 20)
            {
                DatosPartida.vidaJugador = jugador.GetComponent<VidaJugador>().vida;
                textoOleada.text = "OLEADA 20 - BOSS";
                textoTemporizador.text = "";

                Debug.Log("Entrando en la sala del jefe final");
                enabled = false;
                SceneManager.LoadScene("SalaJefe");
                return;
            }
            
            if (oleadaActual <= 10)
            {
                tiempoGeneracion = Mathf.Max(0.5f, tiempoGeneracion - 0.05f);
            }
            else
            {
                tiempoGeneracion = Mathf.Max(0.5f, tiempoGeneracion - 0.20f);
            }
            Debug.Log("Oleada " + oleadaActual + " Tiempo generacion - " + tiempoGeneracion);
            jugador.position = new Vector3(6.47f,-5.47f,-2);
            esDescanso = true;
            panelDescanso.SetActive(true);
            temporizadorDescanso = tiempoDescanso;
        }
        
    }
    // Update is called once per frame
    void GenerarEnemigos()
    {
        int aleatorio = UnityEngine.Random.Range(0, puntos.Length);
        GameObject nuevoEnemigo = Instantiate(enemigo, puntos[aleatorio].position, Quaternion.identity);
        nuevoEnemigo.GetComponent<EnemigoController>().jugador = jugador;  
        enemigosGenerados.Add(nuevoEnemigo);
    }

    void EliminarEnemigos()
    {
        foreach (GameObject enemigoCreado in enemigosGenerados)
        {
            if (enemigoCreado != null)
            {
                Destroy(enemigoCreado);
            }
        }
        enemigosGenerados.Clear();
    }


}
