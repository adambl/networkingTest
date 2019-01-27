using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    Vector3 masterPosition = new Vector3(0f, 0.5f, -30f);
    Vector3 secondPlayerPosition = new Vector3(0f, 0.5f, 30f);

    DamageManager damageManager;

    public GameObject gameResultCanvas;
    public Text gameResultText;

    public string OtherPlayerNickname { get; private set; }
    public bool GameOver { get; private set; }

    private string RetrieveOtherPlayerNickname()
    {
        if (PhotonNetwork.PlayerListOthers == null || PhotonNetwork.PlayerListOthers.Length == 0)
        {
            return null;
        }

        return PhotonNetwork.PlayerListOthers[0].NickName;
    }

    public void Start()
    {
        Debug.Log(string.Format("In GameManager. IsMasterClient: {0}", PhotonNetwork.IsMasterClient));
        //gameResultCanvas.SetActive(false);
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Lost", null } });

        OtherPlayerNickname = RetrieveOtherPlayerNickname();
         
        Vector3 launcherPosition = masterPosition;
        if (!PhotonNetwork.IsMasterClient)
        {
            launcherPosition = secondPlayerPosition;
        }

        GameObject missleLauncher = PhotonNetwork.Instantiate(
            "MissleLauncher", 
            launcherPosition, 
            Quaternion.identity);

        GameObject building = PhotonNetwork.Instantiate(
            "Building",
            launcherPosition + new Vector3(3,0,0),
            Quaternion.identity);

        damageManager = (DamageManager)GameObject.Find("DamageManager").GetComponent<DamageManager>();
        //TODO - count adversary buildings and not your own. currently it is the same.
        damageManager.NumOfBuildings = 1;
    }

    #region Photon Callbacks
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }

        if (!GameOver)
        {
            Debug.LogFormat("Ending game in OnPlayerLeftRoom. Setting lost party to {0}", other.NickName);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Lost", other.NickName } },
                new Hashtable() { { "Lost", null } }
            );
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.LogFormat("OnPhotonCustomRoomPropertiesChanged() {0}", propertiesThatChanged.ToString()); // custom room properties changed
        //respond to lost indication - Finish the game with winner notification and exit button
        if (propertiesThatChanged.ContainsKey("Lost") && !string.IsNullOrEmpty((string)propertiesThatChanged["Lost"]))
        {
            OnLostDeclared((string)propertiesThatChanged["Lost"]);

        }
    }

    private void OnLostDeclared(string lostNickname)
    {
        GameOver = true;
        string winnerNickname = GetWinnerNickname(lostNickname);
        gameResultText.text = string.Format("{0} Won", winnerNickname);
        gameResultCanvas.SetActive(true);
    }

    private string GetWinnerNickname(string lostNickname)
    {
        if (PhotonNetwork.NickName == lostNickname)
        {
            return OtherPlayerNickname;
        }

        return PhotonNetwork.NickName;
    }

   

    #endregion
}




