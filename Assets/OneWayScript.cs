using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayScript : MonoBehaviour
{
    [SerializeField] PlatformEffector2D platformEffector;


    public float passWaitTime;



    private void Update()
    {

        

        if (passWaitTime >= 0)
        {
            passWaitTime -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            if (passWaitTime <= 0)
            {
                platformEffector.rotationalOffset = 180f;//밑으로
                passWaitTime = 0.1f;
                
            }

        }
        
        if (!Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && passWaitTime < 0)
        {
            platformEffector.rotationalOffset = 0f;//위
            passWaitTime = 0.1f;

        }
        /*
        if(passWaitTime < 0)
        {
            platformEffector.rotationalOffset = 0f;//위
        }
        */

    }
}
