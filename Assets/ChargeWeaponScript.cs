using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ChargeWeaponScript : MonoBehaviour
{
    Player player;
    [SerializeField] PhotonView pv;
    [SerializeField] float maxChargeTime;
    [SerializeField] float charge;
    [SerializeField] Image chargeImage;
    [SerializeField] bool isCharging;
    [SerializeField] bool isFullCharging;
    [SerializeField] bool isOnlyFullChargingShot;
    float scale;
    float amount;

    [SerializeField] float maxChargeDMG;
    [SerializeField] float maxChargeScale;
    [SerializeField] int maxChargeAmount;
    [SerializeField] float fireCool;

    [SerializeField] int weaponNum;
    bool isright;
    [SerializeField] ParticleSystem particle;



    float aaa;
    float aaa2;
    float aaa3;
    int dirY;
    int dirZ;
    int dir;
    bool once;
    [PunRPC]
    void BulletDirRPC(int pp, bool isRight)
    {
        //playerName = pp;
        //Debug.Log(pp);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject py in players)
        {
            player = py.GetComponent<Player>();
            if (player.pv.ViewID == pp)
            {
                once = true;
                aaa = (maxChargeDMG - 1) / maxChargeTime;
                aaa2 = (maxChargeScale - 1) / maxChargeTime;
                aaa3 = maxChargeAmount / maxChargeTime;
                isright = isRight;
                player.isCharging = true;
                return;
            }
        }
    }
    IEnumerator Fire()
    {
        if (dir == 1)
        {
            if (player.rigid.velocity.x > -8)
            {
                player.rigid.AddForce(new Vector2(-player.kickX, 0));
            }
        }
        else if (dir == 2)
        {
            if (player.rigid.velocity.x < 8)
            {
                player.rigid.AddForce(new Vector2(player.kickX, 0));
            }
        }
        else if (dir == 3)
        {
            player.rigid.AddForce(new Vector2(0, -player.kickY));
        }
        else if (dir == 4)
        {
            player.rigid.AddForce(new Vector2(0, player.kickY));
        }
        PhotonNetwork.Instantiate("Weapon" + "/" + weaponNum.ToString() + "Weapon" + "/" + weaponNum + "A"/*이름 중요*/, player.shotPos.transform.position, Quaternion.Euler(0, dirY, Random.Range(dirZ - player.bulletSpread2, dirZ + player.bulletSpread2)))
                                                    .GetComponent<PhotonView>().RPC("BulletDirRPC", RpcTarget.All, player.pv.ViewID, isright, charge + 1, scale + 1);
        amount -= 1;
        if (isright)
        {
            player.ammo_P -= 1;
        }
        else
        {
            player.ammo_P2 -= 1;
        }
        yield return new WaitForSeconds(fireCool);
        if(amount >= 0)
        {
            StartCoroutine(Fire());
        }
        else
        {
            player.isCharging = false;
            pv.RPC("Destroy", RpcTarget.AllBuffered);
        }

    }

    private void Update()
    {

        gameObject.transform.position = player.transform.position;




        if (isright)
        {
            if (Input.GetMouseButton(0))
            {

                isCharging = true;
                charge += aaa * Time.deltaTime;
                scale += aaa2 * Time.deltaTime;
                amount += aaa3 * Time.deltaTime;


                chargeImage.fillAmount = charge / (maxChargeTime * aaa);
                if (charge >= maxChargeTime * aaa)
                {
                    charge = maxChargeTime * aaa;
                    scale = maxChargeScale;
                    amount = maxChargeAmount;
                    isFullCharging = true;
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }
            }
            else
            {
                if (once)
                {
                    once = false;
                    if (player.attackDir == 1)
                    {

                        dirZ = 0;
                        dirY = 0;
                    }
                    else if (player.attackDir == 2)
                    {
                        dirZ = 0;
                        dirY = 180;
                    }
                    else if (player.attackDir == 3)
                    {
                        dirZ = 90;
                        dirY = -180;
                    }
                    else if (player.attackDir == 4)
                    {
                        dirZ = -90;
                        dirY = 0;
                    }
                    dir = player.attackDir;
                    Debug.Log(amount);
                    StartCoroutine(Fire());

                }
            }
        }
        else
        {
            if (Input.GetMouseButton(1))
            {

                isCharging = true;
                charge += aaa * Time.deltaTime;
                scale += aaa2 * Time.deltaTime;
                amount += aaa3 * Time.deltaTime;


                chargeImage.fillAmount = charge / (maxChargeTime * aaa);
                if (charge >= maxChargeTime * aaa)
                {
                    charge = maxChargeTime * aaa;
                    amount = maxChargeAmount;
                    isFullCharging = true;
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }
            }
            else
            {
                if (once)
                {
                    once = false;
                    if (player.attackDir == 1)
                    {

                        dirZ = 0;
                        dirY = 0;
                    }
                    else if (player.attackDir == 2)
                    {
                        dirZ = 0;
                        dirY = 180;
                    }
                    else if (player.attackDir == 3)
                    {
                        dirZ = 90;
                        dirY = -180;
                    }
                    else if (player.attackDir == 4)
                    {
                        dirZ = -90;
                        dirY = 0;
                    }
                    dir = player.attackDir;
                    Debug.Log(amount);
                    StartCoroutine(Fire());




                }

            }

        }

    }
    [PunRPC]
    void Destroy()
    {
        Destroy(gameObject);
    }
}
