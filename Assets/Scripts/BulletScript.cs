using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour
{
    public PhotonView pv;
    int dir;//방향
    public float bulletSpeed;
    public int bulletDamege;
    public string bulletNum;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime * dir);
    }

    


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground") pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        if (!pv.IsMine && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            other.GetComponent<Player>().takedDamage = bulletDamege;
            other.GetComponent<Player>().Hit();

            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }



        
    }

    [PunRPC]
    void BulletDirRPC(int dir)
    {
        this.dir = dir;
    }


    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
