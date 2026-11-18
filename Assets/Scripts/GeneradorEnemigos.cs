using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject enemigo;
    public Transform jugador;
    public float tiempoGeneracion;
    float temporizador;
    public Transform[] puntos;
    void Start()
    {

    }
    void Update()
    {
        temporizador -= Time.deltaTime;

        if(temporizador <= 0)
        {
            GenerarEnemigos();
            temporizador = tiempoGeneracion;
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
