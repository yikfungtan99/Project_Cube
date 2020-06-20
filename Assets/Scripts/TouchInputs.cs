using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputs : MonoBehaviour
{

    private bool _tap, _doubleTap, _swipeLeft, _swipeRight, _swipeUp, _swipeDown;
    
    //double tap
    [SerializeField] private float doubleTapDelta = 0.5f;
    
    private float _lastTap;
    
    //swipe data
    private Vector2 _swipeDelta, _startTouch;
    
    // Start is called before the first frame update
    private void Start()
    {

    }
    
    // Update is called once per frame
    private void Update()
    {
        
    }

    private void UpdateInput()
    {
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                _tap = true;
                _startTouch = Input.mousePosition;
                _doubleTap = Time.time - _lastTap < doubleTapDelta;
                _lastTap = Time.deltaTime;
            }
            
        }
        
    }
}
