using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickPositionManager_Sprint02 : MonoBehaviour
{

    private int shape = 0;
    private GameObject primitive;
    private float red = .8f, green = .8f, blue = .8f, destroyTime = 3f, timeToDestroy = 3f, Xcutoff;
    public Text mousePosition, blueAmount, redAmount, greenAmount, sizeAmount, timerAmount, animAmount;

    [SerializeField]
    private float distance = 5f, distanceChange;

    private Vector3 clickPosition;
    private bool timedDestroyIsOn = true, isAnimTypeRandom, isAnimSpeedRandom;
    private float size = 1.0f;

    private Vector3 lastClickPosition = Vector3.zero;
    public Text lifeTime;

    public GameObject paintedObject00, paintedObject01, paintedObject02, explosion;
    private Color paintedObjectColor, paintedObjectEmission;

    [SerializeField]
    [Range(0.0f, 2f)]
    private float emissionStrength = 0.5f;

    private float opacityStrength = 0.5f;

    private int animationState = 0;
    private float animationSpeed = 1f;

    public Dropdown animDropDown, shapeDropDown;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) animDropDown.value = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) animDropDown.value = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) animDropDown.value = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSpawnObject(0);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSpawnObject(1);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ChangeSpawnObject(2);

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
                    //Debug.Log(hit.transform.rotation.ToString());
                }
            }
        }

        //NEW
        //Debug.Log(EventSystem.current.currentSelectedGameObject.ToString());
        //Debug.Log((EventSystem.current.currentSelectedGameObject == null));

        //NEW if check
        if (Input.GetMouseButton(1) && (EventSystem.current.currentSelectedGameObject == null)) //right hold
        {
            //distance += distanceChange;

            //checking for an colliders out in the virtual world that my mousePosition 
            //is over when the user left clicks or holds
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; //dont have to assign this as the raycast will assign this procedurally

            if (Physics.Raycast(ray, out hit)) //export out the information to hit
            {
                if (hit.transform.gameObject.layer == 12) //PaintedObject
                {
                    Destroy(hit.transform.gameObject);
                    primitive = Instantiate(explosion, hit.transform.position, Quaternion.identity);
                    Destroy(primitive, 1f);
                }
                //this destroys the paint object, but we can modify it, scale it, recolor it, etc
            }
        }

        if (Input.GetMouseButtonUp(0)) //user released left mouse button so reset lastPosition to avoid large spawns at next paint drop
        {
            lastClickPosition = Vector3.zero;
        }

        //check if UI is show or hidden
        //if UI is show Xcutoff = 200f
        //else UI is hidden Xcutoff = 0f

        //NEW Got rid of mousePosition check since moved paint stroke behind UI
        if (Input.GetMouseButton(0) && (EventSystem.current.currentSelectedGameObject == null) && Input.mousePosition.x > Xcutoff) //left hold
        {
            
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));

            switch (shape)
            {
                case 0:
                    primitive = Instantiate(paintedObject00, clickPosition, Quaternion.identity);
                    break;

                case 1:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    primitive = Instantiate(paintedObject01, clickPosition, Quaternion.identity);
                    break;

                case 2:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    primitive = Instantiate(paintedObject02, clickPosition, Quaternion.identity);
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
                float x = Mathf.Clamp(Random.Range(size, size * 6f) * Mathf.Abs(lastClickPosition.x - clickPosition.x), .1f, size * 10f);
                float y = Mathf.Clamp(Random.Range(size, size * 6f) * Mathf.Abs(lastClickPosition.y - clickPosition.y), .1f, size * 10f);
                float z = (x + y) / 2f;
                primitive.transform.localScale = new Vector3(x, y, z);
            }

            //randomizing colors and scale
            if (primitive.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStrength);
                primitive.GetComponent<Renderer>().material.color = paintedObjectColor;
                paintedObjectEmission = new Color(paintedObjectColor.r * emissionStrength, paintedObjectColor.g * emissionStrength, paintedObjectColor.b * emissionStrength);
                primitive.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
            }
            else { }
            
            foreach (Transform child in primitive.transform)
            {
                if (child.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStrength);
                    child.gameObject.GetComponent<Renderer>().material.color = paintedObjectColor;
                    primitive.gameObject.GetComponent<PrefabData>().initialColorInfo.Add(paintedObjectColor); //NEW
                    paintedObjectEmission = new Color(paintedObjectColor.r * emissionStrength, paintedObjectColor.g * emissionStrength, paintedObjectColor.b * emissionStrength);
                    child.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
                }   
            }

            //New
            if (primitive.GetComponent<Animator>() != null)
            {
                if (isAnimTypeRandom)
                {
                    animationState = (int)Random.Range(0f, 2.99f);
                    animDropDown.value = animationState;
                }
                    primitive.GetComponent<Animator>().SetInteger("state", animationState);

                if (isAnimSpeedRandom) animationSpeed = Random.Range(0f, 1f);
                primitive.GetComponent<Animator>().speed = animationSpeed;
                //primitive.GetComponent<Animator>().SetFloat("speed", animationSpeed);
                primitive.GetComponent<PrefabData>().initialAnimSpeed = animationSpeed;
            }

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

    //New
    public void ChangeAnimationTypeRandom(bool temp)
    {
        isAnimTypeRandom = temp;
    }

    //New
    public void ChangeAnimationSpeedRandom(bool temp)
    {
        isAnimSpeedRandom = temp;
    }

    public void ChangeAnimationState(int temp)
    {
        animationState = temp; // current/future painted objects
        //animDropDown.value = animationState; had to remove as the UI dropdown element ends up calling this method twice

        if(!isAnimTypeRandom)
        {
            foreach (Transform child in transform) //past painted objects
            {
                if (child.gameObject.GetComponent<Animator>() != null)
                {
                    child.gameObject.GetComponent<Animator>().SetInteger("state", animationState);
                    child.gameObject.GetComponent<Animator>().speed = primitive.GetComponent<PrefabData>().initialAnimSpeed;//primitive.GetComponent<Animator>().GetFloat("speed");
                }
            }
        }
    }

    public void ChangeSpawnObject(int temp)
    {
        shape = temp; // current/future painted objects
        shapeDropDown.value = temp;
    }

    //cover next week
    public void ChangeAnimationSpeed(float temp)
    {
        animationSpeed = temp;
        animAmount.text = animationSpeed.ToString("F1") + "X";

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Animator>() != null)
            {
                child.gameObject.GetComponent<Animator>().speed = primitive.GetComponent<PrefabData>().initialAnimSpeed * temp;//primitive.GetComponent<Animator>().GetFloat("speed") * temp;
            }
        }

    }

    //cover next week
    public void ChangeRed(float temp)
    {
        foreach (Transform child in transform) //prefab's parented under click manager object
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.r = temp;
                //paintedObjectColor = new Color(temp, paintedObjectColor.g, paintedObjectColor.b, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            int childCount = 0;
            foreach (Transform grandchild in child.transform) //prefab's children
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.r = child.GetComponent<PrefabData>().initialColorInfo[childCount].r * temp;
                    //paintedObjectColor = new Color(temp, paintedObjectColor.g, paintedObjectColor.b, paintedObjectColor.a);
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }
                childCount++;
            }
        }

        red = temp;
        redAmount.text = (red * 100f).ToString("F0");
    }

    public void ChangeGreen(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor = new Color(paintedObjectColor.r, temp, paintedObjectColor.b, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            int childCount = 0;
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.g = child.GetComponent<PrefabData>().initialColorInfo[childCount].g * temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }
                childCount++;
            }
            
        }

        green = temp;
        greenAmount.text = (green * 100f).ToString("F0");
    }

    public void ChangeBlue(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor = new Color(paintedObjectColor.r, paintedObjectColor.g, temp, paintedObjectColor.a);
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            int childCount = 0;
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.b = child.GetComponent<PrefabData>().initialColorInfo[childCount].b * temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }
                childCount++;
            }
        }

        blue = temp;
        blueAmount.text = (blue * 100f).ToString("F0");
    }

    //cover next week
    public void ChangeEmissionStrength(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectEmission = new Color(paintedObjectColor.r * temp, paintedObjectColor.g * temp, paintedObjectColor.b * temp);
                child.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectEmission = new Color(paintedObjectColor.r * temp, paintedObjectColor.g * temp, paintedObjectColor.b * temp);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
                }
            }

        }
        emissionStrength = temp;
    }

    //cover next week
    public void ChangeOpacityStrength(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.a = temp;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
            }

            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.a = temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }
            }
        }
        opacityStrength = temp;
    }

    public void DestroyObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            primitive = Instantiate(explosion, child.position, Quaternion.identity);
            Destroy(primitive, 1f);
        }
    }

    public void ToggleTimedDestroy(bool timer)
    {
        timedDestroyIsOn = timer;
    }

    public void ChangeSize(float temp)
    {
        foreach (Transform child in transform)
        {
            child.localScale = child.localScale * temp / size;
        }
        size = temp;
    }

    public void ChangeTimeToDestroy(float temp)
    {
        timeToDestroy = temp;
        timerAmount.text = timeToDestroy.ToString("F0") + " Sec";
    }
}
