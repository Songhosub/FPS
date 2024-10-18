using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    //public Transform Front;
    public Transform BackT;
    public Transform ProntT;

    Vector3 Back;
    Vector3 Pront;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.Reload)
        {
            Back = BackT.position;
            transform.position = Back + transform.forward * 0.85f + transform.up * 0.12f;
            transform.forward = -BackT.right;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x - 6.0f, transform.eulerAngles.y - 3.0f, transform.eulerAngles.z);
        }
        else
        {

        }
    }
    public void Reload()
    {
        StartCoroutine(ReloadAni());
    }

    IEnumerator ReloadAni()
    {
        float time = 0;
        while (time <= 1.75f)
        {
            time += Time.deltaTime;
            Back = BackT.position;
            transform.position = Back + transform.forward * 0.85f + transform.up * 0.12f;
            transform.forward = -BackT.right;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x - 6.0f, transform.eulerAngles.y - 3.0f, transform.eulerAngles.z);
            yield return null;
        }
        time = 0;
        while (time <= 0.5f)
        {
            time += Time.deltaTime;
            Pront = ProntT.position;
            transform.position = Pront + transform.right * 0.185f + transform.forward * 0.1f + transform.up * 0.02f;
            transform.forward = -BackT.right;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x - 6.0f, transform.eulerAngles.y - 3.0f, transform.eulerAngles.z);
            yield return null;
        }
    }

}
