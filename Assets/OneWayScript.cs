using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayScript : MonoBehaviour
{
    [SerializeField] PlatformEffector2D platformEffector;
    [SerializeField] Player player;
    [SerializeField] Rigidbody2D playerRigid;




    private void Update()
    {
        player = GameObject.Find("MyPlayer").GetComponent<Player>();
        playerRigid = player.GetComponent<Rigidbody2D>();

        

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            if (player.passWaitTime <= 0)
            {
                platformEffector.rotationalOffset = 180f;
                player.passWaitTime = 0.3f;
                Debug.Log("222");
            }

        }
        if (playerRigid.velocity.y > 0)
        {
            platformEffector.rotationalOffset = 0f;
            Debug.Log("111");
        }



    }
}
