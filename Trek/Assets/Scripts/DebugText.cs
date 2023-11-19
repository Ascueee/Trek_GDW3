using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DebugText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI velocityText;
    [SerializeField] PlayerMovement pm;
    //[SerializeField] PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocityText.text = "Player Velocity: " + pm.GetVelocity();
    }
}
