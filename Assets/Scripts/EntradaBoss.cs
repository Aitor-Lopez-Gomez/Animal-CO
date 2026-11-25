using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntradaBoss : MonoBehaviour
{
    public CanvasGroup panelEntradaBoss;
    public float duracionFundido;
    void Start()
    {
        StartCoroutine(FundidoEntrada());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FundidoEntrada()
    {
        float tiempo = 0;
        panelEntradaBoss.alpha = 1f;
        while (tiempo < duracionFundido)
        {
            tiempo += Time.deltaTime;
            panelEntradaBoss.alpha = 1f - Mathf.Clamp01(tiempo / duracionFundido);
            yield return null;
        }

        panelEntradaBoss.alpha = 0f;
        panelEntradaBoss.gameObject.SetActive(false);
    }
}
