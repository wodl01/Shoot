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

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime * dir);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground") pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
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
