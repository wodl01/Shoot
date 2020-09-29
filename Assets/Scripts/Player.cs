using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D rigid;
    public Animator ani;
    public SpriteRenderer sprite;
    public PhotonView pv;
    public Text nickNameText;
    public Image health;

    public int maxHpValue;
    public int hp;

    [SerializeField] GameObject shotPos;

    [SerializeField] float jumpPow;
    [SerializeField] bool isGround;
    [SerializeField] string bulletName;

    [SerializeField] bool left;
    Vector3 curPos;

    void Awake()
    {
        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nickNameText.color = pv.IsMine ? Color.green : Color.red;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            float axis = Input.GetAxisRaw("Horizontal");
            rigid.velocity = new Vector2(4 * axis, rigid.velocity.y);

            

            if(axis != 0)
            {
                ani.SetBool("IsMove", true);
                pv.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
            }
            else
            {
                ani.SetBool("IsMove", false);
            }

            if(rigid.velocity.y < 0)
            {
                ani.SetBool("IsFalling", true);
            }
            else
            {
                ani.SetBool("IsFalling", false);
            }

            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.6f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            ani.SetBool("IsGround", isGround);
            if (Input.GetKeyDown(KeyCode.Space) && isGround) pv.RPC("JumpRPC", RpcTarget.All);

            if (Input.GetMouseButtonDown(0))//총발사
            {
                PhotonNetwork.Instantiate(bulletName, shotPos.transform.position, Quaternion.identity)
                    .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, left ? -1 : 1);
            }
        }
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        
        if(axis == -1)
        {
            left = true;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            left = false;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
        

    [PunRPC]
    void JumpRPC()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpPow);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }


}
