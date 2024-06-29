using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour {
    public bool isGround { get; private set; }
    public bool isWall { get; private set; }
    public float friction { get; private set; }
    public Vector2 contactNormal { get; private set; }
    [SerializeField, Range(0, 90)] private float _maxAngleWall = 0; // in degrees
    [SerializeField, Range(0, 90)] private float _maxAngleGround = 0; // in degrees
    private PhysicsMaterial2D _material;
    private float _minWallNormalX;
	private float _minGroundNormalY;

    void Start() {
        _minWallNormalX = Mathf.Cos(_maxAngleWall * Mathf.PI / 180.0f);
        _minGroundNormalY = Mathf.Cos(_maxAngleGround * Mathf.PI / 180.0f);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        updateContacts(collision);
        getFriction(collision);
    }

    void OnCollisionStay2D(Collision2D collision) {
        updateContacts(collision);
        getFriction(collision);
    }

    void OnCollisionExit2D(Collision2D collision) {
        isGround = false;
        isWall = false;
        friction = 0;
    }

    void updateContacts(Collision2D collision) {
        for (int i = 0; i < collision.contactCount; i++) {
            contactNormal = collision.GetContact(i).normal;
            isWall |= Mathf.Abs(contactNormal.x) >= _minWallNormalX;
			isGround |= Mathf.Abs(contactNormal.y) >= _minGroundNormalY;
        }
    }

    void getFriction(Collision2D collision) {
        _material = collision.rigidbody?.sharedMaterial;
		friction = _material ? _material.friction : 0;
    }
}
