using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFollow : MonoBehaviour
{
    public Transform jugador;
    public Renderer mapa;
    private Camera camara;
    public float suavidad = 5f;
    // Start is called before the first frame update
    void Start()
    {
        camara = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        float altura  = camara.orthographicSize;
        float anchura = altura * camara.aspect;

        Bounds limites = mapa.bounds;

        float minX = limites.min.x + anchura;
        float maxX = limites.max.x - anchura;

        float minY = limites.min.y + altura;
        float maxY = limites.max.y - altura;

        float x = Mathf.Clamp(jugador.position.x, minX, maxX);
        float y = Mathf.Clamp(jugador.position.y, minY, maxY);
        Vector3 destino = new Vector3(x, y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destino, suavidad * Time.deltaTime);
    }
}   
