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
    public TextMeshProUGUI textoPuntuacion;
    public GameObject panelPuntuacion;
    bool mejoraElegida = false;
    


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
            jugador.GetComponent<VidaJugador>().textoVida.gameObject.SetActive(false);
            jugador.GetComponent<VidaJugador>().barraVida.gameObject.SetActive(false);
            panelPuntuacion.SetActive(false);
            
            if (temporizadorDescanso <= 0)
            {

                esDescanso = false;
                temporizador = tiempoGeneracion;
                panelDescanso.SetActive(false);
            }
            return;

        }

        temporizador -= Time.deltaTime;
        jugador.GetComponent<VidaJugador>().textoVida.gameObject.SetActive(true);
        jugador.GetComponent<VidaJugador>().barraVida.gameObject.SetActive(true);
        panelPuntuacion.SetActive(true);
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
            DatosPartida.puntacion += 50;
            DatosPartida.vidaJugador += 10;
            jugador.GetComponent<VidaJugador>().RecuperarVida();
            oleadaActual++;
            temporizadorOleada = duracionOleada;
            textoOleada.text = "OLEADA " + oleadaActual; 
            
            
            if (oleadaActual == 20)
            {
                DatosPartida.vidaJugador = jugador.GetComponent<VidaJugador>().vida;
                DatosPartida.vidaMaxima = jugador.GetComponent<VidaJugador>().vidaMaxima;
                DatosPartida.dano = jugador.GetComponent<PlayerController>().dano;
                DatosPartida.velocidad = jugador.GetComponent<PlayerController>().speed;
                jugador.GetComponent<PlayerController>().BloquearJugador();
                panelPuntuacion.SetActive(false);

                textoOleada.gameObject.SetActive(false);
                textoTemporizador.gameObject.SetActive(false);
                jugador.GetComponent<VidaJugador>().textoVida.gameObject.SetActive(false);
                jugador.GetComponent<VidaJugador>().barraVida.gameObject.SetActive(false);
                jugador.GetComponent<VidaJugador>().textoVida.gameObject.SetActive(false);
                jugador.GetComponent<VidaJugador>().barraVida.gameObject.SetActive(false);
                
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
            mejoraElegida = false;
            panelDescanso.SetActive(true);
            
            temporizadorDescanso = tiempoDescanso;
        }
        textoPuntuacion.text = DatosPartida.puntacion.ToString();
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

    public void ElegirVida()
    {
        if (mejoraElegida)
            return;

        jugador.GetComponent<VidaJugador>().MejorarVida();
        mejoraElegida = true;
        temporizadorDescanso = 0f;
    }

    public void ElegirDano()
    {
        if (mejoraElegida)
            return;

        jugador.GetComponent<PlayerController>().MejorarDano();
        mejoraElegida = true;
        temporizadorDescanso = 0f;
    }

    public void ElegirVelocidad()
    {
        if (mejoraElegida)
            return;

        jugador.GetComponent<PlayerController>().MejorarVelocidad();
        mejoraElegida = true;
        temporizadorDescanso = 0f;
    }
    

}
