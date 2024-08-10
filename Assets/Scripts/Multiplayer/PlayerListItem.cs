using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public Text PlayerUsername;

    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        PlayerUsername.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    /*public override void OnLeftRoom()
    {
        Destroy(gameObject) ;
    }*/
}
