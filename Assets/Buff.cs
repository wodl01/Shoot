using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    [SerializeField] int buffNum;

    public float during;
    public float time;

    [SerializeField] Image buffImage;

    public Player player;

    public Animator darkEffectAni;

    [SerializeField] int takingDmgPoison;
    [SerializeField] float increase_takingDmgPoison_Time;

    bool once = true;
    private void Update()
    {
        buffImage.fillAmount = time / during;
        if(time >= 0)
        {
            time -= Time.deltaTime;
        }
        if(time < 0)
        {

            gameObject.SetActive(false);
        }
        if(buffNum == 0)
        {
            increase_takingDmgPoison_Time -= Time.deltaTime;
            if(increase_takingDmgPoison_Time <= 0)
            {
                increase_takingDmgPoison_Time = 4;
                takingDmgPoison += 1;

            }
        }
    }




    
    private void OnEnable()
    {

        if(buffNum == 0)
        {
            takingDmgPoison = 1;
            StartCoroutine(poisonLv1());
            player.DecreaseTakedHeal = 0.8f;

        }
        if(buffNum == 1)
        {
            darkEffectAni.SetInteger("BuffLv", 1);
        }
        if (buffNum == 2)
        {
            darkEffectAni.SetInteger("BuffLv", 2);
        }
        if (buffNum == 3)
        {
            darkEffectAni.SetInteger("BuffLv", 3);
        }
        if (buffNum == 4)
        {
            darkEffectAni.SetInteger("BuffLv", 4);
        }
    }
    private void OnDisable()
    {
        if (buffNum == 0)
        {
            player.DecreaseTakedHeal = 1f;
        }
        if (buffNum == 1 || buffNum == 2 || buffNum == 3 || buffNum == 4)
        {
            darkEffectAni.SetInteger("BuffLv", 0);
        }
        time = 0;
    }

    IEnumerator poisonLv1()
    {
        if(time > 0)
        {
            if(takingDmgPoison < player.hp && player.hp != 1)
            {
                player.Hit(takingDmgPoison, 2, false, false);
                Debug.Log(takingDmgPoison + "111111111");
            }
            else if(takingDmgPoison >= player.hp && player.hp != 1)
            {
                player.Hit(player.hp -1, 2, false, false);
                Debug.Log(player.hp - 1 + "2222222222");
            }
            else
            {
                Debug.Log("3");
            }
            yield return new WaitForSeconds(0.2f);

            StartCoroutine(poisonLv1());

            
        }
        /*
        if (Mathf.Round(player.maxHpValue * 0.03f) < player.hp && player.hp != 1)
        {
            player.Hit(player.maxHpValue * 0.03f, 2, false);
            Debug.Log(player.maxHpValue * 0.03f + "111111111");
        }
        else if (Mathf.Round(player.maxHpValue * 0.03f) >= player.hp && player.hp != 1)
        {
            player.Hit(player.hp - 1, 2, false);
            Debug.Log(player.hp - 1 + "2222222222");
        }
        else
        {
            Debug.Log("3");
        }
        yield return new WaitForSeconds(0.3f);

        StartCoroutine(poisonLv1());*/
    }

}
