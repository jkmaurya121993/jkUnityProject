using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class PhotonVoiceChat : MonoBehaviourPunCallbacks
{
    public bool AutoConnect = true;
    public GameObject audioscource1;


    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;


    public void Start()
    {
        if (this.AutoConnect)
        {
            this.ConnectNow();
        }
    }

    public void ConnectNow()
    {
        Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
    }


    // below, we implement some callbacks of the Photon Realtime API.
    // Being a MonoBehaviourPunCallbacks means, we can override the few methods which are needed here.


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnJoinedRoom()
    {
       // audioscource1.SetActive(true);
        InstantiatPlayer(audioscource1);
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
    }
    public GameObject player;
    GameObject localObject;
    void InstantiatPlayer(GameObject player)
    {
        var position = Vector3.zero;
        var rotation = Quaternion.identity;
        if (localObject != null)
        {
            position = new Vector3(5, 5, 5);//localObject.transform.position;
            rotation = localObject.transform.rotation;
            PhotonNetwork.Destroy(localObject);
        }
        localObject = PhotonNetwork.Instantiate(player.name, position, rotation);
    }
}
