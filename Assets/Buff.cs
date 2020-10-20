using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    public int buffNum;

    public float during;
    public float time;

    [SerializeField] Image buffImage;


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

}
