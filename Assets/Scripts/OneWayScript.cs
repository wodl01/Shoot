using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayScript : MonoBehaviour
{
    [SerializeField] PlatformEffector2D platformEffector;
    public Player player;
    private void Update()
    {
        if(player != null)
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && player.isGround)//밑 점프
            {
                platformEffector.rotationalOffset = 180f;//밑으로 내려갈수있음
            }

            if (!Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))//그냥점프
            {
                platformEffector.rotationalOffset = 0f;//위로 올라갈수있음
            }
            if (player.isFallen)
            {
                platformEffector.rotationalOffset = 0f;
            }
        }
    }
}
