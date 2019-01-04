using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class BuildingController : MonoBehaviourPun
{
    public float buildingHealth = 1;

    public void DestroySelf()
    {
        PhotonNetwork.Destroy(this.gameObject);
        //TODO - need to add explosion 
    }

    internal void InflictDamage(float damage)
    {
        this.buildingHealth -= damage;
        Debug.LogFormat("Health after inflicted damage amount {0}: {1}", damage, this.buildingHealth);
    }

    internal bool IsDestroyed()
    {
        Debug.LogFormat("Building health in IsDestroyed: {0}", this.buildingHealth);
        return this.buildingHealth <= 0;
    }
}
