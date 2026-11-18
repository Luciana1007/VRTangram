using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControladorTangram : MonoBehaviour
{
    [Header("Piezas del tangram")]
    public ValidadorPieza[] piezas;
    
    [Header("Las 3 piezas escondidas")]
    public ValidadorPieza[] piezasEscondidas;

    [Header("Control Juego")]
    public MenuJuego menuJuego;

    private int piezasCorrectas = 0;
    private int piezasEncontradas = 0;
    private bool puzzleCompletado = false;
    private bool busquedaCompletada = false;
    private bool[] piezasYaEncontradas;

    void Start()
    {
        piezasYaEncontradas = new bool[piezasEscondidas.Length];

        foreach (ValidadorPieza pieza in piezasEscondidas)
        {
            if (pieza != null && pieza.grabInteractable != null)
                pieza.grabInteractable.selectEntered.AddListener(AlAgarrarPieza);
        }
    }

    void OnDestroy()
    {
        foreach (ValidadorPieza pieza in piezasEscondidas)
        {
            if (pieza != null && pieza.grabInteractable != null)
                pieza.grabInteractable.selectEntered.RemoveListener(AlAgarrarPieza);
        }
    }
    
    public void IniciarBusqueda()
    {
        piezasCorrectas = 0;
        piezasEncontradas = 0;

        puzzleCompletado = false;
        busquedaCompletada = false;

        piezasYaEncontradas = new bool[piezasEscondidas.Length];

        foreach (ValidadorPieza pieza in piezas)
        {
            pieza.ReiniciarPieza();
            pieza.BloquearPieza();
            pieza.DesactivarValidacion();
        }

        foreach (ValidadorPieza pieza in piezasEscondidas)
        {
            pieza.HabilitarPieza();
        }
        Debug.Log("Comienza la busqueda de 3 piezas");
    }

    public void AlAgarrarPieza(SelectEnterEventArgs args)
    {
        if (busquedaCompletada)
            return;

        for (int i = 0; i < piezasEscondidas.Length; i++)
        {
            if ((UnityEngine.Object)args.interactableObject == piezasEscondidas[i].grabInteractable)
            {
                
                if (piezasYaEncontradas[i])
                    return;

                piezasYaEncontradas[i] = true;
                piezasEncontradas++;

                
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.ReproducirSonidoEncontrarPieza();
                }

                Debug.Log("Piezas encontradas: " +
                    piezasEncontradas + "/3");

                if (piezasEncontradas >= piezasEscondidas.Length)
                {
                    CompletarBusqueda();
                }

                return;
            }
        }
    }
    
    public void CompletarBusqueda()
    {
        busquedaCompletada = true;

        Debug.Log("¡Se encontraron las 3 piezas!");

        
        foreach (ValidadorPieza pieza in piezas)
        {
            pieza.ActivarValidacion();
            pieza.HabilitarPieza();
        }

        
        menuJuego.IniciarCronometro();
    }
    
    public void RegistrarPiezaCorrecta()
    {
        if (puzzleCompletado)
            return;

        piezasCorrectas++;

        Debug.Log("Piezas colocadas: " + piezasCorrectas + "/7");

        if (piezasCorrectas >= piezas.Length)
        {
            puzzleCompletado = true;

            Debug.Log("Tangram Completado!!");

            menuJuego.GanarJuego();
        }
    }

    public void ReiniciarTangram()
    {
        piezasCorrectas = 0;
        puzzleCompletado = false;

        foreach (ValidadorPieza pieza in piezas)
        {
            bool conservarPosicion = false;

            for (int i = 0; i < piezasEscondidas.Length; i++)
            {
                if (pieza == piezasEscondidas[i] && piezasYaEncontradas[i])
                {
                    conservarPosicion = true;
                    break;
                }
            }

            pieza.ReiniciarPieza(conservarPosicion);
            pieza.ActivarValidacion();
        }
    }

    public void BloquearTodasLasPiezas()
    {
        foreach (ValidadorPieza pieza in piezas)
        {
            pieza.BloquearPieza();
        }
    }

    public void HabilitarPiezasNoColocadas()
    {
        foreach (ValidadorPieza pieza in piezas)
        {
            if (!pieza.EstaColocada())
                pieza.HabilitarPieza();
        }
    }
}
