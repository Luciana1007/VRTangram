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
        grabInteractable.selectExited.AddListener(AlSoltar);
    }

     void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectExited.RemoveListener(AlSoltar);
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        if (colocadaCorrectamente)
            return;
        
        ValidarPieza(); 
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

        controlador.RegistrarPiezaCorrecta();
    }

    public void ReiniciarPieza()
    {
        colocadaCorrectamente = false;

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
            grabInteractable.enabled = false;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}
