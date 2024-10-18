using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    GameObject Player;
    int AttackPower;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        AttackPower = 20;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Attack()
    {
        Player.GetComponent<PlayerController>().PlayerDamaged(AttackPower);
    }
}
