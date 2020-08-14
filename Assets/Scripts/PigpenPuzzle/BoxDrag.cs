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

    //Reference scriptableobject data script
    [SerializeField] BoxData boxData;

    private CubeState _cubeState;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        _cubeState = GameObject.Find("CubeControl").GetComponent<CubeState>();
        StoreBoxOriPos();
    }

    void Update()
    {
        if (_cubeState.currentState != CubeStates.Examine) return;
        DragBox();
    }

    int slotIndex;
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
            if (Input.GetMouseButtonDown(0))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
                if (Physics.Raycast(ray, out hit, 10000.0f))
                {
                    if (hit.collider.CompareTag("Draggable"))
                    {
                        selectedObj = hit.collider.transform;
                        restorePos = hit.collider.transform.position;
                        if (selectedObj != null)
                        {
                            selectedTransform = selectedObj.transform;
                            screenPoint = Camera.main.WorldToScreenPoint(selectedTransform.position);
                            objOffset = selectedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(
                                Input.mousePosition.x,
                                Input.mousePosition.y,
                                screenPoint.z));
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0) && selectedObj != null)
                {
                    selectedObj.transform.position = restorePos;
                }
            }

            if (Input.GetMouseButton(0) && selectedObj != null)
            {
                //selectedTransform.position = new Vector3(truePos.x, truePos.y, selectedTransform.position.z);
                Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + objOffset;
                selectedTransform.position = cursorPosition;
               
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
                        selectedTransform = null;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Slots") /*&& firstTap.phase == TouchPhase.Ended*/)
        {
            this.transform.position = col.transform.position;
            //Getting Slot Index number
            slotIndex = (int)col.GetComponent<DropSlots>().slotData.boxIndex;
            //Debug.Log("I HIT A SLOT" + slotIndex + "MY INDEX " + BoxIndex);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = boxOriPos;
    }

    void StoreBoxOriPos()
    {
        boxOriPos = gameObject.transform.position;
    }

}
