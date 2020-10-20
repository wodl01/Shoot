using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour
{
    [SerializeField] string kindOfBullet;
    [SerializeField] Player player;
    public PhotonView pv;
    int dirNum;//방향
    public float bulletSpeed;

    public int playerDamage;
    public float playerBlood;
    public int bulletDamage;
    public int finalDamage;


    public string bulletNum;
    [SerializeField] Rigidbody2D bulletRigid;

    int dirX;
    int dirY;

    public int buffCode;
    public float during;
    public float duringAbility;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Start()
    {
        
        if(dirNum == 1)
        {
            dirX = 1;
            dirY = 0;
        }
        if (dirNum == 2)
        {
            dirX = -1;
            dirY = 0;
        }
        if (dirNum == 3)
        {
            dirX = 0;
            dirY = 1;
        }
        if (dirNum == 4)
        {
            dirX = 0;
            dirY = -1;
        }
        finalDamage = playerDamage * bulletDamage;

        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        //transform.Translate(new Vector3(dirX,dirY,0) * bulletSpeed * Time.deltaTime);
        bulletRigid.velocity = new Vector3(dirX, dirY, 0) * bulletSpeed;
    }

    


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground") pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        if (!pv.IsMine && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            other.GetComponent<Player>().takedDamage = finalDamage;
            player.hp += (1 + playerBlood) / finalDamage;
            other.GetComponent<Player>().buffScript.buffNum = buffCode;
            other.GetComponent<Player>().buffScript.Active = true;

            other.GetComponent<Player>().Hit();

            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }



        
    }

    [PunRPC]
    void BulletDirRPC(int dir)
    {
        this.dirNum = dir;
    }


    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
