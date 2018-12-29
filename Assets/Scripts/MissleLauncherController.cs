using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleLauncherController : MonoBehaviourPun
{
    public GameObject misslePrefab;

    private Vector3 initialMousePressPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (Input.GetButtonDown("Fire1") && initialMousePressPosition == Vector3.zero)
        {
            Debug.Log(Input.mousePosition);
            initialMousePressPosition = Input.mousePosition;
        } else if (Input.GetButtonUp("Fire1") && initialMousePressPosition != Vector3.zero)
        {
            //Reset the mouse press
            Vector3 startPressPosition = initialMousePressPosition;
            initialMousePressPosition = Vector3.zero;

            Vector3 diff = Input.mousePosition - startPressPosition;
            if (diff.y > 0)
            {
                InstantiateMissle(diff);
            }

        }
    }

    private void InstantiateMissle(Vector3 diff)
    {
        Vector3 missleInitializationPosition = CalcMissleInitializationPosition(this.transform.position);
        GameObject instantiatedMissle = PhotonNetwork.Instantiate(misslePrefab.name, missleInitializationPosition, Quaternion.identity);
        Vector3 forceOnMissle = CalcForceOnInstantiatedMissle(diff);
        instantiatedMissle.GetComponent<Rigidbody>().AddForce(forceOnMissle);
    }

    private Vector3 CalcForceOnInstantiatedMissle(Vector3 diff)
    {
        return new Vector3(diff.x * 10, diff.y * 10, diff.y * 10);
    }

    private Vector3 CalcMissleInitializationPosition(Vector3 position)
    {
        return position + new Vector3(0, 2, 2);
    }
}
