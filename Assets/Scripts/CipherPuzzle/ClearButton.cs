using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    private Animator anim;
    CipherPuzzleManager cpm;
    void Start()
    {
        anim = GetComponent<Animator>();
        cpm = GetComponentInParent<CipherPuzzleManager>();
    }

    private void OnMouseDown()
    {
        print("ClearPressed");
        anim.SetTrigger("isPressed");
        //cpm.ClearSlot();
        cpm.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
