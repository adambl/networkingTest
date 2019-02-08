﻿using Photon.Pun;
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
    private Rigidbody rigidbody;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
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
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
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
