﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerScript : MonoBehaviour
{
    public MonsterScript Ms;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "MyPlayer")
        {

            Ms.player = other.gameObject;
            
        }
    }

}
