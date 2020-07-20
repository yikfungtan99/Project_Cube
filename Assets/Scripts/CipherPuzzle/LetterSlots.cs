using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSlots : MonoBehaviour
{
    public CipherPuzzleManager cpm;
    public TextMeshPro slot;
    public bool slotActive;
    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        cpm = GetComponentInParent<CipherPuzzleManager>();
        slot = GetComponentInChildren<TextMeshPro>();
    }

    public void SetActiveSlot(bool b)
    {
        slotActive = b;
        ChangeSlotState();
    }

    public void SetSlot(string s)
    {
        slot.SetText(s.ToUpper());
    }

    public void ClearSlot()
    {
        slot.SetText("-");
    }

    private void ChangeSlotState() // reminder to replace code inside with actual animations or something
    {
        if(slotActive)
        {
            transform.GetChild(0).transform.gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).transform.gameObject.SetActive(true);
        }
    }
   
}
