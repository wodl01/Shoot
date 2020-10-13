using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool isMine = false;
    public Rigidbody2D rigid;
    public Animator ani;
    public SpriteRenderer sprite;
    public PhotonView pv;

    public Text nickNameText;
    public Image health;
    [SerializeField] int damage;


    [SerializeField] Canvas canvas;
    [SerializeField] Text bulletText;

    [SerializeField] GameObject reLoadOB;
    [SerializeField] GameObject reLoadBarOB;
    [SerializeField] Slider reLoadBar;

    [SerializeField] GameObject bodyUp;
    [SerializeField] Animator bodyUpAni;
    [SerializeField] GameObject upPosOB;
    [SerializeField] GameObject downPosOB;

    [SerializeField] GameObject weaponOB;
    [SerializeField] GameObject weaponOB2;
    [SerializeField] GameObject weaponUpPos;
    [SerializeField] GameObject weaponUpPos2;
    [SerializeField] GameObject weaponMidPos;
    [SerializeField] GameObject weaponMidPos2;
    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>
    public float maxHpValue;
    public float hp;

    public int takedDamage;

    [SerializeField] bool isLookUp;

    [SerializeField] GameObject shotPos;


    [SerializeField] float jumpPow;
    public bool isGround;
    [SerializeField] float speed;

    [SerializeField] SpriteRenderer weaponSprite;
    
    [SerializeField] int weaponNum;
    [SerializeField] GameObject[] bullet;
    [SerializeField] int maximumAmmo_P;
    [SerializeField] int ammo_P;
    [SerializeField] bool IsFullAuto_P;
    [SerializeField] float delay_P;
    [SerializeField] float delayTime;
    [SerializeField] float reLoadTime_P;
    [SerializeField] float reLoadingTime;
    [SerializeField] Animator reLoadAni;
    [SerializeField] bool isReload;
    [SerializeField] string itemSpriteName; 
    itemScript item;

    [SerializeField] int attackDir;
    /// <summary>
    /// ////////////////////////////////////////////
    /// </summary>
    bool Once = true;


    [SerializeField] bool left;
    public bool isBodySetUp;
    Vector3 curPos;

    [SerializeField]float cooltime;
    public float passWaitTime;


    [SerializeField] OneWayScript oneWay;
    public bool isFallen;

    void Awake()
    {
        
        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nickNameText.color = pv.IsMine ? Color.green : Color.red;

        if (pv.IsMine)
        {
            var Cm = GameObject.Find("CMcamera").GetComponent<CinemachineVirtualCamera>();
            bulletText = GameObject.FindGameObjectWithTag("BulletText").GetComponent<Text>();
            Cm.Follow = transform;
            Cm.LookAt = transform;

            oneWay = GameObject.Find("OneWayTile").GetComponent<OneWayScript>();
            oneWay.player = this;
            //gameObject.name = "MyPlayer";
            isMine = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F) && other.tag == "Item" && cooltime < 0 && !isReload)
            {
                //무기교체
                item = other.GetComponent<itemScript>();
                //자신이 가지고있던"무기"생성
                if (weaponNum > 0)
                {
                    PhotonNetwork.Instantiate(weaponNum.ToString() + "weapon", gameObject.transform.position, Quaternion.identity).GetComponent<itemScript>().ammo = ammo_P;
                }



                //"무기"아이템 얻음
                weaponNum = item.itemNum;
                maximumAmmo_P = item.maximumAmmo;
                ammo_P = item.ammo;
                if (item.isFullAuto == true)
                {
                    IsFullAuto_P = true;
                }
                else IsFullAuto_P = false;

                itemSpriteName = other.name;

                pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, itemSpriteName);

                delay_P = item.delayTime;
                reLoadTime_P = item.reLoadTime;
                reLoadingTime = reLoadTime_P;
                Debug.Log("1");

                bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();


                //자신이 획득한"무기"삭제
                other.GetComponent<itemScript>().pv.RPC("DestroyItemRPC", RpcTarget.AllBuffered);
                Debug.Log("2");
                cooltime = 2;
            }
            
        }
        
        
    }

    void Update()
    {
        
        if(cooltime >= 0)
        {
            cooltime -= Time.deltaTime;
        }
        if(delayTime >= 0)
        {
            delayTime -= Time.deltaTime;
        }
        if (passWaitTime >= 0)
        {
            passWaitTime -= Time.deltaTime;
        }



        if (pv.IsMine)
        {
            //움직임
            float axis = Input.GetAxisRaw("Horizontal");
            Vector3 move = new Vector3(axis, 0, 0);
            rigid.velocity = new Vector2(speed * axis, rigid.velocity.y);


            health.fillAmount = hp / maxHpValue;

            if (axis != 0)
            {
                ani.SetBool("IsMove", true);
                pv.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
            }
            else
            {
                ani.SetBool("IsMove", false);
            }

            if (rigid.velocity.y < 0)
            {
                ani.SetBool("IsFalling", true);
            }
            else
            {
                ani.SetBool("IsFalling", false);
            }

            //isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0, -0.25f), didi, 1 << LayerMask.NameToLayer("Ground"));
            //ani.SetBool("IsGround", isGround);
            
            ani.SetBool("IsGround", isGround);
            if (Input.GetKeyDown(KeyCode.Space) && isGround && !Input.GetKey(KeyCode.S)) pv.RPC("JumpRPC", RpcTarget.All);
            //점프

            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                //위를봄
                if (!left)
                {
                    attackDir = 3;
                    weaponOB.transform.position = weaponUpPos.transform.position;
                    weaponOB.transform.rotation = Quaternion.Euler(0, 0, 90);
                    weaponOB2.transform.position = weaponUpPos2.transform.position;
                    weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 90);
                    bodyUpAni.SetBool("IsLookUp", true);
                    bodyUpAni.SetBool("IsLookInfront", false);
                }
                

                if (left)
                {
                    attackDir = 3;
                    weaponOB.transform.position = weaponUpPos.transform.position;
                    weaponOB.transform.rotation = Quaternion.Euler(0, 0, -90);
                    weaponOB2.transform.position = weaponUpPos2.transform.position;
                    weaponOB2.transform.rotation = Quaternion.Euler(0, 0, -90);
                    bodyUpAni.SetBool("IsLookUp", true);
                    bodyUpAni.SetBool("IsLookInfront", false);
                }
            }
            else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                //아래를봄
                attackDir = 4;
                bodyUpAni.SetBool("IsLookUp", false);
                bodyUpAni.SetBool("IsLookInfront", false);
            }
            else
            {
                if (left)
                {
                    //왼쪽
                    weaponOB.transform.position = weaponMidPos.transform.position;
                    weaponOB.transform.rotation = Quaternion.Euler(0, 0, 0);
                    weaponOB2.transform.position = weaponMidPos2.transform.position;
                    weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 0);
                    attackDir = 2;
                    bodyUpAni.SetBool("IsLookInfront", true);
                }
                else
                {
                    //오른쪽
                    weaponOB.transform.position = weaponMidPos.transform.position;
                    weaponOB.transform.rotation = Quaternion.Euler(0, 0, 0);
                    weaponOB2.transform.position = weaponMidPos2.transform.position;
                    weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 0);
                    attackDir = 1;
                    bodyUpAni.SetBool("IsLookInfront", true);
                }
            }




            if (Input.GetMouseButtonDown(0) && weaponNum > 0 && ammo_P > 0 && delayTime < 0 && !isReload &&!IsFullAuto_P)
            {
                //총발사
                bullet[weaponNum - 1].GetComponent<BulletScript>().bulletDamege = damage;




                PhotonNetwork.Instantiate(weaponNum.ToString()/*이름 중요*/, shotPos.transform.position, Quaternion.identity)
                    .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir);
                ammo_P -= 1;

                bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();

                delayTime = delay_P;
            }
            else if (Input.GetMouseButton(0) && weaponNum > 0 && ammo_P > 0 && delayTime < 0 && !isReload && IsFullAuto_P)
            {
                //자동총발사
                bullet[weaponNum - 1].GetComponent<BulletScript>().bulletDamege = damage;




                PhotonNetwork.Instantiate(weaponNum.ToString()/*이름 중요*/, shotPos.transform.position, Quaternion.identity)
                    .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir);
                ammo_P -= 1;

                bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();

                delayTime = delay_P;
            }

            if (Input.GetKeyDown(KeyCode.R)|| ammo_P == 0)
            {
                //총장전
                if (ammo_P != maximumAmmo_P && isReload == false && weaponNum > 0 && delayTime < 0)
                {
                    isReload = true;
                    reLoadOB.SetActive(true);
                    reLoadBarOB.SetActive(true);
                    
                }
                
                


            }
            
            if (reLoadingTime >= 0 && isReload)
            {
                reLoadingTime -= Time.deltaTime;
            }

            reLoadBar.value = reLoadingTime / reLoadTime_P;

            if(reLoadingTime < 0 && isReload)
            {
                isReload = false;
                reLoadingTime = reLoadTime_P;
                reLoadBarOB.SetActive(false);

                ammo_P = maximumAmmo_P;
                bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                reLoadAni.SetTrigger("Ready");
                StartCoroutine(reLoad());
                
                
            }

            if (Input.GetKeyDown(KeyCode.Q) && !isReload)
            {
                //자신이 가지고있는 "무기" 버림
                if (weaponNum > 0)
                {
                    PhotonNetwork.Instantiate(weaponNum.ToString() + "weapon", gameObject.transform.position, Quaternion.identity).GetComponent<itemScript>().ammo = ammo_P;

                    weaponSprite.sprite = null;
                    weaponNum = 0;
                    itemSpriteName = "Null";

                    pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, itemSpriteName);

                    ammo_P = 0;
                    maximumAmmo_P = 0;

                    bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                }
            }
        }
        else if ((transform.position = curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    public void BodySetUp()
    {
        isBodySetUp = true;
        bodyUp.transform.position = upPosOB.transform.position;
    }
    public void BodySetDown()
    {
        isBodySetUp = false;
        bodyUp.transform.position = downPosOB.transform.position;
    }



    IEnumerator reLoad()
    {
        //재장전
        yield return new WaitForSeconds(0.4f);


        reLoadOB.SetActive(false);

    }



    [PunRPC]
    void FlipXRPC(float axis)
    {
        //방향전환
        
        if(axis == -1)
        {
            left = true;

            canvas.transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.localScale = new Vector3(1, 1, 1);

        }
        else
        {
            left = false;

            canvas.transform.localScale = new Vector3(-1, 1, 1);
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
    
    [PunRPC]
    void ChangeWeaponSpriteRPC(string itemSpriteName)
    {
        if(weaponNum > 0)
        {
            //무기Sprite바꿈
            weaponSprite.sprite = Resources.Load(itemSpriteName, typeof(Sprite)) as Sprite;
        }
        else
        {
        weaponSprite.sprite = Resources.Load(itemSpriteName, typeof(Sprite)) as Sprite;
        }
    }



    [PunRPC]
    void JumpRPC()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * jumpPow);
    }


    public void Hit()
    {
        hp -= takedDamage;
        if(hp <= 0)
        {
            if(weaponNum > 0)//죽었을때
            {
                PhotonNetwork.Instantiate(weaponNum.ToString() + "weapon", gameObject.transform.position, Quaternion.identity).GetComponent<itemScript>().ammo = ammo_P;
            }
            weaponNum = 0;
            GameObject.Find("Canvas").transform.Find("RespawnPanel").gameObject.SetActive(true);
            pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]//오브젝트 파괴
    void DestroyRPC() => Destroy(gameObject);




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(health.fillAmount);
            stream.SendNext(weaponSprite.name);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            health.fillAmount = (float)stream.ReceiveNext();
            weaponSprite.name = (string)stream.ReceiveNext();
        }
    }


}
