using UnityEngine;

public class ChunkRetrack : MonoBehaviour
{
    public void ResetBonys()
    {
        SnowBump[] bonys = GetComponentsInChildren<SnowBump>(true);

        foreach (SnowBump bony in bonys)
        {
            bony.ResetBony();
        }
    }
}