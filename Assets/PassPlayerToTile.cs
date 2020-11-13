using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlayerToTile : MonoBehaviour
{
    public Player Player;
    [SerializeField] OneWayScript[] oneWay;

    private void Update()
    {
        if(Player != null)
        {
            for (int i = 0; i < oneWay.Length; i++)
            {
                oneWay[i].player = Player;
            }
            Player = null;
        }
    }
}
