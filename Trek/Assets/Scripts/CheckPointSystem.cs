using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSystem : MonoBehaviour
{
    [SerializeField] GameObject[] checkPoints;
    [SerializeField] Transform[] respawnPos;
    [SerializeField] Transform playerObj;
    public int checkpointIndex;
    bool playerDead;


    //if the player hits the checkpoint then it turns of the collider so the player cannot touch it again
    public void TurnOffCollider()
    {
        checkPoints[checkpointIndex].GetComponent<BoxCollider>().enabled = false;
    }
    public void CheckIfDead()
    {
        playerDead = true;

        if(playerDead == true)
        {
            playerObj.position = respawnPos[checkpointIndex - 1].position;
            playerDead = false;
        }
    }
}
