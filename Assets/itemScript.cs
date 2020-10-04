using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class itemScript : MonoBehaviour
{
    public PhotonView pv;
    public int itemType;

    public int itemNum;



    public int maximumAmmo;
    public int ammo;
    public bool isFullAuto;
    public float delayTime;
    public float reLoadTime;

    public Sprite weaponSprite;





    [PunRPC]
    public void DestroyItemRPC()
    {
        Destroy(gameObject);
    }
    
}
