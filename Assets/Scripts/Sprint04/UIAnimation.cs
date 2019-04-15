using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{

    private bool isHidden = false;
    private IEnumerator coroutine;
    public float speed = 2f;
    private Vector3 showPos, hidePos;

    private void Start()
    {
        showPos = Vector3.zero;
        hidePos = new Vector3(-6f, 0f, 0f);

        if(!Application.isEditor)
        {
            transform.position = hidePos;
            isHidden = true;
            MoveUI();
        }
        
    }

    public void MoveUI()
    {
        if(isHidden)
        {
            coroutine = MovingUI(showPos);
            isHidden = false;
        }
        else
        {
            coroutine = MovingUI(hidePos);
            isHidden = true;
        }
        StartCoroutine(coroutine);
    }

    private IEnumerator MovingUI(Vector3 endPos)
    {
        float elapsedTime = 0f;

        while(Vector3.Distance(transform.position, endPos) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.position, endPos, elapsedTime/speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
