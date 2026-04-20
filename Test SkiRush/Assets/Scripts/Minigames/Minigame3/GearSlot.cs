using UnityEngine;
using UnityEngine.EventSystems;

public class GearSlot : MonoBehaviour, IDropHandler
{
    public string acceptedGearID;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DraggableGear gear = eventData.pointerDrag.GetComponent<DraggableGear>();

        if (gear == null) return;

        if (gear.gearID == acceptedGearID)
        {
            gear.SnapToSlot();

            GearGameManager manager = FindAnyObjectByType<GearGameManager>();
            if (manager != null)
            {
                manager.CheckWin();
            }
        }
        else
        {
            gear.ReturnToStart();
        }
    }
}