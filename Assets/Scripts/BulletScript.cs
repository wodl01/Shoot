using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour
{
    public bool isBullet;

    public Player player;
    public PhotonView pv;
    int dirNum;//방향
    public float bulletSpeed;

    public float playerDamage;
    public float playerBlood;
    public float bulletDamage;
    public float finalDamage;
    public string playerName;
    public float finalAttackHeal;

    [SerializeField] Rigidbody2D bulletRigid;

    int dirX;
    int dirY;

    public int buffCode;
    public float during;
    public float duringAbility;
    private void Awake()
    {


        // if (players.pv.IsMine == true)
        //{
        //   player = players;
        //}
    }




    
    private void Start()
    {
       
    }

    private void Update()
    {
        //transform.Translate(new Vector3(dirX,dirY,0) * bulletSpeed * Time.deltaTime);
        
    }

    


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground") pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        if (!pv.IsMine && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            other.GetComponent<Player>().takedDamage = finalDamage;//데미지주기
            finalAttackHeal = (1 + playerBlood) / finalDamage;//피흡
            Debug.Log((1 + playerBlood) / finalDamage);
            player.pv.RPC("AttackHeal", RpcTarget.All, finalAttackHeal);
            //버프주기
            other.GetComponent<Player>().buffScript.buffNum = buffCode;
            other.GetComponent<Player>().buffScript.Active = true;
            
            other.GetComponent<Player>().Hit();

            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }



        
    }

    [PunRPC]
    void BulletDirRPC(int dir , int pp)
    {
        //playerName = pp;
        //Debug.Log(pp);
        dirNum = dir;



        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject py in players)
        {

            player = py.GetComponent<Player>();
            if (player.pv.ViewID == pp)
            {
                //Debug.Log("찾았다" + player.name);
                // 찾았다
                if (dirNum == 1)
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

                if (isBullet)
                {
                    bulletRigid.velocity = new Vector3(dirX, dirY, 0) * bulletSpeed;
                }

                Destroy(gameObject, 3f);

                playerDamage = player.GetComponent<Player>().playerDamage;
                duringAbility = player.GetComponent<Player>().duringAbility;
                playerBlood = player.GetComponent<Player>().blood;
                finalDamage = playerDamage * (1 + bulletDamage);
                return;
            }

        }
        
    }


    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
