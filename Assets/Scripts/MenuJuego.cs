using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    
    [Header("Botones con sonido")]
    public Button[] botonesConSonido;

    private float tiempoRestante;
    private bool cronometroActivo = false;
    private bool partidaFinalizada = false;

    void Start()
    {
        controladorTangram.BloquearTodasLasPiezas();

        panelCronometro.SetActive(false);

        
        if (AudioManager.Instance != null)
        AudioManager.Instance.ReproducirMusica();
        
    }
    
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

        textoCronometro.text =
            "Tiempo restante: " +
            minutos.ToString("00") + ":" +
            segundos.ToString("00");
    }

    public void EmpezarJuego()
    {
        panelPrincipal.SetActive(false);
        panelInstrucciones.SetActive(false);
        panelResultado.SetActive(false);

        panelCronometro.SetActive(true);

        textoCronometro.gameObject.SetActive(true);
        textoCronometro.text =
            "Cuando se encuentren las 3 piezas escondidas, " +
            "el cronómetro se activará.";

        textoBotonPausar.gameObject.SetActive(false);

        partidaFinalizada = false;
        cronometroActivo = false;

        

    }

    public void IniciarCronometro()
    {
        tiempoRestante = tiempoLimite;
        cronometroActivo = true;

        panelCronometro.SetActive(true);

        ActualizarTextoTiempo();

        textoBotonPausar.gameObject.SetActive(true);
        textoBotonPausar.text = "PAUSAR";
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

        if (cronometroActivo)
        {
            controladorTangram.HabilitarPiezasNoColocadas();
            textoBotonPausar.text = "PAUSAR";
        }
        else
        {
            controladorTangram.BloquearTodasLasPiezas();
            textoBotonPausar.text = "CONTINUAR";
        }
    }

    public void ReiniciarCronometro()
    {
        if (partidaFinalizada)
            return;

        tiempoRestante = tiempoLimite;
        cronometroActivo = true;

        ActualizarTextoTiempo();

        textoBotonPausar.gameObject.SetActive(true);
        textoBotonPausar.text = "PAUSAR";

        controladorTangram.ReiniciarTangram();
    }

    public void GanarJuego()
    {
        if (partidaFinalizada)
            return;

        partidaFinalizada = true;
        
        cronometroActivo = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.ReproducirSonidoVictoria();

        panelCronometro.SetActive(false);
        
        panelResultado.SetActive(true);

        textoResultado.text =
            "¡FELICIDADES!\n¡Completaste el tangram!";

        controladorTangram.BloquearTodasLasPiezas();
    }

    public void PerderJuego()
    {
        if (partidaFinalizada)
            return;

        partidaFinalizada = true;
        
        cronometroActivo = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.ReproducirSonidoDerrota();

        panelCronometro.SetActive(false);
        
        panelResultado.SetActive(true);

        textoResultado.text =
            "¡SE ACABÓ EL TIEMPO!\nIntentalo nuevamente.";

        controladorTangram.BloquearTodasLasPiezas();
    }

    public void Reintentar()
    {
        cronometroActivo = false;
        partidaFinalizada = false;

        panelResultado.SetActive(false);
        panelCronometro.SetActive(false);
        panelPrincipal.SetActive(true);

        controladorTangram.BloquearTodasLasPiezas();
    }

    private void ReproducirSonidoBoton()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.ReproducirSonidoBoton();
    }
}
