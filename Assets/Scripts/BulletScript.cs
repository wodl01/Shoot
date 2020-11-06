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
    public int hitNum;
    public float attackSpeed;

    [SerializeField] bool isRightGun;

    [SerializeField] Rigidbody2D bulletRigid;
    [SerializeField] Animator ani;

    int dirX;
    int dirY;
    int islookLeft;

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
        if (!isBullet)
        {
            if (isRightGun)
            {
                transform.position = player.shotPos.transform.position;
            }
            else
            {
                transform.position = player.shotPos2.transform.position;
            }
            

            if(dirX != 0)
            {
                gameObject.transform.localScale = new Vector3(-dirX, 1, 1);
                
            }
            
            if(dirY != 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, dirY * -90);
                gameObject.transform.localScale = new Vector3(1, islookLeft, 1);
            }



        }
    }

    


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" && isBullet) pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
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
            if (isBullet)
            {
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            
        }
        
    }
    public void CanAttack()
    {
        if (isRightGun)
        {
            player.readyToAttack = true;
            player.pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered,player.weaponNum);
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        else
        {
            player.readyToAttack2 = true;
            player.pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, player.weaponNum2);
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }



    [PunRPC]
    void BulletDirRPC(int dir , int pp, bool isRight)
    {
        //playerName = pp;
        //Debug.Log(pp);
        dirNum = dir;

        isRightGun = isRight;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject py in players)
        {

            player = py.GetComponent<Player>();
            if (player.pv.ViewID == pp)
            {
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
                
                islookLeft = player.left ? 1 : -1;

                Destroy(gameObject, 3f);

                playerDamage = player.GetComponent<Player>().playerDamage;
                duringAbility = player.GetComponent<Player>().duringAbility;
                playerBlood = player.GetComponent<Player>().blood;
                finalDamage = playerDamage * (1 + bulletDamage);
                attackSpeed = player.attackSpeedAbility;

                ani.SetFloat("AttackSpeed", attackSpeed);

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
