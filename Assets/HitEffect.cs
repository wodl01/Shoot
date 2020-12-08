using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HitEffect : MonoBehaviour
{
    [SerializeField] PhotonView pv;
    [SerializeField] bool turn;

    private void Start()
    {
        if (turn)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
    }
    public void DestroyEffect()
    {
        pv.RPC("Destroy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Destroy()
    {
        Destroy(gameObject);
    }
}
