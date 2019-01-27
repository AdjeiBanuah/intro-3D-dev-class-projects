using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPositionManager_01ScreenToWorldPoint: MonoBehaviour {

	void Update ()
    {
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
        {
            //Goal: log click position in world space to the console

            //create a vector to store the mouse position
            //Set it to a default value whose value will never be recorded from the mouse position
            //so when start clicking will get results of (-1,-1,-1) that we know if not real information
            Vector3 clickPosition = -Vector3.one;

            //method 1: ScreenToWorldPoint
            //going to use screenToWorldPoint, a built-in unity method that derives from cameras
            //vector to pass to method contains two main pieces of information
            //x, and y coordinates are the pixel locations on the screen where the mouse is
            //Can get that from Input.mousePosition but z is zero from that
            //We need a z location of how far into the virtual world that we want to determine our click was at
            //for now, we arbitary pick z = 5
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, 5f));

            //print out the position and spawn a sphere
            Debug.Log(clickPosition);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = clickPosition;
        }
	}
}
