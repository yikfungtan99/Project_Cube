using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PigpenManager : PuzzleManager
{
    [SerializeField] private bool isInteractor;

    //public static PigpenManager instance = null;
    [SerializeField] List<GameObject> dragBox;
    [SerializeField] List<GameObject> dragBoxPos;
    [SerializeField] GameObject pigpenPrefab;
    int ansBoxes = 4;
    int alphabetBoxes = 6;

    // private void Awake()
    // {
    //     //CheckInstance();
    //     //SpawnPigpen();
    //     //Subscribe to event
    //     //PigpenButton.ansCorrectPass += CompareIndex;
    // }

    // void CheckInstance()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     else if (instance = this)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // void SpawnPigpen()
    // {
    //     GameObject tempGO;
    //     tempGO = Instantiate(pigpenPrefab);
    //     //To get alphabet answer GameObjects
    //     for(int i=0; i<ansBoxes; i++)
    //     {
    //         dragBox.Add(tempGO.transform.GetChild(i).gameObject);
    //     }
    //
    //     //To set alphabet boxes back to its original pos
    //     for(int i=0; i<alphabetBoxes; i++)
    //     {
    //         dragBoxPos.Add(tempGO.transform.GetChild(i).gameObject);
    //     }
    // }

    public void CompareIndex()
    {
        if (!isInteractor) return;
        print("Compare Index");
        foreach (GameObject boxDrag in dragBox)
        {
            if(boxDrag.GetComponent<BoxDrag>().BoxIndex == boxDrag.GetComponent<BoxDrag>().SlotIndex)
            {
                Debug.Log("YOU WIN!");
            }

            if(boxDrag.GetComponent<BoxDrag>().BoxIndex != boxDrag.GetComponent<BoxDrag>().SlotIndex)
            {
                Debug.Log("NO NO");
                Debug.Log(" Box " + boxDrag.GetComponent<BoxDrag>().BoxIndex + " Slot " + boxDrag.GetComponent<BoxDrag>().SlotIndex);
                foreach (GameObject dragBoxPosition in dragBoxPos)
                {
                    dragBoxPosition.GetComponent<BoxDrag>().ResetPosition();
                }
            }
        }
    }
}
