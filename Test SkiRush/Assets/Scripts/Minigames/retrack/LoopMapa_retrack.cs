using UnityEngine;

public class LoopMapa_retrack : MonoBehaviour
{
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float alturaChunk = 16f;
    [SerializeField] private float limiteInferior = -8f;
    [SerializeField] private Transform[] chunks;

    private void Start()
    {
        if (GameFlowManager.Instance != null)
        {
            MinijocRuntimeConfig config = GameFlowManager.Instance.GetConfigActual();
            velocidad = config.velocitat;

            Debug.Log("Velocitat del mapa retrack configurada a: " + velocidad);
        }
    }

    private void Update()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].position += Vector3.down * velocidad * Time.deltaTime;
        }

        for (int i = 0; i < chunks.Length; i++)
        {
            if (chunks[i].position.y <= limiteInferior)
            {
                Transform chunkMesAmunt = BuscarChunkMesAmunt();

                chunks[i].position = new Vector3(
                    chunks[i].position.x,
                    chunkMesAmunt.position.y + alturaChunk,
                    chunks[i].position.z
                );

                ChunkRetrack chunkRetrack = chunks[i].GetComponent<ChunkRetrack>();
                if (chunkRetrack != null)
                {
                    chunkRetrack.ResetBonys();
                }
            }
        }
    }

    private Transform BuscarChunkMesAmunt()
    {
        Transform mesAmunt = chunks[0];

        for (int i = 1; i < chunks.Length; i++)
        {
            if (chunks[i].position.y > mesAmunt.position.y)
            {
                mesAmunt = chunks[i];
            }
        }

        return mesAmunt;
    }
}