using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraController : MonoBehaviour
{
    Quaternion cameraRotation = Quaternion.Euler(175f, 0, 180f);
    private float cameraZ = 36f;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.position = new Vector3(0, 6, cameraZ);
            transform.rotation = cameraRotation;
        }
    }

}
