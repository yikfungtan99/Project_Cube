using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Vector3 up = Vector3.zero,
    right = new Vector3(0, 90, 0),
    down = new Vector3(0, 180, 0),
    left = new Vector3(0, 270, 0),
    CurrentDirection = Vector3.zero;

    Vector3 nextPos, destination, direction;
    public float speed = 5f;
    float rayLength = 1f;
    bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        CurrentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public void MoveUp()
    {
            nextPos = Vector3.forward;
            CurrentDirection = up;
            canMove = true;
      

        if(Vector3.Distance(destination,transform.position)<= 0.00001f)
        {
            transform.localEulerAngles = CurrentDirection;
            if(canMove)
            {
                if (PositionValid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }
                
            }
            
        }
    }

    public void MoveLeft()
    {

        
        nextPos = Vector3.left;
        CurrentDirection = left;
        canMove = true;

        if (Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = CurrentDirection;
            if (canMove)
            {
                if (PositionValid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }

            }

        }
    }

    public void MoveRight()
    {
        nextPos = Vector3.right;
        CurrentDirection = right;
        canMove = true;

        if (Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = CurrentDirection;
            if (canMove)
            {
                if (PositionValid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }

            }

        }
    }

    public void MoveDown()
    {
        nextPos = Vector3.back;
        CurrentDirection = down;
        canMove = true;

        if (Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = CurrentDirection;
            if (canMove)
            {
                if (PositionValid())
                {
                    destination = transform.position + nextPos;
                    direction = nextPos;
                    canMove = false;
                }

            }

        }
    }

    bool PositionValid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);
        RaycastHit hit;

        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);

        if(Physics.Raycast(myRay,out hit, rayLength))
        {
            if(hit.collider.tag == "Wall")
            {
                return false;
            }
        }
        return true;
    }
        
}
