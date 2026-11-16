using UnityEngine;

public class UnirModelos : MonoBehaviour
{
    public MeshFilter modelo1;
    public MeshFilter modelo2;

    [ContextMenu("Unificar modelos")]
    void Unificar()
    {
        MeshFilter[] modelos = { modelo1, modelo2 };

        CombineInstance[] combinaciones = new CombineInstance[2];

        for (int i = 0; i < modelos.Length; i++)
        {
            combinaciones[i].mesh = modelos[i].sharedMesh;
            combinaciones[i].transform = transform.worldToLocalMatrix * modelos[i].transform.localToWorldMatrix;
        }

        Mesh nuevaMalla = new Mesh();
        nuevaMalla.CombineMeshes(combinaciones);

        MeshFilter filtro = GetComponent<MeshFilter>();
        if (filtro == null)
            filtro = gameObject.AddComponent<MeshFilter>();
        
        filtro.sharedMesh = nuevaMalla;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer == null)
            renderer = gameObject.AddComponent<MeshRenderer>();
        
        renderer.material = modelo1.GetComponent<MeshRenderer>().sharedMaterial;

        modelo1.gameObject.SetActive(false);
        modelo1.gameObject.SetActive(false);

    }
}
