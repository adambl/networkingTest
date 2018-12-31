using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WinLooseManager : MonoBehaviourPun
{
    public int NumOfBuildings { get; set; }

    public void OnBuildingDestroyed(GameObject destroyedBuilding)
    {
        if (--NumOfBuildings <= 0)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Looser", PhotonNetwork.NickName } },
                new Hashtable() { { "Looser", null } }
            );
        }

    }
}
