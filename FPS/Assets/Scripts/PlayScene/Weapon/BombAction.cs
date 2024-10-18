using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject Explosion;
    public float ThrowPower = 1000.0f;
    PlayerController Player;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Player.Explosion(transform.position);
        Destroy(gameObject);
    }
}
