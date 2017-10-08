using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text.RegularExpressions;

public class Receive : MonoBehaviour {

    SerialPort stream = new SerialPort("COM7", 115200);
    private char[] delimeters = { ',', '\n' };
    public static string humidity;
    public static string temperature;
    public static int RFID;

    public static bool RFID1;
    public static bool RFID2;
    public static bool RFID3;

    public static bool allPlayers;

    private float timer;

    private string receivedData;

    // Use this for initialization
    void Start() {
        stream.ReadTimeout = 50;
        stream.Open();

        RFID1 = false;
        RFID2 = false;
        RFID3 = false;
        allPlayers = false;
        
    }

    // Update is called once per frame
    void Update() {
        receivedData = stream.ReadLine();
        if(receivedData != null)
        {
            Debug.Log("Data is not null right now");
            Debug.Log("receivedData is " + receivedData);
            parseData(receivedData);
        }
        else
        {
            Debug.Log("Data is null");
        }

        //if (allPlayers)
        //{

            //Todo: create experience
            //Debug.Log("CREATE EXPERIENCE");
        //}
    }

    void parseData(string receivedData)
    {
        string[] data = receivedData.Split(delimeters);
        if(data.Length == 2) //humidity, temperature
        {
            humidity = data[0];
            temperature = data[1];
            RFID = 0; 
        } else if (data.Length == 3) //humidity, temperature, RFID
        {
            humidity = data[0];
            temperature = data[1];
            RFID = int.Parse(data[2]);
            if (RFID == 1 && !RFID1)
            {
                RFID1 = true;
                checkAllPlayers();
                Debug.Log("RFID 1 REGISTERED");
            } else if (RFID == 2 && !RFID2)
            {
                RFID2 = true;
                checkAllPlayers();
                Debug.Log("RFID 2 REGISTERED");
            } else if (RFID == 3 && !RFID3)
            {
                RFID3 = true;
                checkAllPlayers();
                Debug.Log("RFID 3 REGISTERED");
            } else
            {
                Debug.Log("error: RFID device not supported");
            }

            Debug.Log("Humidty is: " + humidity);
            Debug.Log("Temp is: " + temperature);
            Debug.Log("RFID 1: " + RFID1);
            Debug.Log("RFID 2: " + RFID2);
            Debug.Log("RFID 3: " + RFID3);
        }
        else
        {
            Debug.Log("error parsing Arduino data stream");
            Debug.Log(data.Length);
        }
    }

    void checkAllPlayers()
    {
        if (RFID1 && RFID1 && RFID3)
        {
            allPlayers = true;
        }
    }

    public void clearAllPlayers()
    {
        RFID1 = false;
        RFID2 = false;
        RFID3 = false;
        allPlayers = false;
    }
}
