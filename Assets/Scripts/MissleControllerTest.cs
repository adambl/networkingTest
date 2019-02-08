using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissleControllerTest : MonoBehaviour
{
    public float maxBlastDamage = 1f;

    private GameObject plane;
    public float blastRadius = 5f;
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        plane = GameObject.FindWithTag("GamePlayPlane");
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
        //TODO - need to add explosion 
    }



    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        DestroySelf();
    }

    public void Update()
    {
        if (IsOutOfBounds())
        {
            DestroySelf();
            return;
        }
        transform.rotation = Quaternion.LookRotation(rb.velocity);
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
