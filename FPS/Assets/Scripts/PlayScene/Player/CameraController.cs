using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform CameraPos;
    
    float mx = 0;
    float my = 0;
    float temp = 0;

    public float RotateSpeed = 60;

    // Start is called before the first frame update
    void Start()
    {
        CameraPos = GameObject.Find("CameraPosition").transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.GM.gameState == GameManager.GameState.Stop)
        {
            return;
        }

        //이동
        transform.position = CameraPos.position;

        //회전
        float X = Input.GetAxis("Mouse X");
        float Y = Input.GetAxis("Mouse Y");

        mx += RotateSpeed * Time.deltaTime * X;
        my += RotateSpeed * Time.deltaTime * Y;


        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            mx = Mathf.Clamp(mx, temp - 75, temp + 75);
        }
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetMouseButtonDown(0))
        {
            temp = mx;
        }
        my = Mathf.Clamp(my, -20, 45);


        transform.eulerAngles = new Vector3(-my, mx, 0);
    }

    public void RotateSpeedChange(float speed)
    {
        RotateSpeed = speed;
    }
}
