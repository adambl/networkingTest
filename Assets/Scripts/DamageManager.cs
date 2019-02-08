using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

public class TargetDamage
{
    public GameObject Target { get; set; }
    public float Damage { get; set; }

    public TargetDamage(GameObject target, float damage)
    {
        Target = target;
        Damage = damage;
    }
}

public class DamageManager : MonoBehaviourPun
{
    public int NumOfBuildings { get; set; }

    public void OnBuildingDestroyed(BuildingController destroyedBuilding)
    {
        Debug.LogFormat("Number of buildings: {0}. destroyed building: {1}", NumOfBuildings, destroyedBuilding);
        if (--NumOfBuildings <= 0)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Lost", PhotonNetwork.NickName } },
                new Hashtable() { { "Lost", null } }
            );
            Debug.LogFormat("Set custom properties: {0}. {1}", "Winner", PhotonNetwork.NickName);
        }
    }

    private List<TargetDamage> GetTargetsDamages(MissleController missle, GameObject hit)
    {
        return GameObject.FindGameObjectsWithTag("Building")
            .Where(x => PhotonView.Get(x).IsMine).Where(building => IsInBlastZone(building, missle))
            .Select(building => new TargetDamage(building, GetBlastDamage(building, missle))).ToList();
    }

    private static float GetBlastDamage(GameObject building, MissleController missle)
    {
        float distance = Vector3.Distance(missle.transform.position, building.transform.position);
        return (1 - distance / missle.blastRadius) * missle.maxBlastDamage;
    }

    private bool IsInBlastZone(GameObject building, MissleController missle)
    {
        return (Vector3.Distance(missle.transform.position, building.transform.position) < missle.blastRadius);
    }

    public void ApplyDamage(MissleController  missle, GameObject hit)
    {
        GetTargetsDamages(missle, hit).ForEach(obj =>
        {
            Debug.LogFormat("Inflicting damage on {0}", obj.Target);
            BuildingController hitBuilding = obj.Target.GetComponentInChildren(typeof(BuildingController)) as BuildingController;
            hitBuilding.InflictDamage(obj.Damage);
            if (hitBuilding.IsDestroyed())
            {
                Debug.LogFormat("Building destroyed {0}", obj.Target);
                hitBuilding.DestroySelf();
                OnBuildingDestroyed(hitBuilding);
            }
        });
    }
}
