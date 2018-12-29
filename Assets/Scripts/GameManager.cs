using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    GameObject missleLauncherPrefab;

    public void Start()
    {
        PhotonNetwork.Instantiate(missleLauncherPrefab.name, Vector3.zero, Quaternion.identity);
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


    #endregion

    //dfd
    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}




