using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de audio")]
    public AudioSource musicaFondo;
    public AudioSource efectosSonido;

    [Header("Musica de fondo")]
    public AudioClip musicaJuego;

    [Header("Efectos de sonido")]
    public AudioClip sonidoEncontrarPieza;
    public AudioClip sonidoColocarPieza;
    public AudioClip sonidoBoton;
    public AudioClip sonidoVictoria; 
    public AudioClip sonidoDerrota;

    [Header("Volumen")]
    [Range(0f, 1f)]
    public float volumenMusica = 0.3f;
    
    [Range(0f, 1f)]
    public float volumenEfectos = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicaFondo = gameObject.AddComponent<AudioSource>();
        efectosSonido = gameObject.AddComponent<AudioSource>();

        musicaFondo.playOnAwake = false;
        musicaFondo.loop = true;
        musicaFondo.spatialBlend = 0f;

        efectosSonido.playOnAwake = false;
        efectosSonido.loop = false;
        efectosSonido.spatialBlend = 0f;

        musicaFondo.volume = volumenMusica;
        efectosSonido.volume = volumenEfectos;
    }

    public void ReproducirMusica()
    {
        if (musicaJuego == null)
            return;

        musicaFondo.clip = musicaJuego;
        musicaFondo.Play();
    }

    public void ReproducirSonidoEncontrarPieza()
    {
        if (sonidoEncontrarPieza != null)
            efectosSonido.PlayOneShot(sonidoEncontrarPieza);
    }

    public void ReproducirSonidoColocarPieza()
    {
        if (sonidoColocarPieza != null)
            efectosSonido.PlayOneShot(sonidoColocarPieza);
    }

    public void ReproducirSonidoBoton()
    {
        if (sonidoBoton != null)
            efectosSonido.PlayOneShot(sonidoBoton);
    }

    // NUEVO: sonido de victoria
    public void ReproducirSonidoVictoria()
    {
        if (sonidoVictoria != null)
            efectosSonido.PlayOneShot(sonidoVictoria);
    }

    // NUEVO: sonido de derrota
    public void ReproducirSonidoDerrota()
    {
        if (sonidoDerrota != null)
            efectosSonido.PlayOneShot(sonidoDerrota);
    }
}