using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject enemigo;
    public Transform jugador;
    public float tiempoGeneracion;
    float temporizador;
    void Start()
    {
        GenerarEnemigos();
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
        GameObject nuevoEnemigo = Instantiate(enemigo, transform.position, quaternion.identity);
        nuevoEnemigo.GetComponent<EnemigoController>().jugador = jugador;  
    }
}
