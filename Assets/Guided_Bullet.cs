using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Guided_Bullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject target;
    [SerializeField] Rigidbody2D rigid;
    public int rotateSpeed;
    public int playerViewId;
    public int bulletDir;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Monster")
        {
            target = other.gameObject;
        }
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PhotonView>().ViewID != playerViewId)
        {
            target = other.gameObject;
        }
    }
    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            target = null;
        }
    }*/

    private void Update()
    {
        if(target != null)
        {
            Vector2 dir = bullet.transform.position - target.transform.position;

            dir.Normalize();

            float cross = Vector3.Cross(dir, bullet.transform.right).z;

            rigid.angularVelocity = cross * rotateSpeed;
        }
    }
}
