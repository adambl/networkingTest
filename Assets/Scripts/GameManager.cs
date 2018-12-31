using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    GameObject missleLauncherPrefab;
    Vector3 masterPosition = new Vector3(0f, 0.5f, -30f);
    Vector3 secondPlayerPosition = new Vector3(0f, 0.5f, 30f); 

    public void Start()
    {
        Debug.Log(string.Format("In GameManager. IsMasterClient: {0}", PhotonNetwork.IsMasterClient));

        Vector3 launcherPosition = masterPosition;
        if (!PhotonNetwork.IsMasterClient)
        {
            launcherPosition = secondPlayerPosition;
        }

        GameObject missleLauncher = PhotonNetwork.Instantiate(
            "MissleLauncher", 
            launcherPosition, 
            Quaternion.identity);
    }

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    #region Photon Callbacks
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        Debug.LogFormat("OnPhotonCustomRoomPropertiesChanged() {0}", propertiesThatChanged.ToString()); // custom room properties changed
        //Todo - respond to loose indication
    }

    #endregion

    //dfd
    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}




