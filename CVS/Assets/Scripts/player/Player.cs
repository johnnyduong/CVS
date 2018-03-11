﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {
    
    [SerializeField]
    private Text myCubitsNumberText;

    [SerializeField]
    private float tossRange = 5;

    [SerializeField]
    private int cubitsNum;

    public int CubitsNum
    {
        get { return cubitsNum; }
        set
        {
            cubitsNum = value;
            myCubitsNumberText.text = value+"";

        }
    }

    [SerializeField]
    private GameObject throwableCubePrefab;

    [SerializeField]
    private Transform spawnPoint;
	// Use this for initialization
	void Start () {
	}

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        //left click do something here, 
        //TODO will change when VR implementation
        if (Input.GetMouseButtonDown(0))
        {
            /*
            if (CubitsNum <= 0)
            {
                return;
            }
            CubitsNum--;
            */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ThrowCube(hit.point);
                
            }
        }
    
    }



    void ThrowCube(Vector3 location)
    {
        Vector3 location1 = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        GameObject cubeBall = Instantiate(throwableCubePrefab, spawnPoint.position, transform.rotation) as GameObject;

        Vector3 leveledLocation = new Vector3(location.x, transform.position.y, location.z);
        Vector3 targetVector = leveledLocation - transform.position;
        cubeBall.GetComponent<Rigidbody>().velocity = targetVector.normalized* tossRange; //cubeBall.transform.forward * 5;
        // Destroy the bullet after 2 seconds
       //Destroy(bullet, 2.0f);

    }


    //Touch another object with .tag name
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "pickable")
        {
            other.GetComponent<IPickUp>().PickMeUp();
            CubitsNum++;
        }
    }

}