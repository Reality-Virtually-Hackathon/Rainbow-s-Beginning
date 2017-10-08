using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rogue Fong, 8 October 2017

public class HandleRFID : MonoBehaviour {

    public GameObject arduino;

    public GameObject player1Check;
    public GameObject player2Check;
    public GameObject player3Check;

    bool player1;
    bool player2;
    bool player3;

	// Use this for initialization
	void Start () {
        player1Check.SetActive(false);
        player2Check.SetActive(false);
        player3Check.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        player1 = arduino.GetComponent<Receive>().getRFID1();
        player2 = arduino.GetComponent<Receive>().getRFID2();
        player3 = arduino.GetComponent<Receive>().getRFID3();


        if (player1)
        {
            player1Check.SetActive(true);
        }
        if (player2)
        {
            player2Check.SetActive(true);
        }
        if (player3)
        {
            player3Check.SetActive(true);
        }

        if(player1 && player2 && player3)
        {
            //run experience


            //after experience is called
            player1 = false;
            player2 = false;
            player3 = false;
        }

	}
}
