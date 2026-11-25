using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
    public VideoPlayer videoCinematica;
    public CanvasGroup panelTransicionBoss;
    public float duracionFundido = 2f;
    


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

                jugador.GetComponent<PlayerController>().BloquearJugador();

                textoOleada.gameObject.SetActive(false);
                textoTemporizador.gameObject.SetActive(false);

                StartCoroutine(TransicionBoss());

                enabled = false;
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

    IEnumerator TransicionBoss()
    {
        float tiempo = 0f;

        videoCinematica.Prepare();

        while (tiempo < duracionFundido)
        {
            tiempo += Time.deltaTime;
            panelTransicionBoss.alpha = Mathf.Clamp01(tiempo / duracionFundido);

            yield return null;
        }

        panelTransicionBoss.alpha = 1f;

        yield return new WaitForSeconds(1f);

        videoCinematica.loopPointReached += FinalizarCinematica;
        videoCinematica.Play();

        yield return new WaitForSeconds(0.3f);

        panelTransicionBoss.alpha = 0f;
    }
    void FinalizarCinematica(VideoPlayer video)
    {
        video.loopPointReached -= FinalizarCinematica;
        SceneManager.LoadScene("SalaJefe");
    }


}
