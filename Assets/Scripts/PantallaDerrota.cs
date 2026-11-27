using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaDerrota : MonoBehaviour
{
    public TextMeshProUGUI textoPuntacion;
    public TextMeshProUGUI textoOleada;
    void Start()
    {
        textoPuntacion.text = DatosPartida.oleadaActual.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textoPuntacion.text = DatosPartida.puntacion.ToString();
        textoOleada.text = DatosPartida.oleadaActual.ToString();
    }

    public void Reiniciar(){
        Time.timeScale = 1f;
        DatosPartida.puntacion = 0;
        DatosPartida.oleadaActual = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene("SalaPrincipal");
    }

    public void Salir()
    {
        Application.Quit();
    }


}
