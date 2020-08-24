using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBall : MonoBehaviour
{
    private Vector3 targetPos;
    public float speed = 6;
    void Start()
    {
        targetPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);
    }

    public void SetTarget(GameObject target)
    {
        targetPos = target.gameObject.transform.localPosition;
    }
}
