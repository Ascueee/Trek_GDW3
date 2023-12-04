using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] GameObject[] powerUps;
    [SerializeField] Transform powerUpholder;
    [SerializeField] LevelManager lm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "GrapplePickUp")
        {
            powerUps[0].gameObject.SetActive(true);
            powerUps[1].gameObject.SetActive(false);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "PlatformPickUp")
        {
            powerUps[0].gameObject.SetActive(false);
            powerUps[1].gameObject.SetActive(true);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Gem")
        {

            var gameObject = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().numOfGems++;
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "CheckPoint")
        {
            var gameObject = GameObject.FindGameObjectWithTag("CheckPointManager");
            gameObject.GetComponent<CheckPointSystem>().TurnOffCollider();
            gameObject.GetComponent<CheckPointSystem>().checkpointIndex++;

        }

        if(other.gameObject.tag == "Death")
        {

            var gameObject = GameObject.FindGameObjectWithTag("CheckPointManager");
            gameObject.GetComponent<CheckPointSystem>().CheckIfDead();
        }
    }
}
