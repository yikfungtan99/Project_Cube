using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSlots : MonoBehaviour
{
    public CipherPuzzleManager cpm;
    [SerializeField] Material alphabetMat;
    MeshRenderer meshRenderer;
    public bool slotActive;
    //public TextMeshPro slot;

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        cpm = GetComponentInParent<CipherPuzzleManager>();
        meshRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();
        //slot = GetComponentInChildren<TextMeshPro>();
    }

    public void SetSlot(string s)
    {
        //slot.SetText(s.ToUpper());
        alphabetMat = Resources.Load<Material>("CipherAlphabetMaterials/" + s.ToUpper());
        meshRenderer.material = alphabetMat;
    }

    public void ClearSlot()
    {
        //slot.SetText("-");
        alphabetMat = Resources.Load<Material>("CipherAlphabetMaterials/_LetterSlotTexture");
        meshRenderer.material = alphabetMat;
    }

    public void ChangeSlotState(bool b) // reminder to replace code inside with actual animations or something
    {
        transform.GetChild(0).transform.gameObject.SetActive(b);
        if(b == true)
        {
            slotActive = false;    
        }
        else
        {
            slotActive = true;
        }
    }
}
