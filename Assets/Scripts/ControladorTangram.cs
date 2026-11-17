using UnityEngine;

public class ControladorTangram : MonoBehaviour
{
    [Header("Piezas del tangram")]
    public ValidadorPieza[] piezas;

    [Header("Control Juego")]
    public MenuJuego menuJuego;

    private int piezasCorrectas = 0;
    private bool puzzleCompletado = false;

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
            pieza.ReiniciarPieza();
        }
    }

    public void BloquearTodasLasPiezas()
    {
        foreach (ValidadorPieza pieza in piezas)
        {
            pieza.BloquearPieza();
        }
    }
}
