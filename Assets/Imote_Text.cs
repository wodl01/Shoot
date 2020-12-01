using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Imote_Text : MonoBehaviour
{
    [SerializeField] string[] inputMessage;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Text imoteMessage;
    [SerializeField] float upPow;
    [SerializeField] Color damageColor;
    [SerializeField] Color healColor;
    [SerializeField] Color poisonColor;
    [SerializeField] Color shildColor;
    Color color;


    bool once;
    private void Start()
    {
        rigid.velocity = new Vector2(0, upPow);

        gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("DamageDummy").transform);

        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    [PunRPC]
    void ChangeTextRPC2(int changeTextNum, int colornum)
    {
        imoteMessage.text = inputMessage[changeTextNum];
        
        if (colornum == 0)
        {
            imoteMessage.color = healColor;
        }
        else if (colornum == 1)
        {
            imoteMessage.color = damageColor;
        }
        else if (colornum == 2)
        {
            imoteMessage.color = poisonColor;
        }
        else if (colornum == 3)
        {
            imoteMessage.color = shildColor;
        }
    }
}
