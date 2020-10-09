using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    [SerializeField] Player player;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            player.isGround = true;
        }
        else
        {
            player.isGround = false;
        }
    }
}
