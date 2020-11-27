using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] MonsterScript Ms;
    int damage;

    private void Start()
    {
        damage = Ms.damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().buffScript.buffNum = Ms.takingBuffNum;
            other.GetComponent<Player>().buffScript.during = Ms.during;
            other.GetComponent<Player>().buffScript.Active = true;
            other.GetComponent<Player>().Hit(damage, 1, true, true);
        }
    }
}
