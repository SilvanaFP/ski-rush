using UnityEngine;

public class GearGameManager : MonoBehaviour
{
    public DraggableGear[] gears;
    public GameObject winPanel;

    public void CheckWin()
    {
        foreach (DraggableGear gear in gears)
        {
            if (!gear.isPlaced)
            {
                return;
            }
        }

        winPanel.SetActive(true);
        Debug.Log("Minijoc completat!");
        Time.timeScale = 0f;
    }
}