using UnityEngine;
using TMPro;

public class MenuJuego : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelPrincipal;
    public GameObject panelCronometro;
    public GameObject panelInstrucciones;
    public GameObject panelResultado;

    [Header("Textos")]
    public TMP_Text textoCronometro;
    public TMP_Text textoBotonPausar;
    public TMP_Text textoResultado;

    [Header("Controlador")]
    public ControladorTangram controladorTangram;

    [Header("Tiempo")]
    public float tiempoLimite = 180f;
    
    private float tiempoRestante;
    private bool cronometroActivo = false;
    private bool partidaFinalizada = false;

    // Update is called once per frame
    void Update()
    {
        if (!cronometroActivo || partidaFinalizada)
            return;
        
        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            ActualizarTextoTiempo();

            PerderJuego();
            return;
        }
        
        ActualizarTextoTiempo();
    }

    private void ActualizarTextoTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);

        textoCronometro.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public void EmpezarJuego()
    {
        panelPrincipal.SetActive(false);
        panelInstrucciones.SetActive(false);
        panelResultado.SetActive(false);
        panelCronometro.SetActive(true);

        partidaFinalizada = false;
        tiempoRestante = tiempoLimite;

        cronometroActivo = true;

        ActualizarTextoTiempo();
        textoBotonPausar.text = "PAUSAR";

        controladorTangram.ReiniciarTangram();
    }

    public void MostrarInstrucciones()
    {
        panelPrincipal.SetActive(false);
        panelInstrucciones.SetActive(true);
    }

    public void VolverMenu()
    {
        panelInstrucciones.SetActive(false);
        panelPrincipal.SetActive(true);
    }

    public void PausarCronometro()
    {
        if (partidaFinalizada)
            return;
        
        cronometroActivo = !cronometroActivo;

        textoBotonPausar.text = cronometroActivo ? "Pausar" : "Continuar";
    }

    public void ReiniciarCronometro()
    {
        if (partidaFinalizada)
            return;
        
        tiempoRestante = tiempoLimite;
        cronometroActivo = true;

        ActualizarTextoTiempo();
        textoBotonPausar.text = "PAUSAR";

        controladorTangram.ReiniciarTangram();
    }

    public void GanarJuego()
    {
        if (partidaFinalizada)
            return;
        
        partidaFinalizada = true;
        cronometroActivo = false;

        panelCronometro.SetActive(false);
        panelResultado.SetActive(true);

        textoResultado.text = "¡FELICIDADES!\n¡Completaste el tangram!";

        controladorTangram.BloquearTodasLasPiezas();
    }

    public void PerderJuego()
    {
        if (partidaFinalizada)
            return;
        
        partidaFinalizada = true;
        cronometroActivo = false;

        panelCronometro.SetActive(false);
        panelResultado.SetActive(true);

        textoResultado.text = "¡SE ACABÓ EL TIEMPO!\nIntentalo nuevamente.";
    }

    public void Reintentar()
    {
        cronometroActivo = false;
        partidaFinalizada = false;

        panelResultado.SetActive(false);
        panelCronometro.SetActive(false);
        panelPrincipal.SetActive(true);

        controladorTangram.ReiniciarTangram();
    }
}
