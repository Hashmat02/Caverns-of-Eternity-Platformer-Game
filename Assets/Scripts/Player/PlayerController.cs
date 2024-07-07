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
	private float _defMaxVel;

    [Header("Jump Properties")]
    [SerializeField, Range(0.0f, 10.0f)] private float _jumpHeight = 3.0f;
    [SerializeField, Range(0, 5)] private int _maxJumps = 1;
    [SerializeField, Range(0.0f, 5.0f)] private float _fallMult = 3.0f;
    [SerializeField, Range(0.0f, 5.0f)] private float _riseMult = 1.5f;
    [SerializeField, Range(0.0f, 0.5f)] private float _coyoteTime = 0.2f;

    private bool _wantJump = false;
    private float _coyoteCounter;
    private int _jumpCount = 0;
	public bool forceJump = false;

    [Header("Wall Interaction Properties")]
    [SerializeField, Range(0.0f, 5.0f)] private float _wallSlideMaxVel = 2.0f;
    [SerializeField] private Vector2 _wallJumpClimb = new Vector2(7.0f, 12.0f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(10.7f, 10.0f);
    [SerializeField, Range(0.0f, 1.0f)] private float _wallStickTime = 0.5f;

    private bool _isWallJumping = false;
    private float _wallStickCounter;

    private delegate void DesiredJump();
    private DesiredJump _desiredJump;

	public delegate void PlayJump();
	public PlayJump playJump;

	public delegate void JumpLanded();
	public JumpLanded landed;

    private Rigidbody2D _body;
    private PlatformCollision _collisionCheck;

	private Animator _anim;
	private SpriteRenderer _sprite;

	public void multMaxVel(float multiplier) {
		_maxVel *= multiplier;
	}

	public void addMaxJumps(int added) {
		if (_maxJumps + added > 5) {
			return;
		}
		_maxJumps += added;
	}

	public void revertMaxVel() {
		_maxVel = _defMaxVel;
	}

    void Awake() {
        _body = GetComponent<Rigidbody2D>();
        if (!_body) {
            ErrorHandling.throwError("No Rigidbody2D component found.");
        }
        _collisionCheck = GetComponent<PlatformCollision>();
        if (!_collisionCheck) {
            ErrorHandling.throwError("No PlatformCollision script found.");
        }
		_defMaxVel = _maxVel;
		_anim = GetComponent<Animator>();
		if (!_anim) {
			ErrorHandling.throwError("No Animator found.");
		}
		_sprite = GetComponent<SpriteRenderer>();
		if (!_sprite) {
			ErrorHandling.throwError("No Sprite Renderer found.");
		}
    }

    void Start() {
        _coyoteCounter = _coyoteTime;
        _wallStickCounter = _wallStickTime;
    }

    void Update() {
        _moveX = Input.GetAxisRaw("Horizontal");
		if (_moveX != 0) {
			_anim.SetBool("isWalking", true);
		} else {
			_anim.SetBool("isWalking", false);
		}
		if (_moveX > 0) {
			_sprite.flipX = true;
		} else if (_moveX < 0) {
			_sprite.flipX = false;
		}
        _projectedVelX = _moveX * Mathf.Max(_maxVel - _collisionCheck.friction, 0.0f);
		_desiredJump = _collisionCheck.isGround ? jump : _collisionCheck.isWall ? wallJump : jump;
        _wantJump |= Input.GetButtonDown("Jump");
    }

    void FixedUpdate() {
        _velocity = _body.velocity;
        playerMoveKeyboard();

        #region Coyote Counter
        if (_collisionCheck.isGround && _body.velocity.y == 0) {
            _coyoteCounter = _coyoteTime;
        }
        else {
            _coyoteCounter -= Time.deltaTime;
        }
        #endregion

        #region Wall Stick Counter
        if ( _wallStickCounter > 0.0f && _collisionCheck.isWall && !_collisionCheck.isGround && !_isWallJumping) {
            wallStick();
        }
        else {
            _wallStickCounter = _wallStickTime;
        }
        #endregion

        #region Jump
        if (_wantJump) {
			if (_desiredJump != null) {
				playJump?.Invoke();
			}
            _desiredJump?.Invoke();
            _wantJump = false;
        }
        #endregion

        #region Wall Slide
        if (_collisionCheck.isWall && _velocity.y < -_wallSlideMaxVel) {
            _velocity.y = -_wallSlideMaxVel;
        }
        #endregion

		if (forceJump) {
			forceJumpOnHit();
		}

        // adjust gravity scale
        _body.gravityScale = _body.velocity.y == 0 ? Constants.DEF_GRAVITY_SCALE
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
			return;
        }
		_wallStickCounter = _wallStickTime;
    }

    void jump() {
        if (_coyoteCounter <= 0.0f && _jumpCount >= _maxJumps) {
            _wantJump = false;
            return;
        }
        _jumpCount++;
        _coyoteCounter = 0.0f;
        float jumpVel = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
        _velocity = new Vector2(_velocity.x, jumpVel);
		if (!_collisionCheck.isGround && _maxJumps > 1) {
			_maxJumps--;
		}
    }

	void forceJumpOnHit() {
		float jumpVel = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
		_velocity = new Vector2(-_velocity.x, jumpVel);
		forceJump = false;
	}

    void wallJump() {
        if (-_collisionCheck.contactNormal.x == _moveX) {
            _velocity = new Vector2(_wallJumpClimb.x * _collisionCheck.contactNormal.x, _wallJumpClimb.y);
        }
        else if (_collisionCheck.contactNormal.x == _moveX) {
            _velocity = new Vector2(_wallJumpLeap.x * _collisionCheck.contactNormal.x, _wallJumpLeap.y);
        }
        _isWallJumping = true;
    }

    void OnCollisionEnter2D() {
        if (_collisionCheck.isWall && !_collisionCheck.isGround && _isWallJumping) {
            _body.velocity = Vector2.zero;
        }
        if (_collisionCheck.isGround) {
			landed?.Invoke();
            _jumpCount = 0;
        }
        _isWallJumping = false;
    }
}
