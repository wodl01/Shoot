using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffScript : MonoBehaviour
{
    public int buffNum;
    public float during;
    public float duringAbility;
    [SerializeField] GameObject[] buffs;
    public bool Active;

    public Player player;

    public Animator darkAni;

    bool once = true;
    private void Update()
    {


        if (Active && buffNum != -1)
        {
            Active = false;
            if(buffs[buffNum].GetComponent<Buff>().time < (duringAbility * during))
            {
                buffs[buffNum].GetComponent<Buff>().player = player;
                buffs[buffNum].GetComponent<Buff>().darkEffectAni = darkAni;
                buffs[buffNum].GetComponent<Buff>().during = (duringAbility * during);//총시간
                buffs[buffNum].GetComponent<Buff>().time = buffs[buffNum].GetComponent<Buff>().during;//작용시간 = 총시간
                buffs[buffNum].SetActive(true);
                
                
            }
           
        }
    }



}
