using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

    [SerializeField] float speed;
    [SerializeField] int monsterMaxHp;
    [SerializeField] float monsterHp;
    public int damage;
    private void Start()
    {
        StartCoroutine(Move());



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

    }
    public void Hit(float takedDmg, int colorNum)
    {
        monsterHp -= Mathf.Round(takedDmg);
        hpImage.fillAmount = monsterHp / monsterMaxHp;
        if(monsterHp <= 0)//몬스터 죽음
        {
            ani.SetBool("Die", true);
            
        }
        PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, takedDmg, 1);
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
}
