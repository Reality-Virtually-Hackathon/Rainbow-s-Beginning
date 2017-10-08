using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rogue Fong, 8 October 2017

public class ToggleLayer : MonoBehaviour {

    public enum Layer { Municipal, PopupExperience };

    public static int layer;
    public static int numOfLayers = 3;

    public GameObject[] municipalMembers;
    public GameObject[] popupExperienceMembers;

    //layer 0: empty, layer 1: municipal, layer 2: popup experience

	// Use this for initialization
	void Start () {
        layer = 0;

        //if(municipalMembers == null)
        //{
            municipalMembers = GameObject.FindGameObjectsWithTag("MunicipalMember");
            foreach (GameObject member in municipalMembers)
            {
                Debug.Log(member);
                member.transform.SetParent(gameObject.transform);
            }
        //}
        //if(popupExperienceMembers == null)
        //{
            popupExperienceMembers = GameObject.FindGameObjectsWithTag("PopupExperienceMember");
            foreach (GameObject member in popupExperienceMembers)
            {
                Debug.Log(member);
                member.transform.SetParent(gameObject.transform);
            }
        //}
        Debug.Log("Layer is " + layer);
        AutoToggle();
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("tab pressed");
            Debug.Log("Layer was " +  layer);
            changeLayer();
            Debug.Log("Layer now is " + layer);

            AutoToggle();
        }
		
	}


    void ToggleMunicipalOn()
    {
        foreach (GameObject member in municipalMembers)
        {
            member.SetActive(true);
        }
    }   

    void ToggleMunicipalOff()
    { 
        foreach(GameObject member in municipalMembers)
        {
            member.SetActive(false);
        }
     }

    void TogglePopupExperienceOn()
    {
            foreach(GameObject member in popupExperienceMembers)
            {
                member.SetActive(true);
            }
    }

    void TogglePopupExperienceOff()
    {
        foreach (GameObject member in popupExperienceMembers)
        {
            member.SetActive(false);
        }
    }

    void changeLayer() //increment through layers
    {
        Debug.Log("Called changeLayer()");
        if (layer >= numOfLayers - 1)
        {
            layer = 0;
        }
        else
        {
            layer++;
        }
    }

    void AutoToggle()
    {
        Debug.Log("Toggling Layer");
        if (layer == 0)
        {
            //isMunicipal = false;
            //isPopupExperience = false;
            ToggleMunicipalOff();
            TogglePopupExperienceOff();
            Debug.Log("Everything should be off");
        }
        else if (layer == 1)
        {
            ToggleMunicipalOn();
            TogglePopupExperienceOff();
            Debug.Log("Municipal should be on, whilst popup should all be off.");
        }
        else if (layer == 2)
        {
            ToggleMunicipalOff();
            TogglePopupExperienceOn();
            Debug.Log("Municipal should all be off, whilst popup should all be on.");
        }
        else
        {
            Debug.Log("error: layer does not exist");
            layer = 0;
            ToggleMunicipalOff();
            TogglePopupExperienceOff();
        }
    }
}
