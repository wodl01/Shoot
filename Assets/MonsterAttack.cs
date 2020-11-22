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
            other.GetComponent<Player>().Hit(damage, 1, true);
        }
    }
}
