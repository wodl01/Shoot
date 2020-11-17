using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShildScript : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer shildSprite;
    [SerializeField] bool isright;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(player.weaponNum >= 3000 && player.weaponNum < 5000)
            {
                player.isShilding = true;
                player.shildRechargeTime = 1;
                isright = true;
            }


        }
        else if (Input.GetMouseButton(1))
        {
            if (player.weaponNum2 >= 3000 && player.weaponNum2 < 5000)
            {
                player.isShilding = true;
                player.shildRechargeTime = 1;
                isright = false;
            }
        }
        else
        {
            player.isShilding = false;
        }
    }

    [PunRPC]
    void ShildShape()
    {
        if (isright)
        {
            shildSprite.sprite = Resources.Load("Weapon" + "/" + player.weaponNum.ToString() + "Weapon" + "/" + player.weaponNum.ToString(), typeof(Sprite)) as Sprite;
        }
        else if (!isright)
        {
            shildSprite.sprite = Resources.Load("Weapon" + "/" + player.weaponNum2.ToString() + "Weapon" + "/" + player.weaponNum2.ToString(), typeof(Sprite)) as Sprite;
        }

    }
}
