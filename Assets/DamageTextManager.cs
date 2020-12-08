using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DamageTextManager : MonoBehaviour
{

    [SerializeField] PhotonView pv;

    public void DamageTextSpawn(GameObject where ,float TakedDamage, int colorNum ,bool isplus)
    {
        if (pv.IsMine)
        {
            PhotonNetwork.Instantiate("DamageText", where.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, TakedDamage, colorNum, isplus);
        }

    }
}
