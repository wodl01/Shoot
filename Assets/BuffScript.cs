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

    private void Update()
    {
        if (Active)
        {
            Active = false;
            buffs[buffNum].GetComponent<Buff>().during = (duringAbility * during);
            buffs[buffNum].GetComponent<Buff>().time = buffs[buffNum].GetComponent<Buff>().during;
            buffs[buffNum].SetActive(true);
        }
    }



}
