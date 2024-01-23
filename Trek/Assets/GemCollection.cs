using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GemSound;

    private void Start()
    {
        GemSound.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Gem")
        {
            GemSound.SetActive(true);
            Destroy(other.gameObject);  
        }
    }
}
