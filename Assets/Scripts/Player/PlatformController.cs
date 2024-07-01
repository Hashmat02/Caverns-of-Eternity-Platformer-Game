//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewBehaviourScript : MonoBehaviour
//{
//    public Transform posA, posB;
//    public int Speed;
//    Vector2 targetPos;

//    void Start()
//    {
//        targetPos = posB.position; 
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Vector2.Distance(transform.position, posA.position) < .1f) targetPos = posB.position;
//        if (Vector2.Distance(transform.position, posB.position) < .1f) targetPos = posA.position;

//        transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);

//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            collision.transform.SetParent(this.transform);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            collision.transform.SetParent(null);
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform posA, posB;
    public int Speed;
    public bool isConstantlyMoving = true;
    private bool isMoving = false; 
    Vector2 targetPos;

    void Start()
    {
        targetPos = posB.position;
        isMoving = isConstantlyMoving; 
    }

    void Update()
    {
        if (isMoving)
        {
            if (Vector2.Distance(transform.position, posA.position) < .1f) targetPos = posB.position;
            if (Vector2.Distance(transform.position, posB.position) < .1f) targetPos = posA.position;

            transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
        }
    }

    public void ToggleMovement()
    {
        if (!isConstantlyMoving)
        {
            isMoving = !isMoving;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
