using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ValidadorPieza : MonoBehaviour
{
    [Header("Referencia")]
    public Transform objetivo;

    [Header("Configuracion")]
    public float distanciaMaxima = 0.05f;
    public float anguloMaximo = 20f;

    [Header("Componentes")]
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;

    [Header("Controlador")]
    public ControladorTangram controlador;

    private bool colocadaCorrectamente = false;
    private bool validacionActiva = false;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    void Awake()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;

        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }
    
    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.selectExited.AddListener(AlSoltar);
    }

     void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectExited.RemoveListener(AlSoltar);
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        if (colocadaCorrectamente || !validacionActiva)
            return;
        
        ValidarPieza(); 
    }


   public void ActivarValidacion()
   {
        validacionActiva = true;
   }

   public void DesactivarValidacion()
   {
        validacionActiva = false;
   }

   public void ValidarPieza()
   {
        float distancia = Vector3.Distance(transform.position, objetivo.position);

        float angulo = Quaternion.Angle(transform.rotation, objetivo.rotation);

        if (distancia <= distanciaMaxima && angulo <= anguloMaximo)
        {
            EncajarPieza();
        }
    }

    private void EncajarPieza()
    {
        colocadaCorrectamente = true;

        transform.SetPositionAndRotation(objetivo.position, objetivo.rotation);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        grabInteractable.enabled = false;

        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirSonidoColocarPieza();
        }

        controlador.RegistrarPiezaCorrecta();
    }

    public void HabilitarPieza()
    {
        if (colocadaCorrectamente)
            return;
        
        if (rb != null)
            rb.isKinematic = false;

        if (grabInteractable != null)
            grabInteractable.enabled = true; 
    }

    public void ReiniciarPieza()
    {
        colocadaCorrectamente = false;

        validacionActiva = false;

        grabInteractable.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.SetPositionAndRotation(posicionInicial, rotacionInicial);

    }

    public void BloquearPieza()
    {

        if (grabInteractable != null)
        {
            if (grabInteractable.interactionManager != null)
            {
                for (int i = grabInteractable.interactorsSelecting.Count - 1; i >= 0; i--)
                {
                    var interactor = grabInteractable.interactorsSelecting[i];

                    grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);
                }
            }

            grabInteractable.enabled = false;

        }    
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    public bool EstaColocada()
    {
        return colocadaCorrectamente;
    }
}
