using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MonsterSpawnManager : MonoBehaviour
{
    public bool start;
    [SerializeField] int monsterNum;
    [SerializeField] GameObject spawnPos;
    [SerializeField] Player player;
    private void Update()
    {
        if (start)
        {
            start = false;
            PhotonNetwork.Instantiate("Monster" + "/" + monsterNum.ToString(), spawnPos.transform.position, Quaternion.identity)
                                                .GetComponent<PhotonView>().RPC("isSpawn", RpcTarget.All, player.pv.ViewID);
        }

    }
}
