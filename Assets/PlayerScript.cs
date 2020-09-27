using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] Rigidbody2D playerRigid;
    [SerializeField] Animator playerAni;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] PhotonView photonView;
    [SerializeField] Text NickNameText;
    [SerializeField] Image health;

    [SerializeField] bool isGround;
    Vector3 curPos;

    void Awake()
    {
        NickNameText.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        NickNameText.color = photonView.IsMine ? Color.green : Color.red;
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }


}
