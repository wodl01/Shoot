using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    [SerializeField]int buffNum;

    public float during;
    public float time;

    [SerializeField] Image buffImage;

    public Player player;

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

        if(buffNum == 1)
        {
            StartCoroutine(poisonLv1());
            Debug.Log("dd");
        }
    }
    IEnumerator poisonLv1()
    {
        if(time > 0)
        {
            player.hp -= 10f;
            yield return new WaitForSeconds(1f);
            StartCoroutine(poisonLv1());
        }
        
    }

}
