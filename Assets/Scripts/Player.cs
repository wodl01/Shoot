using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] Player player;
    public bool isMine = false;
    
    public Rigidbody2D rigid;
    public Animator ani;
    public SpriteRenderer sprite;
    public PhotonView pv;
    public GameManager gm;
    public Text nickNameText;
    public Image health;
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject DmgOB;
    [SerializeField] SpriteRenderer weaponSpriteRender;
    [SerializeField] SpriteRenderer weaponSpriteRender2;
    [SerializeField] SpriteRenderer clothesSpriteRender;
    [SerializeField] SpriteRenderer clothesDownSpriteRender;
    int clothesDownAniNum;


    [SerializeField] Canvas canvas;
    [SerializeField] Text bulletText;
    [SerializeField] Text bulletText2;

    public BuffScript buffScript;
    [SerializeField] Animator darkBuffAni;
    

    [SerializeField] GameObject reLoadOB;
    [SerializeField] GameObject reLoadBarOB;
    [SerializeField] Slider reLoadBar;

    [SerializeField] GameObject bodyUp;
    [SerializeField] Animator bodyUpAni;
    [SerializeField] GameObject upPosOB;
    [SerializeField] GameObject downPosOB;

    public GameObject weaponOB;
    public GameObject weaponOB2;
    [SerializeField] GameObject oneHandGunUpPos;
    [SerializeField] GameObject oneHandGunUpPos2;
    [SerializeField] GameObject oneHandGunMidPos;
    [SerializeField] GameObject oneHandGunMidPos2;
    [SerializeField] GameObject oneHandGunDownPos;
    [SerializeField] GameObject oneHandGunDownPos2;
    [SerializeField] GameObject oneHandSwordUpPos;
    [SerializeField] GameObject oneHandSwordUpPos2;
    [SerializeField] GameObject oneHandSwordMidPos;
    [SerializeField] GameObject oneHandSwordMidPos2;
    [SerializeField] GameObject oneHandSwordDownPos;
    [SerializeField] GameObject oneHandSwordDownPos2;

    [SerializeField] GameObject weaponReloadSpawn;
    [SerializeField] GameObject weaponReloadSpawn2;
    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>
    public float maxHpValue;
    public float hp;


    public int playerDamage;

    [SerializeField] bool isLookUp;

    public GameObject shotPos;
    public GameObject shotPos2;
    public GameObject swingPos;
    public GameObject swingPos2;

    public int spawnAttackObAmount;
    public int spawnAttackObAmount2;

    public string spawnSkillObName;
    public string spawnSkillObName2;

    public float duringAbility;

    public float blood;

    public float DecreaseTakedDamage;
    public float DecreaseTakedHeal;

    [SerializeField] float jumpPow;
    public bool isGround;
    [SerializeField] float speed;


    
    public int weaponNum;
    public int weaponNum2;
    [SerializeField] string weaponSpecies;
    [SerializeField] string weaponSpecies2;
    [SerializeField] int clothesNum;
    [SerializeField] int plusMaxHp;
    [SerializeField] float clothesPlusHp;
    public int attackCode;
    public int attackCode2;
    [SerializeField] GameObject[] bullet;
    public float bulletSpread;
    public float bulletSpread2;
    float kickXX;
    public float kickX;//2
    public float kickY;//200
    [SerializeField] int maximumAmmo_P;
    [SerializeField] int maximumAmmo_P2;
    [SerializeField] int ammo_P;
    [SerializeField] int ammo_P2;
    [SerializeField] bool IsFullAuto_P;
    [SerializeField] bool IsFullAuto_P2;
    public bool readyToAttack;
    public bool readyToAttack2;
    public float shotDelay;
    public float shotDelay2;
    public float attackSpeedAbility;
    [SerializeField] float reLoadTime_P;
    [SerializeField] float reLoadTime_P2;
    [SerializeField] float reLoadingTime;
    [SerializeField] float reLoadingTime2;
    [SerializeField] Animator reLoadAni;
    [SerializeField] bool isReload;
    [SerializeField] bool isReload2;
    public int barrierMax;
    public int barrierMax2;
    [SerializeField] float barrierAmount;
    [SerializeField] float barrierAmount2;
    public bool isShilding;
    public float shildRechargeTime;
    [SerializeField] float shildChargeMaxTime;
    public bool canUseShild;
    [SerializeField] SpriteRenderer shildSprite;
    [SerializeField] Animator shildAni;
    [SerializeField] bool isRightShild;

    public string itemSpriteName; 
    itemScript item;

    public int skillCode;

    [SerializeField] int attackDir;
    /// <summary>
    /// ////////////////////////////////////////////
    /// </summary>
    bool Once = true;


    public bool left;
    public bool isBodySetUp;
    Vector3 curPos;

    [SerializeField] float cooltime;
    [SerializeField] float cooltime2;



    [SerializeField] PassPlayerToTile PPTT;
    public bool isFallen;
    float dirZ;
    float dirY;
    void Awake()
    {
        //this.gameObject.name = Random.Range(0, 999999).ToString();
        //if( // 씬내에 같은 이름 있으면)
        //   { // 다시 돌려줌 }

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.player = this;

        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nickNameText.color = pv.IsMine ? Color.green : Color.red;

        if (pv.IsMine)
        {

            var Cm = GameObject.Find("CMcamera").GetComponent<CinemachineVirtualCamera>();
            bulletText = GameObject.FindGameObjectWithTag("BulletText").GetComponent<Text>();
            bulletText2 = GameObject.FindGameObjectWithTag("BulletText2").GetComponent<Text>();
            buffScript = GameObject.FindGameObjectWithTag("BuffPanel").GetComponent<BuffScript>();
            DmgOB = GameObject.Find("DamageDummy");
            buffScript.player = this;
            buffScript.darkAni = darkBuffAni;

            Cm.Follow = transform;
            Cm.LookAt = transform;

            PPTT = GameObject.Find("OneWay").GetComponent<PassPlayerToTile>();
            PPTT.Player = this;

            pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, weaponNum);
            pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, weaponNum2);

            isMine = true;

            StartCoroutine(ClothesHeal());
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

                itemSpriteName = other.GetComponent<getitem>().weapon_number;

                pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, weaponNum);

                readyToAttack = true;
                reLoadTime_P = item.reLoadTime;
                reLoadingTime = reLoadTime_P;
                Debug.Log("1");

                if(weaponNum >= 1000 || weaponNum < 2000)
                {
                    bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                }
                else if(weaponNum >= 2000 || weaponNum < 3000)
                {
                    bulletText.text = "∞";
                }
                else if(weaponNum >= 3000)
                {
                    bulletText.text = barrierAmount.ToString() + "/" + barrierMax.ToString();
                }


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

        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject damage = PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity);

            damage.transform.SetParent(DmgOB.transform);
            damage.transform.localScale = new Vector3(1, 1, 1);
        }



        if (pv.IsMine)
        {
            //움직임
            float axis = Input.GetAxisRaw("Horizontal");
            //Vector3 move = new Vector3(axis, 0, 0);
            if (isGround)
            {
                rigid.velocity = new Vector2(speed * axis, rigid.velocity.y);
                kickXX = 0;
            }
            else
            {
                rigid.velocity = new Vector2(speed * axis + kickXX, rigid.velocity.y);
            }
            

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
                attackDir = 3;
                bodyUpAni.SetBool("IsLookUp", true);
                bodyUpAni.SetBool("IsLookInfront", false);
                pv.RPC("ClotheUpChange", RpcTarget.All,attackDir);
                if (!left)
                {
                    
                    if(weaponSpecies == "OneHandGun" || weaponSpecies == "OneHandShild" || weaponSpecies == "TwoHandShild")
                    {
                        weaponOB.transform.position = oneHandGunUpPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 90);
                        
                    }
                    else if(weaponSpecies == "OneHandSword")
                    {
                        weaponOB.transform.position = oneHandSwordUpPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 200);
                        
                    }
                    if(weaponSpecies2 == "OneHandGun" || weaponSpecies2 == "OneHandShild" || weaponSpecies2 == "TwoHandShild")
                    {
                        weaponOB2.transform.position = oneHandGunUpPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else if(weaponSpecies2 == "OneHandSword")
                    {
                        weaponOB2.transform.position = oneHandSwordUpPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 200);
                    }

                }
                else if (left)//위를봄
                {
                    
                    if (weaponSpecies == "OneHandGun" || weaponSpecies == "OneHandShild" || weaponSpecies == "TwoHandShild")
                    {
                        weaponOB.transform.position = oneHandGunUpPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, -90);
                        
                    }
                    else if(weaponSpecies == "OneHandSword")
                    {
                        weaponOB.transform.position = oneHandSwordUpPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, -200);
                        
                    }
                    if (weaponSpecies2 == "OneHandGun" || weaponSpecies2 == "OneHandShild" || weaponSpecies2 == "TwoHandShild")
                    {
                        weaponOB2.transform.position = oneHandGunUpPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, -90);
                    }
                    else if (weaponSpecies2 == "OneHandSword")
                    {
                        weaponOB2.transform.position = oneHandSwordUpPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, -200);
                    }

                }
            }
            else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                //아래를봄
                attackDir = 4;
                bodyUpAni.SetBool("IsLookUp", false);
                bodyUpAni.SetBool("IsLookInfront", false);
                pv.RPC("ClotheUpChange", RpcTarget.All,attackDir);
                if (!left)
                {
                    if (weaponSpecies == "OneHandGun" || weaponSpecies == "OneHandShild" || weaponSpecies == "TwoHandShild")
                    {
                        weaponOB.transform.position = oneHandGunDownPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, -90);
                        
                    }
                    else if (weaponSpecies == "OneHandSword")
                    {
                        weaponOB.transform.position = oneHandSwordDownPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, -71);
                        
                    }
                    if (weaponSpecies2 == "OneHandGun" || weaponSpecies2 == "OneHandShild" || weaponSpecies2 == "TwoHandShild")
                    {
                        weaponOB2.transform.position = oneHandGunDownPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, -90);
                    }
                    else if (weaponSpecies2 == "OneHandSword")
                    {
                        weaponOB2.transform.position = oneHandSwordDownPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, -71);
                    }
                }
                if (left)//아래를봄
                {
                    if (weaponSpecies == "OneHandGun" || weaponSpecies == "OneHandShild" || weaponSpecies == "TwoHandShild")
                    {
                        weaponOB.transform.position = oneHandGunDownPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 90);
                        
                    }
                    else if (weaponSpecies == "OneHandSword")
                    {
                        weaponOB.transform.position = oneHandSwordDownPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 71);
                        
                    }
                    if (weaponSpecies2 == "OneHandGun" || weaponSpecies2 == "OneHandShild" || weaponSpecies2 == "TwoHandShild")
                    {
                        weaponOB2.transform.position = oneHandGunDownPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else if (weaponSpecies2 == "OneHandSword")
                    {
                        weaponOB2.transform.position = oneHandSwordDownPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 71);
                    }
                }
            }
            else
            {
                //중간
                bodyUpAni.SetBool("IsLookInfront", true);
                pv.RPC("ClotheUpChange", RpcTarget.All,attackDir);
                if (left)
                {
                    //왼쪽
                    attackDir = 2;
                    if (weaponSpecies == "OneHandGun" || weaponSpecies == "OneHandShild" || weaponSpecies == "TwoHandShild")
                    {
                        weaponOB.transform.position = oneHandGunMidPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 0);
                        
                    }
                    else if (weaponSpecies == "OneHandSword")
                    {
                        weaponOB.transform.position = oneHandSwordMidPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 44);
                        
                    }
                    if (weaponSpecies2 == "OneHandGun" || weaponSpecies2 == "OneHandShild" || weaponSpecies2 == "TwoHandShild")
                    {
                        weaponOB2.transform.position = oneHandGunMidPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (weaponSpecies2 == "OneHandSword")
                    {
                        weaponOB2.transform.position = oneHandSwordMidPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 44);
                    }
                }
                else//중간
                {
                    //오른쪽
                    attackDir = 1;
                    if(weaponSpecies == "OneHandGun" || weaponSpecies == "OneHandShild" || weaponSpecies == "TwoHandShild")
                    {
                        weaponOB.transform.position = oneHandGunMidPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, 0);
                        
                    }
                    else if (weaponSpecies == "OneHandSword")
                    {
                        weaponOB.transform.position = oneHandSwordMidPos.transform.position;
                        weaponOB.transform.rotation = Quaternion.Euler(0, 0, -44);
                        
                    }
                    if (weaponSpecies2 == "OneHandGun" || weaponSpecies2 == "OneHandShild" || weaponSpecies2 == "TwoHandShild")
                    {
                        weaponOB2.transform.position = oneHandGunMidPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (weaponSpecies2 == "OneHandSword")
                    {
                        weaponOB2.transform.position = oneHandSwordMidPos2.transform.position;
                        weaponOB2.transform.rotation = Quaternion.Euler(0, 0, -44);
                    }
                }
            }




            if (Input.GetMouseButtonDown(0) && weaponNum > 999 && ammo_P > 0 && readyToAttack && !isReload && !IsFullAuto_P)
            {
                //총발사
                bool isRight = true;
                if (weaponSpecies == "OneHandGun")
                {
                    StartCoroutine(FireCoolTIme(isRight));
                }
                if (attackDir == 1)
                {
                    dirZ = 0;
                    dirY = 0;
                    kickXX = -kickX;
                    Debug.Log("오늘쪽");
                }
                else if (attackDir == 2)
                {
                    dirZ = 0;
                    dirY = 180;
                    kickXX = kickX;

                }
                else if (attackDir == 3)
                {
                    if (left)
                    {
                        dirZ = 90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = 90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, -kickY));
                    Debug.Log("위");
                }
                else if (attackDir == 4)
                {
                    if (left)
                    {
                        dirZ = -90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = -90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, kickY));
                    Debug.Log("아래");
                }



                for (int i = 0; i < spawnAttackObAmount; i++)
                {
                    if (weaponNum > 999 && weaponNum < 2000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString() + "A"/*이름 중요*/, shotPos.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread, dirZ + bulletSpread)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);
                        Debug.Log(dirZ);
                    }
                    else if (weaponNum > 1999 && weaponNum < 3000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString() + "A"/*이름 중요*/, swingPos.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread, dirZ + bulletSpread)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }

                }
                if (weaponSpecies == "OneHandSword")
                {
                    pv.RPC("EmptyWeaponSpriteRPC", RpcTarget.AllBuffered, isRight);
                }


                ammo_P -= 1;


                if (weaponNum >= 1000 && weaponNum < 2000)
                {
                    bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                }
                else if (weaponNum >= 2000 && weaponNum < 3000)
                {
                    bulletText.text = "∞";
                }


                readyToAttack = false;

            }
            else if (Input.GetMouseButton(0) && weaponNum > 999 && ammo_P > 0 && readyToAttack && !isReload && IsFullAuto_P)
            {
                //자동총발사
                bool isRight = true;
                if (weaponSpecies == "OneHandGun")
                {
                    StartCoroutine(FireCoolTIme(isRight));
                }

                if (attackDir == 1)
                {
                    dirZ = 0;
                    dirY = 0;
                    kickXX = -kickX;
                    Debug.Log("오늘쪽");
                }
                else if (attackDir == 2)
                {
                    dirZ = 0;
                    dirY = 180;
                    kickXX = kickX;

                }
                else if (attackDir == 3)
                {
                    if (left)
                    {
                        dirZ = 90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = 90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, -kickY));
                    Debug.Log("위");
                }
                else if (attackDir == 4)
                {
                    if (left)
                    {
                        dirZ = -90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = -90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, kickY));
                    Debug.Log("아래");
                }

                for (int i = 0; i < spawnAttackObAmount; i++)
                {
                    if (weaponNum > 999 && weaponNum < 2000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString() + "A"/*이름 중요*/, shotPos.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread, dirZ + bulletSpread)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }
                    else if (weaponNum > 1999 && weaponNum < 3000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString() + "A"/*이름 중요*/, swingPos.transform.position, Quaternion.Euler(0, 0, Random.Range(dirZ - bulletSpread, dirZ + bulletSpread)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }
                }

                if (weaponSpecies == "OneHandSword")
                {
                    pv.RPC("EmptyWeaponSpriteRPC", RpcTarget.AllBuffered, isRight);
                }
                ammo_P -= 1;


                if (weaponNum >= 1000 && weaponNum < 2000)
                {
                    bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                }
                else if (weaponNum >= 2000 && weaponNum < 3000)
                {
                    bulletText.text = "∞";
                }

                readyToAttack = false;

            }


            if (Input.GetMouseButtonDown(1) && weaponNum2 > 999 && ammo_P2 > 0 && readyToAttack2 && !isReload2 && !IsFullAuto_P2)
            {
                //반대쪽총발사
                bool isRight = false;
                if (weaponSpecies2 == "OneHandGun")
                {
                    StartCoroutine(FireCoolTIme(isRight));
                }
                if (attackDir == 1)
                {
                    dirZ = 0;
                    dirY = 0;
                    kickXX = -kickX;
                    Debug.Log("오늘쪽");
                }
                else if (attackDir == 2)
                {
                    dirZ = 0;
                    dirY = 180;
                    kickXX = kickX;

                }
                else if (attackDir == 3)
                {
                    if (left)
                    {
                        dirZ = 90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = 90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, -kickY));
                    Debug.Log("위");
                }
                else if (attackDir == 4)
                {
                    if (left)
                    {
                        dirZ = -90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = -90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, kickY));
                    Debug.Log("아래");
                }



                for (int i = 0; i < spawnAttackObAmount; i++)
                {

                    if (weaponNum2 > 999 && weaponNum2 < 2000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString() + "A"/*이름 중요*/, shotPos2.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread2, dirZ + bulletSpread2)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }
                    else if (weaponNum2 > 1999 && weaponNum2 < 3000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString() + "A"/*이름 중요*/, swingPos2.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread2, dirZ + bulletSpread2)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }
                }

                if (weaponSpecies2 == "OneHandSword")
                {
                    pv.RPC("EmptyWeaponSpriteRPC", RpcTarget.AllBuffered, isRight);
                }
                ammo_P2 -= 1;

                if (weaponNum2 >= 1000 && weaponNum2 < 2000)
                {
                    bulletText2.text = ammo_P2.ToString() + "/" + maximumAmmo_P2.ToString();
                }
                else if (weaponNum2 >= 2000 && weaponNum2 < 3000)
                {
                    bulletText2.text = "∞";
                }


                readyToAttack2 = false;

            }
            else if (Input.GetMouseButton(1) && weaponNum2 > 999 && ammo_P2 > 0 && readyToAttack2 && !isReload2 && IsFullAuto_P2)
            {
                //반대쪽연속총발사
                bool isRight = false;
                if (weaponSpecies2 == "OneHandGun")
                {
                    StartCoroutine(FireCoolTIme(isRight));
                }
                if (attackDir == 1)
                {
                    dirZ = 0;
                    dirY = 0;
                    kickXX = -kickX;
                    Debug.Log("오늘쪽");
                }
                else if (attackDir == 2)
                {
                    dirZ = 0;
                    dirY = 180;
                    kickXX = kickX;

                }
                else if (attackDir == 3)
                {
                    if (left)
                    {
                        dirZ = 90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = 90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, -kickY));
                    Debug.Log("위");
                }
                else if (attackDir == 4)
                {
                    if (left)
                    {
                        dirZ = -90;
                        dirY = -180;
                    }
                    else
                    {
                        dirZ = -90;
                        dirY = 0;
                    }
                    rigid.AddForce(new Vector2(0, kickY));
                    Debug.Log("아래");
                }

                for (int i = 0; i < spawnAttackObAmount; i++)
                {

                    if (weaponNum2 > 999 && weaponNum2 < 2000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString() + "A"/*이름 중요*/, shotPos2.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread2, dirZ + bulletSpread2)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }
                    else if (weaponNum2 > 1999 && weaponNum2 < 3000)
                    {
                        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString() + "A"/*이름 중요*/, swingPos2.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - bulletSpread2, dirZ + bulletSpread2)))
                                                .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, attackDir, this.pv.ViewID, isRight);

                    }
                }

                if (weaponSpecies2 == "OneHandSword")
                {
                    pv.RPC("EmptyWeaponSpriteRPC", RpcTarget.AllBuffered, isRight);
                }
                ammo_P2 -= 1;




                if (weaponNum2 >= 1000 && weaponNum2 < 2000)
                {
                    bulletText2.text = ammo_P2.ToString() + "/" + maximumAmmo_P2.ToString();
                }
                else if (weaponNum2 >= 2000 && weaponNum2 < 3000)
                {
                    bulletText2.text = "∞";
                }


                readyToAttack2 = false;

            }

            if (Input.GetKeyDown(KeyCode.R)|| ammo_P == 0)
            {
                //총장전
                if (ammo_P != maximumAmmo_P && isReload == false && weaponNum > 0)
                {
                    isReload = true;
                    reLoadOB.SetActive(true);
                    reLoadBarOB.SetActive(true);
                    pv.RPC("ReloadWeaponSpriteRPC", RpcTarget.AllBuffered, itemSpriteName);
                    //PhotonNetwork.Instantiate(weaponNum.ToString() + "_Clip", weaponReloadSpawn.transform.position, Quaternion.identity);
                }
            }
            if (Input.GetKeyDown(KeyCode.R) || ammo_P2 == 0)
            {
                //총장전
                if (ammo_P2 != maximumAmmo_P2 && isReload2 == false && weaponNum2 > 0)
                {
                    isReload2 = true;
                    reLoadOB.SetActive(true);
                    reLoadBarOB.SetActive(true);
                    pv.RPC("ReloadWeaponSpriteRPC", RpcTarget.AllBuffered, itemSpriteName);
                    //PhotonNetwork.Instantiate(weaponNum2.ToString() + "_Clip", weaponReloadSpawn2.transform.position, Quaternion.identity);
                }
            }
            if (Input.GetMouseButton(0) && canUseShild)
            {
                //방패
                if (weaponNum >= 3000 && weaponNum < 5000)
                {
                    bool active = true;

                    
                    isShilding = true;
                    shildRechargeTime = 1;
                    
                    isRightShild = true;
                    pv.RPC("ShildShape", RpcTarget.AllBuffered, isShilding, isRightShild);
                }


            }
            else if (Input.GetMouseButton(1) && canUseShild)
            {
                //반대쪽 방패
                if (weaponNum2 >= 3000 && weaponNum2 < 5000)
                {
                    bool active = true;
                    isShilding = true;
                    shildRechargeTime = 1;
                    isRightShild = false;
                    pv.RPC("ShildShape", RpcTarget.AllBuffered, isShilding, isRightShild);
                }
            }
            else
            {
                bool active = false;
                pv.RPC("ShildShape", RpcTarget.AllBuffered, isShilding, isRightShild);
                isShilding = false;
            }
        
            if (reLoadingTime >= 0 && isReload)
            {
                reLoadingTime -= Time.deltaTime;
            }
            if (reLoadingTime2 >= 0 && isReload2)
            {
                reLoadingTime2 -= Time.deltaTime;
            }
            if(shildRechargeTime >= 0)
            {
                shildRechargeTime -= Time.deltaTime;
            }
            else
            {
                if(weaponNum >= 3000 && weaponNum < 5000 && barrierAmount < barrierMax)
                {
                    barrierAmount += barrierMax * shildChargeMaxTime;
                }
                if(barrierAmount > barrierMax)
                {
                    barrierAmount = barrierMax;
                    canUseShild = true;
                }
            }
            if (weaponNum >= 3000)
            {
                
                bulletText.text = barrierAmount.ToString("F0") + "/" + barrierMax.ToString("F0");
            }
            if (weaponNum2 >= 3000)
            {

                bulletText2.text = barrierAmount2.ToString("F0") + "/" + barrierMax2.ToString("F0");
            }
            reLoadBar.value = reLoadingTime / reLoadTime_P;

            if(reLoadingTime < 0 && isReload)
            {
                isReload = false;
                reLoadingTime = reLoadTime_P;
                reLoadBarOB.SetActive(false);

                ammo_P = maximumAmmo_P;
                if (weaponNum >= 1000 && weaponNum < 2000)
                {
                    bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                }
                else if (weaponNum >= 2000 && weaponNum < 3000)
                {
                    bulletText.text = "∞";
                }

                reLoadAni.SetTrigger("Ready");


            }
            if (reLoadingTime2 < 0 && isReload2)
            {
                isReload2 = false;
                reLoadingTime2 = reLoadTime_P2;
                reLoadBarOB.SetActive(false);

                ammo_P2 = maximumAmmo_P2;

                if (weaponNum2 >= 1000 && weaponNum2 < 2000)
                {
                    bulletText2.text = ammo_P2.ToString() + "/" + maximumAmmo_P2.ToString();
                }
                else if (weaponNum2 >= 2000 && weaponNum2 < 3000)
                {
                    bulletText2.text = "∞";
                }

                reLoadAni.SetTrigger("Ready");

            }

            if (Input.GetKeyDown(KeyCode.Q) && !isReload)
            {
                //자신이 가지고있는 "무기" 버림
                if (weaponNum > 0)
                {
                    PhotonNetwork.Instantiate(weaponNum.ToString() + "weapon", gameObject.transform.position, Quaternion.identity).GetComponent<itemScript>().ammo = ammo_P;

                    weaponSpriteRender.sprite = null;
                    weaponNum = 0;
                    itemSpriteName = "Null";

                    pv.RPC("ChangeWeaponSpriteRPC", RpcTarget.AllBuffered, weaponNum);

                    ammo_P = 0;
                    maximumAmmo_P = 0;

                    if (weaponNum >= 1000 && weaponNum < 2000)
                    {
                        bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
                    }
                    else if (weaponNum >= 2000 && weaponNum < 3000)
                    {
                        bulletText.text = "∞";
                    }

                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                
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
    public void ClothesDownAni0()//1
    {
        clothesDownAniNum = 6;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
    }
    public void ClothesDownAni1()//2
    {
        clothesDownAniNum = 7;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
    }
    public void ClothesDownAni2()//3
    {
        clothesDownAniNum = 8;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
    }
    public void ClothesDownAni3()//4
    {
        clothesDownAniNum = 9;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
    }
    public void ClothesDownJumpUp()//점프
    {
        clothesDownAniNum = 4;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
    }
    public void ClothesDownJumpDown()//점프다운
    {
        clothesDownAniNum = 5;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
    }
    public void ClothesDownIdle()//가만히
    {
        clothesDownAniNum = 3;
        pv.RPC("ClothesDownChange", RpcTarget.AllBuffered, clothesDownAniNum);
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
    public void ChangeWeaponSpriteRPC(int itemSpriteName)
    {
        if(weaponNum >= 1000 && weaponNum2 > 1000)
        {
            //무기Sprite바꿈
            weaponSpriteRender.sprite = Resources.Load("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString(), typeof(Sprite)) as Sprite;


            weaponSpriteRender2.sprite = Resources.Load("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString(), typeof(Sprite)) as Sprite;

        }
        else if (weaponNum >= 1000 && weaponNum2 < 1000)
        {
            weaponSpriteRender.sprite = Resources.Load("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString(), typeof(Sprite)) as Sprite;

        }
        else if(weaponNum < 1000 && weaponNum2 >= 1000)
        {
            weaponSpriteRender2.sprite = Resources.Load("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString(), typeof(Sprite)) as Sprite;

        }
        else
        {
            weaponSpriteRender.sprite = Resources.Load("Null", typeof(Sprite)) as Sprite;
            weaponSpriteRender2.sprite = Resources.Load("Null", typeof(Sprite)) as Sprite;
        }
        if (weaponNum >= 1000 && weaponNum < 2000)
        {
            bulletText.text = ammo_P.ToString() + "/" + maximumAmmo_P.ToString();
        }
        else if (weaponNum >= 2000 && weaponNum < 3000)
        {
            bulletText.text = "∞";
        }
    }
    [PunRPC]
    public void ShildShape(bool isActive, bool isright)
    {
        if (isright && isActive && canUseShild)
        {
            shildSprite.sprite = Resources.Load("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum.ToString(), typeof(Sprite)) as Sprite;
            shildAni.SetBool("ShildOn", true);
            Debug.Log("1111");
        }
        else if (!isright && isActive && canUseShild)
        {
            shildSprite.sprite = Resources.Load("Weapon" + "/" + weaponNum2.ToString() + "Weapon" + "/" + weaponNum2.ToString(), typeof(Sprite)) as Sprite;
            shildAni.SetBool("ShildOn", true);
            Debug.Log("2222");
        }
        else
        {
            shildSprite.sprite = Resources.Load("Null", typeof(Sprite)) as Sprite;
            shildAni.SetBool("ShildOn", false);
            Debug.Log("3333");
        }
    }
    IEnumerator FireCoolTIme(bool isright)
    {
        
        yield return new WaitForSeconds(isright ? shotDelay : shotDelay2);
        if (isright)
        {
            readyToAttack = true;
        }
        else
        {
            readyToAttack2 = true;
        }
    }

    [PunRPC]
    void EmptyWeaponSpriteRPC(bool isright)
    {
        if (isright)
        {
            weaponSpriteRender.sprite = Resources.Load("Null", typeof(Sprite)) as Sprite;
        }
        else
        {
            weaponSpriteRender2.sprite = Resources.Load("Null", typeof(Sprite)) as Sprite;
        }
    }
    [PunRPC]
    void ClotheUpChange(int dir)
    {

        if (attackDir == 1 || attackDir == 2) dir = 0;
        else if (attackDir == 3) dir = 1;
        else if (attackDir == 4) dir = 2;
        clothesSpriteRender.sprite = Resources.Load(clothesNum + "Clothe" + "/" + dir, typeof(Sprite)) as Sprite;
        Debug.Log(clothesNum + "Clothe" + "/" + dir);
        

    }
    [PunRPC]
    void ClothesDownChange(int num)
    {
        clothesDownSpriteRender.sprite = Resources.Load(clothesNum + "Clothe" + "/" + num, typeof(Sprite)) as Sprite;
    }

    /*[PunRPC]
    void ReloadWeaponSpriteRPC(string itemSpriteName)
    {
        if (weaponNum > 0)
        {
            //무기Sprite바꿈
            weaponSpriteRender.sprite = Resources.Load(itemSpriteName + "_RE", typeof(Sprite)) as Sprite;
        }
        else
        {
            weaponSpriteRender.sprite = Resources.Load(itemSpriteName + "_RE", typeof(Sprite)) as Sprite;
        }
    }*/



    [PunRPC]
    void JumpRPC()
    {
        //rigid.velocity = Vector2.zero;
        isGround = false;
        rigid.AddForce(Vector2.up * jumpPow);
    }

    IEnumerator ClothesHeal()
    {
        
        yield return new WaitForSeconds(5f);
        int colorNum = 0;
        PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, clothesPlusHp * DecreaseTakedHeal, colorNum);
        if (maxHpValue < hp + (clothesPlusHp * DecreaseTakedHeal))//오버
        {
            hp = maxHpValue;
        }
        else
        {
            hp += clothesPlusHp * DecreaseTakedHeal;
        }

        StartCoroutine(ClothesHeal());
    }
    [PunRPC]
    public void AttackHeal(float takingDmg)
    {

        if (pv.IsMine)
        {
                    //int colorNum = 0;
        Debug.Log("자힐");
        float healAmount = takingDmg * blood;
            if (maxHpValue < hp + (takingDmg * DecreaseTakedHeal))
            {
                hp = maxHpValue;
                PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, healAmount, 0);
                Debug.Log("no");
            }
            else
            {
                hp += healAmount * DecreaseTakedHeal;
                PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, healAmount, 0);
                Debug.Log("yes");
            }
        }
        
        
    }
   
    public void Hit(float takedDmg, int colorNum, bool canDecrease)
    {
        if (pv.IsMine)
        {
            float finalDamage;
            if (canDecrease && !isShilding)
            {
                finalDamage = takedDmg / (1 + DecreaseTakedDamage);
            }
            else
            {
                finalDamage = takedDmg;
            }
            if (isShilding && canUseShild)//방패로 막았을때
            {
                colorNum = 3;
                barrierAmount -= finalDamage;
                shildRechargeTime = 1;
                PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, finalDamage, colorNum);
                if (barrierAmount <= 0)
                {
                    barrierAmount = 0;
                    canUseShild = false;
                    PhotonNetwork.Instantiate("ImoteText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC2", RpcTarget.All, 0, colorNum);
                }
            }
            else
            {
                hp -= Mathf.Round(finalDamage);
                PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.All, finalDamage, colorNum);
            }



            if (hp <= 0)
            {
                for (int i = 0; i < buffScript.buffs.Length; i++)
                {
                    buffScript.buffs[i].SetActive(false);
                }
                GameObject.Find("MainCanvas").transform.Find("RespawnPanel").gameObject.SetActive(true);
                pv.RPC("DestroyRPC", RpcTarget.AllBuffered);
            }
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
            //stream.SendNext(weaponSpriteRender.name);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            health.fillAmount = (float)stream.ReceiveNext();
            //weaponSpriteRender.name = (string)stream.ReceiveNext();
        }
    }


}
