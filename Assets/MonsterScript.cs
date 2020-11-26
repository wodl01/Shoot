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
    public GameObject player;
    [SerializeField] Rigidbody2D monsterRigid;
    [SerializeField] Animator ani;
    [SerializeField] Image hpImage;
    [SerializeField] PhotonView pv;
    [SerializeField] Canvas canvas;
    public MonsterBuffScript MB;
    [SerializeField] bool ismyAttack;

    [SerializeField] float speed;
    [SerializeField] int monsterMaxHp;
    public float monsterHp;
    public int damage;
    private void Start()
    {
        StartCoroutine(Move());



    }
    [PunRPC]
    void isSpawn()
    {

    }
    IEnumerator Move()
    {
        randomTime = Random.Range(1, 6);

        yield return new WaitForSeconds(randomTime);
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
    public void Hit(bool myAttack ,float takedDmg, int colorNum)
    {

        //ismyAttack = true;
        if (myAttack)
        {
            PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, takedDmg, 1, false);
        }
        
        monsterHp -= Mathf.Round(takedDmg);

        hpImage.fillAmount = monsterHp / monsterMaxHp;
        if (monsterHp <= 0)//몬스터 죽음
        {
            ani.SetBool("Die", true);

        }

        else
        {
            //ismyAttack = false;
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
