using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuffScript : MonoBehaviour
{
    [SerializeField] MonsterScript MS;
     
    public int buffNum;
    public bool active;
    public float time;

    [SerializeField] float poisontime;
    [SerializeField] bool isPoising;
    
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
                    StartCoroutine(poison());
                }
                
            }
        }
        if(poisontime >= 0)
        {
            poisontime -= Time.deltaTime;
            isPoising = true;
        }
        else
        {
            isPoising = false;
        }
    }
    IEnumerator poison()
    {
        MS.Hit(1, 2);
        yield return new WaitForSeconds(0.2f);

        if (isPoising)
        {
            StartCoroutine(poison());
        }
        
    }
}
