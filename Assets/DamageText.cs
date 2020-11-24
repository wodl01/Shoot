using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DamageText : MonoBehaviour
{
    [SerializeField] int inputNum;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Text damage;
    [SerializeField] float xRange;
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    [SerializeField] Color damageColor;
    [SerializeField] Color healColor;
    [SerializeField] Color poisonColor;
    [SerializeField] Color shildColor;
    Color color;


    bool once;
    private void Start()
    {
        rigid.velocity = new Vector2(Random.Range(-xRange, xRange), Random.Range(minHeight, maxHeight));

        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("DamageDummy").transform);

        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    [PunRPC]
    void ChangeTextRPC(float changeNum, int colornum)
    {
        damage.text = Mathf.Round(changeNum).ToString();
        if(colornum == 0)
        {
            damage.color = healColor;
        }
        else if(colornum == 1)
        {
            damage.color = damageColor;
        }
        else if(colornum == 2)
        {
            damage.color = poisonColor;
        }
        else if(colornum == 3)
        {
            damage.color = shildColor;
        }
    }
}
