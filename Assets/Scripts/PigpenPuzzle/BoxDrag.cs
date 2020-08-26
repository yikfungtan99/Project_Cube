using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class BoxDrag : MonoBehaviour
{
    Camera cam;
    Ray ray;
    RaycastHit hit;
    Touch firstTap;

    Vector3 objOffset;
    Vector3 boxOriPos;
    Vector3 screenPoint;
    Vector3 restorePos;

    Transform selectedTransform;
    Transform selectedObj;
    
    [SerializeField] private bool simulateMouse;
    bool isCollidingWithDraggable = false;
    bool isCollidingWithSlot = false;

    //Reference scriptableobject data script
    [SerializeField] BoxData boxData;
    private int slotIndex;

    private CubeState _cubeState;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        _cubeState = GameObject.Find("CubeControl").GetComponent<CubeState>();
    }

    void Update()
    {
        if (_cubeState.currentState != CubeStates.Examine) return;
        DragBox();
    }

    #region Getters
    public int BoxIndex
    {
        get { return (int)boxData.boxIndex; }
    }

    public int SlotIndex
    {
        get { return slotIndex;  }
    }
    #endregion

    void DragBox()
    {
        Vector3 temp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -(cam.transform.position.z));
        Vector3 truePos = cam.ScreenToWorldPoint(temp);
        if (simulateMouse)
        {
            if (Input.GetMouseButtonDown(0)) // on click
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
                if (Physics.Raycast(ray, out hit, 10000.0f))
                {
                    if (hit.collider.CompareTag("Draggable"))
                    {
                        selectedObj = hit.collider.transform;
                        if (selectedObj != null)
                        {
                            StoreBoxOriPos();
                            selectedTransform = selectedObj.transform;
                            screenPoint = Camera.main.WorldToScreenPoint(selectedTransform.position);
                            objOffset = selectedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(
                                Input.mousePosition.x,
                                Input.mousePosition.y,
                                screenPoint.z));
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0) && selectedTransform != null) // on hold
            {
                selectedTransform.position = new Vector3(truePos.x, truePos.y, selectedTransform.position.z);
                Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + objOffset;
                selectedTransform.position = cursorPosition;
            }

            if (Input.GetMouseButtonUp(0)) // on lift
            {
                if (selectedObj != null)
                {
                    if(!isCollidingWithSlot || isCollidingWithDraggable)
                    {
                        ResetPosition();
                    }
                    selectedObj = null;
                    selectedTransform = null;
                }
            }


        }
        else
        {
            if (Input.touchCount > 0)
            {
                firstTap = Input.GetTouch(0);
                ray = Camera.main.ScreenPointToRay(firstTap.position);
                Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    if (firstTap.phase == TouchPhase.Began)
                    {
                        //Check if tag is Draggable
                        if (hit.collider.CompareTag("Draggable"))
                        {
                            selectedObj = hit.collider.transform;
                            if (selectedObj != null)
                            {
                                StoreBoxOriPos();
                                selectedTransform = selectedObj.transform;
                                screenPoint = Camera.main.WorldToScreenPoint(selectedTransform.position);
                                objOffset = selectedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(firstTap.position.x,
                                                                                                        firstTap.position.y,
                                                                                                        screenPoint.z));
                            }

                        }
                    }

                    if (firstTap.phase == TouchPhase.Moved)
                    {
                        if (selectedTransform != null)
                        {
                            Vector3 cursorPoint = new Vector3(firstTap.position.x, firstTap.position.y, screenPoint.z);
                            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + objOffset;
                            selectedTransform.position = cursorPosition;
                        }
                    }

                    if (firstTap.phase == TouchPhase.Ended)
                    {
                        if (selectedObj != null)
                        {
                            if (!isCollidingWithSlot || isCollidingWithDraggable)
                            {
                                ResetPosition();
                            }
                            selectedObj = null;
                            selectedTransform = null;
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Draggable"))
        {
            isCollidingWithDraggable = false;
        }
        if (col.CompareTag("Slots"))
        {
            isCollidingWithSlot = false;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Draggable"))
        {
            if (!isCollidingWithDraggable)
            {
                isCollidingWithDraggable = true;
            }
        }

        if (col.CompareTag("Slots") /*&& firstTap.phase == TouchPhase.Ended*/)
        {
            if (!isCollidingWithSlot)
            {
                isCollidingWithSlot = true;
            }
            this.transform.position = col.transform.position;
            //Getting Slot Index number
            slotIndex = (int)col.GetComponent<DropSlots>().slotData.boxIndex;
            //Debug.Log("I HIT A SLOT" + slotIndex + "MY INDEX " + BoxIndex);
        }
    }

    public void ResetPosition()
    {
        this.transform.position = boxOriPos;
    }

    void StoreBoxOriPos()
    {
        boxOriPos = this.transform.position;
    }

}
