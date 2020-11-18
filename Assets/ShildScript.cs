using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShildScript : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PhotonView pv;
    [SerializeField] SpriteRenderer shildSprite;
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
                pv.RPC("ShildShape", RpcTarget.AllBuffered,active);
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
                pv.RPC("ShildShape", RpcTarget.AllBuffered, active);
            }
        }
        else
        {
            bool active = false;
            ani.SetBool("ShildOn", false);
            pv.RPC("ShildShape", RpcTarget.AllBuffered, active);
            player.isShilding = false;
        }
    }

    [PunRPC]
    void ShildShape(bool isActive)
    {
        if (isright && isActive)
        {
            shildSprite.sprite = Resources.Load("Weapon" + "/" + player.weaponNum.ToString() + "Weapon" + "/" + player.weaponNum.ToString(), typeof(Sprite)) as Sprite;
            Debug.Log("1111");
        }
        else if (!isright && isActive)
        {
            shildSprite.sprite = Resources.Load("Weapon" + "/" + player.weaponNum2.ToString() + "Weapon" + "/" + player.weaponNum2.ToString(), typeof(Sprite)) as Sprite;
            Debug.Log("2222");
        }
        else if(!isActive)
        {
            shildSprite.sprite = Resources.Load("Null", typeof(Sprite)) as Sprite;
            Debug.Log("3333");
        }
    }
}
