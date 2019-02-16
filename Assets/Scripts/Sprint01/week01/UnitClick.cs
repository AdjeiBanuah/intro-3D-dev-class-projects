using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log(this.gameObject.name + " was clicked, only world position returned is " + this.gameObject.transform.position);
    }
}
