using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : MonoBehaviourPun
{
    private GameObject plane;
    public void Start()
    {
        plane = GameObject.FindWithTag("GamePlayPlane");
    }

    private void DestroySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
        //TODO - need to add explosion 
    }
    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        DestroySelf();
    }

    public void Update()
    {
        if (IsOutOfBounds())
        {
            DestroySelf();
        }
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
