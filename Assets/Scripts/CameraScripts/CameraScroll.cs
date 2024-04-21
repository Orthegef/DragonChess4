using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public Transform childCamera;
    public float speedScroll = 1f;
    public float maxScroll = -2.0f;
    public float minScroll = -20.0f;
    private float posZ = -10.0f;
    private float scrollDelta;
    void Update()
    {
        scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0.0f)
        {
            if (scrollDelta > 0.0f)        //прокрутка колеса миші вгору
            {
                posZ += speedScroll;
                if (posZ > maxScroll)
                    posZ = maxScroll;
                childCamera.localPosition = new Vector3(0, 0, posZ);
            }
            else if (scrollDelta < 0.0f)  //прокрутка колеса миші вниз
            {
                posZ -= speedScroll;
                if (posZ < minScroll)
                    posZ = minScroll;
                childCamera.localPosition = new Vector3(0, 0, posZ);
            }
        }
    }
}
