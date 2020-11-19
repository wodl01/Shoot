using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShildScript : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PhotonView pv;

    [SerializeField] Animator ani;
    [SerializeField] bool isright;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(player.weaponNum >= 3000 && player.weaponNum < 5000 && player.canUseShild)
            {
                bool active = true;
                player.isShilding = true;
                player.shildRechargeTime = 1;
                ani.SetBool("ShildOn", true);
                isright = true;
                pv.RPC("ShildShape", RpcTarget.AllBuffered,active, isright);
            }


        }
        else if (Input.GetMouseButton(1))
        {
            if (player.weaponNum2 >= 3000 && player.weaponNum2 < 5000 && player.canUseShild)
            {
                bool active = true;
                player.isShilding = true;
                player.shildRechargeTime = 1;
                ani.SetBool("ShildOn", true);
                isright = false;
                pv.RPC("ShildShape", RpcTarget.AllBuffered, active, isright);
            }
        }
        else
        {
            bool active = false;
            ani.SetBool("ShildOn", false);
            pv.RPC("ShildShape", RpcTarget.AllBuffered, active, isright);
            player.isShilding = false;
        }
    }

    
}
