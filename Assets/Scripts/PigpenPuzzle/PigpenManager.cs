using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.CodeDom.Compiler;

public class PigpenManager : PuzzleManager
{
    [SerializeField] private bool isInteractor;

    //public static PigpenManager instance = null;
    [SerializeField] List<GameObject> dragBox;
    [SerializeField] List<GameObject> dragBoxPos;
    [SerializeField] GameObject pigpenPrefab;
    [SerializeField] int ansBoxes = 4;
    //int alphabetBoxes = 6;

    private void Start()
    {
        BoxDrag[] tempBox = GetComponentsInChildren<BoxDrag>();
        for(int i = 0; i < tempBox.Length; i++)
        {
            dragBox.Add(tempBox[i].gameObject);
        }
    }

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
        int counter = 0;
        foreach (GameObject boxDrag in dragBox)
        {
            if(boxDrag.GetComponent<BoxDrag>().BoxIndex == boxDrag.GetComponent<BoxDrag>().SlotIndex)
            {
                counter++;
            }

            if(boxDrag.GetComponent<BoxDrag>().BoxIndex != boxDrag.GetComponent<BoxDrag>().SlotIndex)
            {
                Debug.Log("Wrong Answer");
                Debug.Log(" Box " + boxDrag.GetComponent<BoxDrag>().BoxIndex + " Slot " + boxDrag.GetComponent<BoxDrag>().SlotIndex);
                foreach (GameObject dragBoxPosition in dragBoxPos)
                {
                    dragBoxPosition.GetComponent<BoxDrag>().ResetPosition();
                }
            }
        }
        if (counter >= ansBoxes) // checkwin
        {
            _puzzleModule.ModuleComplete();
        }
    }
}
