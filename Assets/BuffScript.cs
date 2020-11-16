using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffScript : MonoBehaviour
{
    public int buffNum;
    public float during;
    public float duringAbility;
    public GameObject[] buffs;
    public bool Active;
    [SerializeField] Buff activedBuffScript;

    public Player player;

    public Animator darkAni;


    bool Can;
    private void Update()
    {


        if (Active && buffNum != -1)
        {
            Active = false;
            activedBuffScript = buffs[buffNum].GetComponent<Buff>();
            if (activedBuffScript.time < (duringAbility * during))
            {

                Can = true;

                if (buffNum == 1 || buffNum == 2 || buffNum == 3 || buffNum == 4)
                {
                    if (buffNum == 4)
                    {
                        buffs[1].SetActive(false);
                        //buffs[1].GetComponent<Buff>().time = 0;
                        buffs[2].SetActive(false);
                        //buffs[2].GetComponent<Buff>().time = 0;
                        buffs[3].SetActive(false);
                        //buffs[3].GetComponent<Buff>().time = 0;

                        Can = true;
                    }
                    if (buffNum == 3)
                    {
                        buffs[1].SetActive(false);
                        //buffs[1].GetComponent<Buff>().time = 0;
                        buffs[2].SetActive(false);
                        //buffs[2].GetComponent<Buff>().time = 0;
                        if (buffs[4].GetComponent<Buff>().time <= 0)
                        {
                            Can = true;
                        }
                        else
                        {
                            Can = false;
                        }
                    }
                    if (buffNum == 2)
                    {
                        buffs[1].SetActive(false);
                        //buffs[1].GetComponent<Buff>().time = 0;
                        if (buffs[4].GetComponent<Buff>().time <= 0 && buffs[3].GetComponent<Buff>().time <= 0)
                        {
                            Can = true;
                        }
                        else
                        {
                            Can = false;
                        }
                    }
                    if (buffNum == 1)
                    {
                        if (buffs[4].GetComponent<Buff>().time <= 0 && buffs[3].GetComponent<Buff>().time <= 0 && buffs[2].GetComponent<Buff>().time <= 0)
                        {
                            Can = true;
                        }
                        else
                        {
                            Can = false;
                        }
                    }
                }
                if (Can)
                {
                    activedBuffScript.player = player;
                    activedBuffScript.darkEffectAni = darkAni;
                    activedBuffScript.during = (duringAbility * during);//총시간
                    activedBuffScript.time = buffs[buffNum].GetComponent<Buff>().during;//작용시간 = 총시간
                    buffs[buffNum].SetActive(true);
                }
                

            }
           
        }
    }



}
