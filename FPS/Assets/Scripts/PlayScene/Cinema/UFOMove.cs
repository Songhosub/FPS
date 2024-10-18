using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMove : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.y == -90.0f)
        {
            return;
        }

        if(transform.position.y > 3.8f)
        {
            transform.Translate(transform.up * -35.0f * Time.deltaTime);
        }
        else if(transform.position.y <= 3.8f)
        {
            transform.position = new Vector3(-44.5f, 3.8f, 102.31f);
            transform.eulerAngles = new Vector3(0, -90.0f, 0);
        }
    }

    public void Move()
    {
        transform.position = new Vector3(-44.5f, 3.8f, 102.31f);
        transform.eulerAngles = new Vector3(0, -90.0f, 0);
    }
}
