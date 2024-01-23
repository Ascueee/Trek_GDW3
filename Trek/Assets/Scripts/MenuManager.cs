using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Pause Menu Vars")]
    [SerializeField] float lerpTime;
    bool lerp;

    [Header("Pause Menu Vars")]
    [SerializeField] KeyCode pauseKeyBind = KeyCode.Escape;
    [SerializeField] Image backGroundImage;
    [SerializeField] TextMeshProUGUI pauseText;
    bool inPauseMenu;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LeanTween.init();
        MenuInput();
        PauseMenu();
    }

    void MenuInput()
    {
        //checks for player input and if the menu is already paused
        if (Input.GetKeyDown(pauseKeyBind) && inPauseMenu == false)
        {
            inPauseMenu = true;//sets inPauseMenu to value
            lerp = true;
        }   
        else if (Input.GetKeyDown(pauseKeyBind) && inPauseMenu == true)
        {
            inPauseMenu = false;
            lerp = true;
        }
            
    }

    void PauseMenu()
    {
        
        if (inPauseMenu == true) {

            if (lerp == true && inPauseMenu == true)
            {
                LeanTween.value(backGroundImage.gameObject, backGroundImage.color.a, 0.24f, lerpTime).setOnUpdate(PauseLerpImage);
                LeanTween.value(pauseText.gameObject, pauseText.color.a, 1f, lerpTime).setOnUpdate(PauseLerpText);
            }
        }

        if(inPauseMenu == false)
        {
            if(lerp == true && inPauseMenu == false)
            {
                LeanTween.value(backGroundImage.gameObject, backGroundImage.color.a, 0f, lerpTime).setOnUpdate(UnPauseLerpImage);
                LeanTween.value(pauseText.gameObject, pauseText.color.a, 0f, lerpTime).setOnUpdate(UnPauseLerpText);
            }
        }
    }


    //********************************LERP FUNCTION************************************************************************
    void PauseLerpImage(float a)
    {
        var alphaChange = new Vector4(backGroundImage.color.r, backGroundImage.color.g, backGroundImage.color.b, a);

        backGroundImage.color = alphaChange;

        if (backGroundImage.color.a == 0.24f)
        {
            
            lerp = false;
        }
    }

    void PauseLerpText(float a)
    {
        var alphaChange = new Vector4(pauseText.color.r, pauseText.color.g, pauseText.color.b, a);

        pauseText.color = alphaChange;

        if (pauseText.color.a == 1f)
        {
            lerp = false;
        }
    }

    void UnPauseLerpImage(float a)
    {
        var alphaChange = new Vector4(backGroundImage.color.r, backGroundImage.color.g, backGroundImage.color.b, a);

        backGroundImage.color = alphaChange;

        if (backGroundImage.color.a == 0f)
        {

            lerp = false;
        }
    }

    void UnPauseLerpText(float a)
    {
        var alphaChange = new Vector4(backGroundImage.color.r, backGroundImage.color.g, backGroundImage.color.b, a);

        pauseText.color = alphaChange;

        if(pauseText.color.a == 1f)
        {
            lerp = false;
        }
    }
}
