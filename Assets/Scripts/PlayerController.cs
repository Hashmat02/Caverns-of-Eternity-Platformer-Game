using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement Properties")]
    [SerializeField, Range(0.0f, 50.0f)] private float _maxVel = 4.0f;
    [SerializeField, Range(0.0f, 50.0f)] private float _maxAccel = 40.0f;
    [SerializeField, Range(0.0f, 50.0f)] private float _maxAirAccel = 20.0f;

    private Vector2 _velocity;
    private float _moveX;
    private float _projectedVelX;

    [Header("Jump Properties")]
    [SerializeField, Range(0.0f, 10.0f)] private float _jumpHeight = 3.0f;
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 1;
    [SerializeField, Range(0.0f, 5.0f)] private float _fallMult = 3.0f;
    [SerializeField, Range(0.0f, 5.0f)] private float _riseMult = 1.5f;
    [SerializeField, Range(0.0f, 0.5f)] private float _coyoteTime = 0.2f;

    private bool _wantJump = false;
    private float _coyoteCounter;
    private int _jumpCount = 0;

    [Header("Wall Interaction Properties")]
    [SerializeField, Range(0.0f, 5.0f)] private float _wallSlideMaxVel = 2.0f;
    [SerializeField] private Vector2 _wallJumpClimb = new Vector2(7.0f, 12.0f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(10.7f, 10.0f);
    [SerializeField, Range(0.0f, 1.0f)] private float _wallStickTime = 0.5f;

    private bool _wantWallJump = false;
    private bool _isWallJumping = false;
    private float _wallStickCounter;

    private Rigidbody2D _body;
    private CollisionCheck _collisionCheck;

    private const float DEF_GRAVITY_SCALE = 1.0f;

    void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionCheck>();
    }

    void Start() {
        _coyoteCounter = _coyoteTime;
        _wallStickCounter = _wallStickTime;
    }

    void Update() {
        _moveX = Input.GetAxisRaw("Horizontal");
        _projectedVelX = _moveX * Mathf.Max(_maxVel - _collisionCheck.friction, 0.0f);
        _wantJump |= Input.GetButtonDown("Jump") && _collisionCheck.isGround;
        _wantWallJump |= Input.GetButtonDown("Jump") && (_collisionCheck.isWall && !_collisionCheck.isGround);
    }

    void FixedUpdate() {
        _velocity = _body.velocity;
        playerMoveKeyboard();

#region Coyote Counter
        if (_collisionCheck.isGround && _body.velocity.y == 0) {
            _coyoteCounter = _coyoteTime;
        } else {
            _coyoteCounter -= Time.deltaTime;
        }
#endregion

#region Wall Stick Counter
        if (_wallStickCounter > 0.0f && _collisionCheck.isWall && !_collisionCheck.isGround && !_isWallJumping) {
            wallStick();
        } else {
            _wallStickCounter = _wallStickTime;
        }
#endregion

#region Jump
        if (_wantJump) {
            jump();
            _wantWallJump = false;
        }
#endregion

#region Wall Jump
        if (_wantWallJump) {
            wallJump();
            _wantJump = false;
        }
#endregion

#region Wall Slide
        if (_collisionCheck.isWall && _velocity.y < -_wallSlideMaxVel) {
            _velocity.y = -_wallSlideMaxVel;
        }
#endregion

        // adjust gravity scale
        _body.gravityScale = _body.velocity.y == 0 ? DEF_GRAVITY_SCALE 
            : _body.velocity.y > 0 ? _riseMult
            : _fallMult;

        _body.velocity = _velocity;
    }

    void playerMoveKeyboard() {
        float acceleration = (_collisionCheck.isGround ? _maxAccel : _maxAirAccel) * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _projectedVelX, acceleration);
    }

    void wallStick() {
        _velocity.x = 0.0f;
        if (_moveX == _collisionCheck.contactNormal.x) {
            _wallStickCounter -= Time.deltaTime;
        } else {
            _wallStickCounter = _wallStickTime;
        }
    }

    void jump() {
        if (_coyoteCounter <= 0.0f && _jumpCount > _maxAirJumps) {
            _wantJump = false;
            return;
        }
        _jumpCount++;
        _coyoteCounter = 0.0f;
        float jumpVel = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
        _velocity = new Vector2(_velocity.x, jumpVel);
        _wantJump = false;
    }

    void wallJump() {
        if (-_collisionCheck.contactNormal.x == _moveX) {
            _velocity = new Vector2(_wallJumpClimb.x * _collisionCheck.contactNormal.x, _wallJumpClimb.y);
        } else if (_collisionCheck.contactNormal.x == _moveX) {
            _velocity = new Vector2(_wallJumpLeap.x * _collisionCheck.contactNormal.x, _wallJumpLeap.y);
        }
        _wantWallJump = false;
        _isWallJumping = true;
    }

    void OnCollisionEnter2D() {
        if (_collisionCheck.isWall && !_collisionCheck.isGround && _isWallJumping) {
            _body.velocity = Vector2.zero;
        }
        if (_collisionCheck.isGround) {
            _jumpCount = 0;
        }
        _isWallJumping = false;
    }
}
