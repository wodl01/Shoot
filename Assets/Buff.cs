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




    }
    private void OnEnable()
    {

        if(buffNum == 0)
        {
            StartCoroutine(poisonLv1());
            player.DecreaseTakedHeal = 0.8f;

        }
        if(buffNum == 1)
        {
            darkEffectAni.SetInteger("BuffLv", 1);
        }
    }
    private void OnDisable()
    {
        if (buffNum == 0)
        {
            player.DecreaseTakedHeal = 1f;
        }
        if(buffNum == 1)
        {
            darkEffectAni.SetInteger("BuffLv", 0);

        }
    }
    IEnumerator poisonLv1()
    {
        if(time > 0)
        {
            if(player.maxHpValue * 0.003f < player.hp && player.hp != 1)
            {
                player.Hit(player.maxHpValue * 0.03f, 2, false);
                Debug.Log("1111");
            }
            else if(player.maxHpValue * 0.003f >= player.hp && player.hp != 1)
            {
                player.Hit(player.hp -1, 2, false);
                Debug.Log("2222");
            }
            else
            {
                Debug.Log("3");
            }
            yield return new WaitForSeconds(0.3f);

            StartCoroutine(poisonLv1());

            
        }
        
    }

}
