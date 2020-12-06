using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCanAttackRange : MonoBehaviour
{
    [SerializeField] Animator monsterAni;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "MyPlayer")
        {
            monsterAni.SetBool("Attack", true);
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "MyPlayer")
        {
            monsterAni.SetBool("Attack", false);

        }
    }
}
