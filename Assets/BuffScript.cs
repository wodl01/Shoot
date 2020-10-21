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



    
    private void Update()
    {
        if (Active)
        {
            Active = false;
            if(buffs[buffNum -1].GetComponent<Buff>().time < (duringAbility * during))
            {
                buffs[buffNum - 1].GetComponent<Buff>().player = player;
                buffs[buffNum -1].GetComponent<Buff>().during = (duringAbility * during);//총시간
                buffs[buffNum -1].GetComponent<Buff>().time = buffs[buffNum -1].GetComponent<Buff>().during;//작용시간 = 총시간
                buffs[buffNum -1].SetActive(true);
                
                
            }
           
        }
    }



}
