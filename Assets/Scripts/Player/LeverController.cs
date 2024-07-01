using UnityEngine;

public class LeverController : MonoBehaviour
{
    public NewBehaviourScript platformController;

    void OnMouseDown()
    {
        platformController.ToggleMovement();
    }
}
