using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    //public Transform Front;
    public Transform BackT;
    Vector3 Back;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Back = BackT.position;
        transform.position = Back + transform.forward * 0.25f;
        transform.forward = -BackT.right;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - 6.0f, transform.eulerAngles.y - 3.0f, transform.eulerAngles.z);
    }
}
