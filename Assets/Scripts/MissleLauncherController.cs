using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissleLauncherController : MonoBehaviourPun
{
    private GameManager gameManager;

    public GameObject misslePrefab;

    private Vector3 initialMousePressPosition = Vector3.zero;

    float direction = 1f;

    Color launcherColor = Color.green;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = (GameManager)GameObject.Find("GameManager").GetComponent<GameManager>();

        if ((!PhotonNetwork.IsMasterClient && photonView.IsMine) ||
            (PhotonNetwork.IsMasterClient && !photonView.IsMine))
        {
            direction = -1f;
            launcherColor = Color.blue;
        }
        //Set the color - master is green, other player blue
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", launcherColor);

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine ||
            gameManager.GameOver ||
            EventSystem.current.IsPointerOverGameObject())
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
        return new Vector3(diff.x * 10 * this.direction, diff.y * 10, diff.y * 10 * this.direction);
    }

    private Vector3 CalcMissleInitializationPosition(Vector3 position)
    {
        return position + new Vector3(0, 2, 2 * this.direction);
    }
}
