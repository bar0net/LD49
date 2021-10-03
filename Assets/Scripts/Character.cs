using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public const float STUCK_TIME_LIMIT = 1.25f;

    public enum MoveType { None = 0, Stop, Walk, Run }

    public float jumpForce = 1.0f;
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;
    public float recoveryTime = 1.0f;

    [Space]
    public float upGravity = 0.33f;
    public float downGravity = 1.8f;

    [Space]
    public AudioClip jumpClip;
    public AudioClip death1;
    public AudioClip death2;


    // Components
    Rigidbody2D _rb;
    Animator _anim;
    Manager _m;
    AudioSource _as;

    bool jumping = false;
    float speed = 0;
    bool playing = false;

    float falling_timer = 0;

    Vector2 prev_velocity = Vector2.zero;
    Vector3 prev_position = Vector3.zero;

    float timer = 0;
    float time_not_moving = 0;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _m = FindObjectOfType<Manager>();
        _as = GetComponent<AudioSource>();

        _anim.speed = 0;
    }

    private void Update()
    {
        if (!playing) return;

        if (falling_timer > 0.07f) _anim.SetBool("falling", true);

        if (timer > 0 && Time.time - timer > recoveryTime)
        {
            timer = 0;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            SetSpeed(MoveType.Walk);
        }

        if (_rb.velocity.y > 0.05f)      _rb.gravityScale = upGravity;
        else if (_rb.velocity.y < 0.05f) _rb.gravityScale = downGravity;
        else _rb.gravityScale = 1.0f;

        float diff_mag = 2 * (transform.position - prev_position).magnitude;
        float expected_mag = speed * Time.deltaTime;

        if (diff_mag < expected_mag && Time.timeScale > 0)
        {
            time_not_moving += Time.deltaTime;
            if (time_not_moving > STUCK_TIME_LIMIT)
            {
                _as.clip = death1;
                _as.Play();
                _m.Death();
                _anim.speed = 0;
            }
        }
        else if (diff_mag >= expected_mag && Time.timeScale > 0)
        {
            time_not_moving = 0;
        }
        prev_position = transform.position;
    }

    void FixedUpdate()
    {
        if (!playing)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        _rb.velocity = new Vector2(speed, _rb.velocity.y);      

        if (jumping && _rb.velocity.y < 0)
        {
            jumping = false;
            _anim.SetBool("jumping", false);
        }

        if (_rb.velocity.y < -0.1f)
        {
            falling_timer += Time.fixedDeltaTime;
        }
        else
        {
            falling_timer = 0;
        }

    }

public void ResetCharacter()
    {
        time_not_moving = 0;
        timer = 0;
        playing = false;
        
        _anim.SetInteger("wrath", 0);
        _anim.SetInteger("happiness", 0);
        _anim.SetBool("jumping", false);
        _anim.SetBool("falling", false);
        _anim.SetBool("sleep", false);
        _anim.ResetTrigger("change");

        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        prev_velocity = Vector2.zero;
        prev_position = Vector3.zero;

        SetSpeed(MoveType.Walk);
        _anim.speed = 0;
    }

    public void SetSpeed(MoveType type)
    {
        switch (type)
        {
            case MoveType.Stop:
                speed = 0;
                timer = Time.time;
                _anim.SetInteger("wrath", -1);
                _anim.SetTrigger("change");

                if (_m.GetMood().y == 1) _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;

            case MoveType.Walk:
                speed = walkSpeed;
                _anim.SetInteger("wrath", 0);
                _anim.SetTrigger("change");
                break;

            case MoveType.Run:
                speed = runSpeed;
                _anim.SetInteger("wrath", 1);
                _anim.SetTrigger("change");
                break;
            default:
                break;
        }
    }

    public void Crouch()
    {
        _anim.SetInteger("happiness", -1);
        _anim.SetTrigger("change");
    }

    public void Stand()
    {
        _anim.SetInteger("happiness", 0);
        _anim.SetTrigger("change");
    }

    public void Jump()
    {
        jumping = true;
        _anim.SetBool("jumping", true);
        _anim.SetInteger("happiness", 1);
        _anim.SetTrigger("change");

        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(jumpForce * Vector2.up);

        _as.clip = jumpClip;
        _as.Play();
    }

    public void Play()
    {
        speed = walkSpeed;
        playing = true;

        _m.AllowEdit(false);
        _anim.speed = 1;
    }

    public void Pause()
    {
        prev_velocity = _rb.velocity;
        playing = false;
    }

    public void Resume()
    {
        _rb.velocity = prev_velocity;
        playing = true;
    }

    public void Sleep()
    {
        _anim.SetBool("sleep", true);
        _anim.SetTrigger("change");
        playing = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain" && !jumping)
        {
            _anim.SetBool("falling", false);
            falling_timer = 0;
        }
    }

    public bool isPlaying()
    {
        return playing;
    }

    public void SetPlaying(bool value) { playing = value; _anim.speed = 0; }

    public void PlayDeathSound()
    {
        _as.clip = death2;
        _as.Play();
    }
}
