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

    public void SetActiveSlot(bool b)
    {
        slotActive = b;
        ChangeSlotState();
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
<<<<<<< Updated upstream
        alphabetMat = Resources.Load<Material>("Materials/_LetterSlotTexture");
=======
        alphabetMat = Resources.Load<Material>("CipherAlphabetMaterials/_LetterSlotTexture");
>>>>>>> Stashed changes
        meshRenderer.material = alphabetMat;
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
