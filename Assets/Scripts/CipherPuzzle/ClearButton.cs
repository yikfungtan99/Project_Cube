using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    CipherPuzzleManager cpm;
    void Start()
    {
        cpm = GetComponentInParent<CipherPuzzleManager>();
    }

    private void OnMouseDown()
    {
        print("ClearPressed");
        //cpm.ClearSlot();
        cpm.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
