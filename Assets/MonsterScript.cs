using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MonsterScript : MonoBehaviour
{
    float randomTime;
    [SerializeField] bool canMove;
    public bool isDie;
    public GameObject player;
    [SerializeField] Rigidbody2D monsterRigid;
    [SerializeField] Animator ani;
    [SerializeField] Image hpImage;
    [SerializeField] PhotonView pv;
    [SerializeField] Canvas canvas;
    public MonsterBuffScript MB;
    [SerializeField] DamageTextManager dtm;
    private AudioManager Audio;
    [SerializeField] bool ismyAttack;

    [SerializeField] float speed;
    [SerializeField] int monsterMaxHp;
    public float monsterHp;
    public int damage;
    public int takingBuffNum;
    public float during;
    [SerializeField] string[] takedSound;
    [SerializeField] string[] monsterSound;
    private void Awake()
    {
        Audio = FindObjectOfType<AudioManager>();
        dtm = FindObjectOfType<DamageTextManager>();
    }
    [PunRPC]
    void isSpawn()
    {

    }


    private void Update()
    {
        
        if(player != null && canMove)
        {
            if (player.transform.position.x < gameObject.transform.position.x)
            {
                monsterRigid.velocity = new Vector2(-1 * speed, 0);
                gameObject.transform.localScale = new Vector2(1, 1);
                canvas.transform.localScale = new Vector2(1, 1);
                ani.SetBool("IsMove", true);
            }
            else
            {
                monsterRigid.velocity = new Vector2(1 * speed, 0);
                gameObject.transform.localScale = new Vector2(-1, 1);
                canvas.transform.localScale = new Vector2(-1, 1);
                ani.SetBool("IsMove", true);
            }
        }
        else if(player == null)
        {
            ani.SetBool("IsMove", false);
        }

    }
    [PunRPC]
    public void Hit(bool myAttack ,float takedDmg, int takedSoundNum,int colorNum)
    {

        //ismyAttack = true;
        if (myAttack)
        {
            Debug.Log(myAttack);
            dtm.DamageTextSpawn(gameObject ,takedDmg, colorNum, false);
            //PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, takedDmg, colorNum, false);
        }
        
        monsterHp -= Mathf.Round(takedDmg);
        Audio.Play(takedSound[takedSoundNum]);
        hpImage.fillAmount = monsterHp / monsterMaxHp;
        if (monsterHp <= 0)//몬스터 죽음
        {
            ani.SetBool("Die", true);
            isDie = true;

        } 
    }

    public void CanMove()
    {
        canMove = true;
    }
    public void StopMove()
    {
        canMove = false;
    }
    public void MonsterAttack()
    {
        Audio.Play(monsterSound[0]);
    }
    public void MonsterBeAttack()
    {
        Audio.Play(monsterSound[1]);
    }
    public void MonsterDie()
    {
        Audio.Play(monsterSound[2]);
    }
    [PunRPC]
    private void DestroyObRPC()
    {
        Destroy(gameObject);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpImage.fillAmount);
        }
        else
        {
            hpImage.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
