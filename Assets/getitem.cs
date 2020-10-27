using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class getitem : MonoBehaviour
{
    public string weapon_number;
    [SerializeField] SpriteRenderer weapon_sprite;
    [SerializeField] PhotonView pv;
    [SerializeField] GameManager gm;


    public void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        weapon_sprite.sprite = gm.itemSprite[gm.allItemList.FindIndex(x => x.Name == weapon_number)];


        // 이미지 가져올 id 찾아야되

        // 이미지 바꿔주고 정보 ㅈㅝ야해

        // 플레이어랑 데이고 f 눌렸을때 내가 매니저에서 가저온 정보
        //플레이어로 넘겨줘야해





    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("asdasd");
            gm.GetItem(weapon_number);
            pv.RPC("destroy", RpcTarget.AllBuffered);
            
        }
    }

    [PunRPC]
    void destroy()
    {
        Destroy(gameObject);
    }
}
