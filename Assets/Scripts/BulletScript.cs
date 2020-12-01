using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour
{
    public bool isBullet;
    [SerializeField] float angle;

    public Player player;
    public PhotonView pv;
    private AudioManager Audio;
    [SerializeField] string sound0;
    [SerializeField] string sound1;
    [SerializeField] string sound2;
    [SerializeField] string sound3;

    int dirNum;//방향
    public float bulletSpeed;
    public float playerDamage;
    public float playerBlood;
    public float bulletDamage;
    public float finalDamage;
    public int hitAmount;
    public float attackSpeed;

    [SerializeField] bool isRightGun;

    [SerializeField] Rigidbody2D bulletRigid;
    [SerializeField] Animator ani;

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
            
            /*
            if(dirX != 0)
            {
                gameObject.transform.localScale = new Vector3(-dirX, 1, 1);
                
            }
            
            if(dirY != 0)
            {
               // gameObject.transform.rotation = Quaternion.Euler(0, 0, dirY * -90);
                gameObject.transform.localScale = new Vector3(1, islookLeft, 1);
            }*/
            
        }
        else
        {
            gameObject.transform.Translate(bulletSpeed * 0.01f, 0, 0);
        }

    }

    


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" && isBullet) pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        if (!pv.IsMine && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            float healAmount;

            other.GetComponent<Player>().Hit(finalDamage, 1, true, true);
            if(player.isShilding == false)//피흡
            {
                player.GetComponent<PhotonView>().RPC("AttackHeal", RpcTarget.AllBuffered, finalDamage);
                healAmount = finalDamage * player.blood;
                
            }
            float finalduring = during * duringAbility;
            //버프주기
            if (!player.isShilding)
            {
                other.GetComponent<Player>().buffScript.buffNum = buffCode;
                other.GetComponent<Player>().buffScript.during = finalduring;
                other.GetComponent<Player>().buffScript.Active = true;
            }
            

            
            if (isBullet)
            {
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
            
        }
        if(other.tag == "Monster")
        {
            if (!other.GetComponent<MonsterScript>().isDie)
            {
                float healAmount;
                other.GetComponent<MonsterScript>().Hit(player.isMine, finalDamage, 0, 1);

                if (player.isShilding == false)//피흡
                {
                    player.GetComponent<PhotonView>().RPC("AttackHeal", RpcTarget.AllBuffered, finalDamage);
                    healAmount = finalDamage * player.blood;

                }
                float finalduring = during * duringAbility;
                if (!player.isShilding)
                {
                    other.GetComponent<MonsterScript>().MB.buffNum = buffCode;
                    other.GetComponent<MonsterScript>().MB.time = finalduring;
                    other.GetComponent<MonsterScript>().MB.isMyAttack = player.pv.IsMine;
                    other.GetComponent<MonsterScript>().MB.active = true;
                }
                double rad = Mathf.Atan2(player.transform.position.y - other.gameObject.transform.position.y, player.transform.position.x - other.gameObject.transform.position.x);
                double degree = (rad * 180) / Mathf.PI;
                PhotonNetwork.Instantiate("HitEffect1", other.gameObject.transform.position, Quaternion.Euler(0, 0, (float)degree));
                Debug.Log(degree);
                if (isBullet)
                {
                    pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
                }
            }
        }
    }
    public void CanAttack()
    {
        if (isRightGun)
        {
            player.readyToAttack = true;
            player.pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered);
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        else
        {
            player.readyToAttack2 = true;
            player.pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered);
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    public void Sound0()
    {
        Audio.Play(sound0);
    }
    public void Sound1()
    {
        Audio.Play(sound1);
    }
    public void Sound2()
    {
        Audio.Play(sound2);
    }
    public void Sound3()
    {
        Audio.Play(sound3);
    }


    [PunRPC]
    void BulletDirRPC(int dir , int pp, bool isRight)
    {
        //playerName = pp;
        //Debug.Log(pp);
        dirNum = dir;

        isRightGun = isRight;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Audio = FindObjectOfType<AudioManager>();

        foreach (GameObject py in players)
        {

            player = py.GetComponent<Player>();
            if (player.pv.ViewID == pp)
            {
                Destroy(gameObject, 3f);

                playerDamage = player.GetComponent<Player>().playerDamage;
                duringAbility = player.GetComponent<Player>().duringAbility;
                playerBlood = player.GetComponent<Player>().blood;
                finalDamage = playerDamage * bulletDamage;
                attackSpeed = player.attackSpeedAbility;

                if (!isBullet)
                {
                    ani.SetFloat("AttackSpeed", attackSpeed);
                }
                

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
