using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickPositionManager_Sprint02 : MonoBehaviour
{

    private int shape = 0;
    private GameObject primitive;
    private float red = .8f, green = .8f, blue = .8f, destroyTime = 3f, timeToDestroy = 3f;
    public Text mousePosition, blueAmount, redAmount, greenAmount, sizeAmount, timerAmount;

    [SerializeField]
    private float distance = 5f, distanceChange;

    private Vector3 clickPosition;
    private bool timedDestroyIsOn = true;
    private float size = 0.5f;

    private Vector3 lastClickPosition = Vector3.zero;
    public Text lifeTime;

    public GameObject paintedObject01;
    private Color paintedObjectColor, paintedObjectEmission;

    public Clock clock;

    void Update ()
    {

        if (Input.GetMouseButtonDown(0)) //left click
        {
            //checking for an colliders out in the virtual world that my mousePosition 
            //is over when the user left clicks or holds
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; //dont have to assign this as the raycast will assign this procedurally

            if (Physics.Raycast(ray, out hit)) //export out the information to hit
            {
                if(hit.transform.gameObject.layer == 11) //Clockface
                {
                    hit.transform.parent.GetComponent<Clock>().UpdateTime(hit.transform.localEulerAngles.y);
                    Debug.Log(hit.transform.rotation.ToString());
                }
            }
        }

        if (Input.GetMouseButton(0)) //left hold
        {
            //checking for an colliders out in the virtual world that my mousePosition 
            //is over when the user left clicks or holds
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; //dont have to assign this as the raycast will assign this procedurally

            if (Physics.Raycast(ray, out hit)) //export out the information to hit
            {
                if (hit.transform.gameObject.layer == 12) //PaintedObject
                {
                    Destroy(hit.transform.gameObject);
                }
                //this destroys the paint object, but we can modify it, scale it, recolor it, etc
            }
        }

        if (Input.GetMouseButtonUp(1)) //user released right mouse button so reset lastPosition to avoid large spawns at next paint drop
        {
            lastClickPosition = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1)) //right click or hold
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));

            switch (shape)
            {
                case 0:
                    primitive = Instantiate(paintedObject01, clickPosition, Quaternion.identity);
                    break;

                case 1:
                    primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;

                case 2:
                    primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;

                default:
                    //if no cases are true, do this by default
                    break;
            }
            //for the first paint drop as it doesnt have reference
            //primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f)* size, Random.Range(0.1f, 1f)*size, Random.Range(0.1f, 1f)*size);

            if (lastClickPosition == Vector3.zero) primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f)*size, Random.Range(0.1f, 1f)*size, Random.Range(0.1f, 1f)*size);
            else
            {
                //float x = Mathf.Clamp(Random.Range(0.5f, 3f) * Mathf.Abs(lastClickPosition.x - clickPosition.x), .1f, 5f);
                float x = Mathf.Clamp(Random.Range(0.5f, 3f) * Mathf.Abs(lastClickPosition.x - clickPosition.x), .1f, 5f);
                float y = Mathf.Clamp(Random.Range(0.5f, 3f) * (Mathf.Abs(lastClickPosition.y - clickPosition.y)), .2f, 5f);
                float z = (x + y) / 2f;
                primitive.transform.localScale = new Vector3(x, y, z);
            }
            //randomizing colors and scale
            //primitive.transform.position = clickPosition;
            paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), 0.5f);
            primitive.GetComponent<Renderer>().material.color = paintedObjectColor;
            paintedObjectEmission = new Color(paintedObjectColor.r / 2f, paintedObjectColor.g / 2f, paintedObjectColor.b / 2f);
            primitive.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);

            primitive.transform.parent = this.transform;
            if(timedDestroyIsOn)
            {
                Destroy(primitive, timeToDestroy);
            }
            lastClickPosition = clickPosition;

        }
        mousePosition.text = "Mouse Position x: " + Input.mousePosition.x.ToString("F0") + ", y: " + Input.mousePosition.y.ToString("F0");
    }

    public void ChangeShape(int tempShape)
    {
        shape = tempShape;
        
    }

    public void ChangeRed(float tempRed)
    {
        red = tempRed;
        redAmount.text = (red * 100f).ToString("F0");
    }

    public void ChangeGreen(float tempGreen)
    {
        green = tempGreen;
        greenAmount.text = (green * 100f).ToString("F0");
    }

    public void ChangeBlue(float tempBlue)
    {
        blue = tempBlue;
        blueAmount.text = (blue * 100f).ToString("F0");
    }

    public void DestroyObjects()
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

    public void ChangeTimeToDestroy(float temp)
    {
        timeToDestroy = temp;
        timerAmount.text = timeToDestroy.ToString("F0") + " Sec";
    }
}
