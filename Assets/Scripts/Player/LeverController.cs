using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public NewBehaviourScript platformController;

    void OnMouseDown()
    {
        platformController.ToggleMovement();
    }
}
