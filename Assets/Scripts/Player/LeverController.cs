//using UnityEngine;

//public class LeverController : MonoBehaviour
//{
//    public NewBehaviourScript platformController;

//    void OnMouseDown()
//    {
//        platformController.ToggleMovement();
//    }
//}


using UnityEngine;

public class LeverController : MonoBehaviour
{
    public NewBehaviourScript platformController; 
    public GameObject player; 
    public float interactionDistance = 2f; 

    void OnMouseDown()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= interactionDistance)
        {
            platformController.ToggleMovement();
        }
        else
        {
            Debug.Log("Player is too far to interact with the lever.");
        }
    }
}
