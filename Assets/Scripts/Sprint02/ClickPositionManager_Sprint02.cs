using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickPositionManager_Sprint02 : MonoBehaviour {

    public LayerMask clickMask, UIMask;
    private Vector3 lastClickPosition = Vector3.zero;
    private int shape = 0;
    private GameObject primitive;
    private float red = 1f, green = 1f, blue = 1f, destroyTime = 3f;
    public Text lifeTime;
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
        {
            //Goal: log click position in world space to the console

            //create a vector to store the mouse position
            //Set it to a default value whose value will never be recorded from the mouse position
            //so when start clicking will get results of (-1,-1,-1) that we know if not real information
            Vector3 clickPosition = -Vector3.one;

            /*
            //method 1: ScreenToWorldPoint
            //going to use screenToWorldPoint, a built-in unity method that derives from cameras
            //vector to pass to method contains two main pieces of information
            //x, and y coordinates are the pixel locations on the screen where the mouse is
            //Can get that from Input.mousePosition but z is zero from that
            //We need a z location of how far into the virtual world that we want to determine our click was at
            //for now, we arbitary pick z = 5
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, 5f));
            */

            /*
            //method 2: Raycast using Colliders
            //Casts a ray from the position of the camera out through the mouse out to infinity
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //dont need a z distance
            //create a raycasthit but its automatically populated from where the ray will hit a collider
            RaycastHit hit; //dont have to assign this as the raycast will assign this procedurally

            //if(Physics.Raycast(ray, out hit)) //export out the information to hit
            if (Physics.Raycast(ray, out hit, 100f, clickMask)) //need to add max distance, and layerMask
            {
                //hit has alot of information about the raycast collision on both sides
                clickPosition = hit.point;

                //but it hits everything which is a problem as the spheres populate more
                //so add physics layers and update the Raycast method call
            }
            */

            
            //method 3: Raycast using Plane
            Plane plane = new Plane(Vector3.forward, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distanceToPlane;

            if(plane.Raycast(ray, out distanceToPlane))
            {
                clickPosition = ray.GetPoint(distanceToPlane);
            }

            //print out the position and spawn a sphere

            //Debug.Log(clickPosition);
            switch(shape)
            {
                case 0:
                    primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;

                case 1:
                    primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;

                case 2:
                    primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
            }
            primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
            /*
            if (lastClickPosition == Vector3.zero) sphere.transform.localScale = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
            else
            {
                float x = Mathf.Clamp(Random.Range(0.5f, 3f) * (Mathf.Abs(lastClickPosition.x - clickPosition.x)), .2f, 10f);
                float y = Mathf.Clamp(Random.Range(0.5f, 3f) * (Mathf.Abs(lastClickPosition.y - clickPosition.y)), .2f, 10f);
                float z = (x + y) / 2f;
                sphere.transform.localScale = new Vector3(x, y, z);
            }
            */
            //randomizing colors and scale
            primitive.transform.position = clickPosition;
            primitive.GetComponent<Renderer>().material.color = new Vector4(Random.Range(0f, red), Random.Range(0f, green), Random.Range(0f, blue), 1f);
            Destroy(primitive, destroyTime);
            lastClickPosition = clickPosition;
        }
	}

    public void changeShape(int tempShape)
    {
        shape = tempShape;
    }

    public void changeRed(float tempRed)
    {
        red = tempRed;
    }

    public void changeGreen(float tempGreen)
    {
        green = tempGreen;
    }

    public void changeBlue(float tempBlue)
    {
        blue = tempBlue;
    }

    public void changeDestructionTimer(float tempDestroyTime)
    {
        destroyTime = (tempDestroyTime * 5f) + 1f;
        lifeTime.text = "Lifetime: " + Mathf.RoundToInt(destroyTime).ToString();
    }
}
