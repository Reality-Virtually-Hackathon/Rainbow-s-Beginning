using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour {

    public Transform target;

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
