using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HitEffect : MonoBehaviour
{
    [SerializeField] PhotonView pv;


    [PunRPC]
    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
