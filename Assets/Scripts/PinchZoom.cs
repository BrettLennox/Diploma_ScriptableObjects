using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = .5f;
    public float orthoZoomSpeed = 0.5f;

    private Camera camera;

    private void Start()
    {
        camera ??= GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;

            if (camera.orthographic)
            {
                camera.orthographicSize += deltaMagnitudediff * orthoZoomSpeed;
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, .1f);
            }
            else
            {
                camera.fieldOfView += deltaMagnitudediff * perspectiveZoomSpeed;
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, .1f, 179.9f);
            }
        }
    }
}
