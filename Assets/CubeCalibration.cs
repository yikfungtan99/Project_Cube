using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCalibration : MonoBehaviour
{
    [SerializeField] private Transform cube;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) CalibrateCube();
    }

    private void CalibrateCube()
    {
        Vector3 rotation = cube.transform.rotation.eulerAngles;
        float roundedX = RoundToAngle(rotation.x);
        float roundedY = RoundToAngle(rotation.y);
        float roundedZ = RoundToAngle(rotation.z);
        
        cube.transform.rotation = Quaternion.Euler(roundedX, roundedY, roundedZ);
    }

    private float CloserToValue(float value, float min, float max)
    {
        return value - min > max - value ? max : min;
    }

    private float RoundToAngle(float curAngle)
    {
        float roundedAngle = 0;

        if (curAngle > 0 && curAngle < 90) roundedAngle = CloserToValue(curAngle, 0, 90);
        if (curAngle > 90 && curAngle < 180) roundedAngle = CloserToValue(curAngle, 90, 180);
        if (curAngle > 180 && curAngle < 270) roundedAngle = CloserToValue(curAngle, 180, 270);
        if (curAngle > 270 && curAngle < 360) roundedAngle = CloserToValue(curAngle, 270, 360);

        return roundedAngle;
    }
}
