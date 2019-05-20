using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissleController : MonoBehaviourPun
{
    public float maxBlastDamage = 1f;

    private GameObject plane;
    public float blastRadius = 5f;

    public DamageManager damageManager;


public GameObject damageRadiusPrefab;

    public void Start()
    {
        Rigidbody rbody = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rbody.velocity);
        plane = GameObject.FindWithTag("GamePlayPlane");
        damageManager = (DamageManager)GameObject.Find("DamageManager").GetComponent<DamageManager>();
    }

    private void DestroySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
        //TODO - need to add explosion 
    }



    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            //show the blast radius
            GameObject damage = Instantiate(damageRadiusPrefab, collision.transform.position, Quaternion.identity);
            damage.transform.localScale = new Vector3(blastRadius, 0, blastRadius);

            //freeze and destroy the missle
            Rigidbody rbody = GetComponent<Rigidbody>(); 
            rbody.constraints = RigidbodyConstraints.FreezeAll;
            DestroySelf();

        }
        else
        {
            damageManager.ApplyDamage(this, collision.gameObject);
        }
    }

    public void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (IsOutOfBounds())
        {
            DestroySelf();
            return;
        }
        Rigidbody rbody = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rbody.velocity);
    }

    private bool IsOutOfBounds()
    {
        if (transform.position.x >= plane.transform.localScale.x * 10 / 2 ||
            transform.position.x <= plane.transform.localScale.x * -10 / 2 ||
            transform.position.z >= plane.transform.localScale.z * 10 / 2 ||
            transform.position.z <= plane.transform.localScale.z * -10 / 2
        )
        {
            return true;
        }
        return false;
    }
}
