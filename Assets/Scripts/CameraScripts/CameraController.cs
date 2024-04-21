using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum Rotation
    {
        Null = 0,
        MouseX = 1,
        MouseY = 2
    }

    public Rotation rotation = Rotation.Null;
    public float speedHor = 50.0f;
    public float speedVert = 50.0f;
    public float maxVert = 50.0f;
    public float minVert = -25.0f;
    private float axisX = 0;

    private void Update()
    {
        if(rotation==Rotation.MouseX)
        {
            if(Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, speedHor * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, -speedHor * Time.deltaTime, 0);
            }
        }
        if (rotation == Rotation.MouseY)
        {
            //if (Input.GetKey(KeyCode.W) && maxVert >= transform.rotation.x)
            //{
            //    transform.Rotate(speedVert * Time.deltaTime,0,0);
            //}
            if (Input.GetKey(KeyCode.W))
            {
                axisX += speedVert * Time.deltaTime;
                axisX = Mathf.Clamp(axisX, minVert, maxVert);
                transform.localEulerAngles = new Vector3(axisX, transform.localEulerAngles.y, 0);
                //transform.Rotate(speedVert * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.S) && minVert <= transform.rotation.x)
            {
                axisX -= speedVert * Time.deltaTime;
                axisX = Mathf.Clamp(axisX, minVert, maxVert);
                transform.localEulerAngles = new Vector3(axisX, transform.localEulerAngles.y, 0);
                //transform.Rotate(-speedVert * Time.deltaTime,0,0);
            }


        }
    }
}
