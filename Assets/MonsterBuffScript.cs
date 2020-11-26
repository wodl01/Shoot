using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterBuffScript : MonoBehaviour
{
    [SerializeField] MonsterScript MS;
    [SerializeField] Player player;
    public bool isMyAttack;
    public int buffNum;
    public bool active;
    public float time;

    [SerializeField] float poisontime;
    [SerializeField] bool isPoising;
    [SerializeField] float increase_takingDmgPoison_Time;
    [SerializeField] float poisonDmg;

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject py in players)
        {

            player = py.GetComponent<Player>();
            /*if (player.pv.ViewID == pp)
            {
                Destroy(gameObject, 3f);

                return;
            }*/
        }
    }
    private void Update()
    {
        if (active)
        {
            active = false;
            if(buffNum == 0)
            {
                if(poisontime < time)
                {
                    poisontime = time;
                }
                if (!isPoising)
                {
                    poisonDmg = 1;
                    StartCoroutine(poison());
                }
                
            }
        }
        if(poisontime >= 0)
        {
            poisontime -= Time.deltaTime;
            increase_takingDmgPoison_Time -= Time.deltaTime;
            if (increase_takingDmgPoison_Time <= 0)
            {
                increase_takingDmgPoison_Time = 4;
                poisonDmg += 1;

            }
            isPoising = true;
        }
        else
        {
            isPoising = false;
        }
    }
    IEnumerator poison()
    {
        if (poisonDmg < MS.monsterHp && MS.monsterHp != 1)
        {
            MS.Hit(isMyAttack ,poisonDmg, 2);
            Debug.Log(poisonDmg + "111111111");
        }
        else if (poisonDmg >= MS.monsterHp && MS.monsterHp != 1)
        {
            MS.Hit(isMyAttack ,MS.monsterHp - 1, 2);
            Debug.Log(MS.monsterHp - 1 + "2222222222");
        }
        else
        {
            Debug.Log("3");
        }
        //PhotonNetwork.Instantiate("DamageText", gameObject.transform.position, Quaternion.identity).GetComponent<PhotonView>().RPC("ChangeTextRPC", RpcTarget.AllBuffered, 1, 2);
        yield return new WaitForSeconds(0.2f);

        if (isPoising)
        {
            StartCoroutine(poison());
        }
        
    }
}
