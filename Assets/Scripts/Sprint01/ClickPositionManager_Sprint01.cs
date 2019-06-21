using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickPositionManager_Sprint01 : MonoBehaviour
{

    private int shape = 0;
    private GameObject primitive;
    private float red = 1f, green = 1f, blue = 1f;
    public Text mousePosition;

    [SerializeField]
    private float distance = 5f, distanceChange;

    private Vector3 clickPosition;
    private bool timedDestroyIsOn = true;
    private float size = 0.5f;
	
	void Update ()
    {

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1)) //left click or hold
        {
            //checking for an colliders out in the virtual world that my mousePosition 
            //is over when the user left clicks or holds
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; //dont have to assign this as the raycast will assign this procedurally

            if (Physics.Raycast(ray, out hit)) //export out the information to hit
            {
                Destroy(hit.transform.gameObject);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) //right click or hold
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));

            switch (shape)
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
            primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f)*size, Random.Range(0.1f, 1f)*size, Random.Range(0.1f, 1f)*size);
            //randomizing colors and scale
            primitive.transform.position = clickPosition;
            primitive.GetComponent<Renderer>().material.color = new Vector4(Random.Range(0f, red), Random.Range(0f, green), Random.Range(0f, blue), 1f);
            primitive.transform.parent = this.transform;
            if(timedDestroyIsOn)
            {
                Destroy(primitive, 3f);
            }
            
        }
        mousePosition.text = "Mouse Position x: " + Input.mousePosition.x.ToString("F0") + ", y: " + Input.mousePosition.y.ToString("F0");
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

    public void destroyObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ToggleTimedDestroy(bool timer)
    {
            timedDestroyIsOn = timer;
    }

    public void ChangeSize(float temp)
    {
        size = temp;
    }
}
