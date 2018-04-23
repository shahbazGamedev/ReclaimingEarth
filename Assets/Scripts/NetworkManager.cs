﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    [SerializeField] Text connectionText;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Camera sceneCamera;

    GameObject player;

    void Start()
    {

        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
        print("Start");
    }

    void Update()
    {
        connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    void OnJoinedLobby()
    {
        print("Joined Lobby");
        RoomOptions ro = new RoomOptions() { IsVisible = true, MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("ReclaimEarth", ro, TypedLobby.Default);
        print("Created Room");
    }

    void OnJoinedRoom()
    {
        print("Joined Room");
        StartSpawnProcess(0f);
    }

    void StartSpawnProcess(float respawnTime)
    {
        sceneCamera.enabled = true;
        StartCoroutine("SpawnPlayer", respawnTime);
    }

    IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        player = PhotonNetwork.Instantiate("Player",
                                           spawnPoints[index].position,
                                           spawnPoints[index].rotation,
                                           0);
        player.GetComponent<PlayerNetworkMover>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
    }
}