using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotaSpeed = 100.0f;
    public float mouseSensitivity = 3.0f;
    public bool RightClickToDrag = false;


    // Update is called once per frame
    void Update()
    {
        float currentMoveSpeed = moveSpeed;
        float left_right = 0.0f;
        float front_back = 0.0f;
        float up_down = 0.0f;
        float hor = 0.0f;
        float ver = 0.0f;

        if (Input.GetMouseButton(1) || !RightClickToDrag)
        {
            hor = mouseSensitivity * Input.GetAxis("Mouse X");
            ver = -mouseSensitivity * Input.GetAxis("Mouse Y");
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentMoveSpeed /= 2;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed *= 2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            left_right = currentMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            left_right = -currentMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            front_back = currentMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            front_back = -currentMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            up_down = currentMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.C))
        {
            up_down = -currentMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            hor = rotaSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            hor = -rotaSpeed * Time.deltaTime;
        }
        

        transform.Translate(new Vector3(left_right, up_down, front_back));
        transform.Rotate(0, hor, 0, Space.World);
        transform.Rotate(ver, 0, 0);
    }
}
