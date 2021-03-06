﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] bool isUp;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isUp)
        {
            if (collision.tag == "Ground" || collision.tag == "PassingGround")
            {
                if(player.rigid.velocity.y <= 0)
                {
                    player.isGround = true;
                }
            }
        }
        if (isUp)
        {
            if (collision.tag == "Ground" || collision.tag == "PassingGround")
            {
                player.isFallen = true;
            }
            else
            {
                player.isFallen = false;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isUp)
        {
            if (collision.tag == "Ground" || collision.tag == "PassingGround")
            {
                if (player.rigid.velocity.y > 0)
                {
                    player.isGround = false;
                }
            }
        }
    }
}
