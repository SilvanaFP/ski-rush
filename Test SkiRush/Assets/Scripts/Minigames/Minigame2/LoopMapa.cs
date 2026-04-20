using UnityEngine;

public class LoopMapa : MonoBehaviour
{
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float alturaChunk = 16f;
    [SerializeField] private float limiteSuperior = 8f;
    [SerializeField] private Transform[] chunks;

    private void Update()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].position += Vector3.up * velocidad * Time.deltaTime;
        }

        for (int i = 0; i < chunks.Length; i++)
        {
            if (chunks[i].position.y >= limiteSuperior)
            {
                Transform chunkMesAbaix = BuscarChunksAbaix();
                chunks[i].position = new Vector3(
                    chunks[i].position.x,
                    chunkMesAbaix.position.y - alturaChunk,
                    chunks[i].position.z
                );
            }
        }
    }

    private Transform BuscarChunksAbaix()
    {
        Transform mesAbaix = chunks[0];

        for (int i = 1; i < chunks.Length; i++)
        {
            if (chunks[i].position.y < mesAbaix.position.y)
            {
                mesAbaix = chunks[i];
            }
        }

        return mesAbaix;
    }
}