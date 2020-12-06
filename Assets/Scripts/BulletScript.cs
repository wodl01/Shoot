using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviour
{
    public bool isBullet;
    public bool isSword;
    [SerializeField] bool isDamaging;
    [SerializeField] bool isEffect;
    [SerializeField] int weaponNum;
    [SerializeField] string flyingEffectName;
    [SerializeField] float cycle;
    [SerializeField] bool flyingEffectTrue;

    public Player player;
    [SerializeField] Animator ani;
    public PhotonView pv;
    [SerializeField] Guided_Bullet gb;
    private AudioManager Audio;
    [SerializeField] int takingSoundNum;
    [SerializeField] string sound0;
    [SerializeField] string sound1;
    [SerializeField] string sound2;
    [SerializeField] string sound3;

    public float bulletSpeed;
    public float playerDamage;
    public float playerBlood;
    public float bulletDamage;
    public float finalDamage;
    public int hitAmount;
    public float attackSpeed;
    [SerializeField] int rotateSpeed;
    [SerializeField] bool isGuided;

    [SerializeField] bool isRightGun;
    [SerializeField] int playerViewId;

    float angle;

    int dirX;
    int dirY;

    public int buffCode;
    public float during;
    public float duringAbility;


    private void Update()
    {

        if (isSword)
        {
            if (isRightGun)
            {
                transform.position = player.shotPos.transform.position;
            }
            else
            {
                transform.position = player.shotPos2.transform.position;
            }
        }
        else if(isBullet)
        {
            gameObject.transform.Translate(bulletSpeed * 0.01f, 0, 0);
        }
    }
    IEnumerator effectCycle()
    {
        PhotonNetwork.Instantiate(flyingEffectName, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(cycle);
        StartCoroutine(effectCycle());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" && isBullet) pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        if (!pv.IsMine && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            if (isDamaging)
            {
                float healAmount;

                other.GetComponent<Player>().Hit(finalDamage, 1, true, true);
                if (player.isShilding == false)//피흡
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
                double rad = Mathf.Atan2(player.transform.position.y - other.gameObject.transform.position.y, player.transform.position.x - other.gameObject.transform.position.x);
                double degree = (rad * 180) / Mathf.PI;

                PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + "HitEffect", other.gameObject.transform.position, Quaternion.Euler(0, 0, (float)degree));

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
                if (isDamaging)
                {
                    float healAmount;
                    other.GetComponent<MonsterScript>().Hit(player.isMine, finalDamage, takingSoundNum, 1);

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


                    PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + "HitEffect", other.gameObject.transform.position, Quaternion.Euler(0, 0, (float)degree));

                }
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
            player.pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, isRightGun);
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        else
        {
            player.readyToAttack2 = true;
            player.pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, isRightGun);
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
    void BulletDirRPC(int pp, bool isRight)
    {
        //playerName = pp;
        //Debug.Log(pp);

        isRightGun = isRight;

        GameObject[] players = GameObject.FindGameObjectsWithTag("MyPlayer");
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
                playerViewId = pp;

                if (isGuided)
                {
                    gb.rotateSpeed = rotateSpeed;
                }

                if (flyingEffectTrue)
                {
                    StartCoroutine(effectCycle());
                }

                if (isSword)
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
        if (!isDamaging && player.pv.IsMine)
        {
            PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + "DestroyEffect", gameObject.transform.position, Quaternion.identity)
                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, playerViewId, isRightGun);
        }
        else if (!isEffect && player.pv.IsMine)
        {
            PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + "DestroyEffect", gameObject.transform.position, Quaternion.identity);
                
        }
        Destroy(gameObject);
    }
}
