using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Control params")]
    public float speed = 4f;
    public float jumpForce = 50f;
    [Space]
    public float wallCheckDistance = .38f;
    public float groundCheckDistance = .1f;

    [Header("References")]
    public Transform _GroundCheck;
    public Transform _WallCheck;
    

    private Rigidbody rb;
    private Vector3 input;
    private static Animator anim;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool wallCollision = false;

    private bool canMove = true;
    private bool isWallSliding = false;

    public bool CanMove {
        get { return canMove; }
        set { canMove = value; }
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        IsGrounded();

        input = new Vector3(Input.GetAxis("Horizontal"),
                               0,
                               Input.GetAxis("Vertical"));
        anim.SetFloat("Speed", (new Vector2(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.z)).magnitude));
        //Debug.Log($"Speed: {(Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) / 2}");
        if (input.x > 0 && !facingRight) {
            Flip();
        } else if (input.x < 0 && facingRight) {
            Flip();
        }

        //Debug.Log($"isGrounded = {IsGrounded()}");

    }

    private void FixedUpdate() {
        if (CanMove) {
            rb.velocity = new Vector3(input.x * speed, rb.velocity.y, input.z * speed);
        }

        //Rotate on z movement?
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rb.velocity.z * -7.5f, 0), 2f);
        
        if (Input.GetButtonDown("Jump") && isGrounded) {
            isGrounded = false;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetButtonDown("Fire1") && isGrounded) {
            anim.SetTrigger("Attack");
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool IsGrounded() {
        isGrounded = Physics.Raycast(_GroundCheck.position, Vector3.down, groundCheckDistance);
        anim.SetBool("IsGrounded", isGrounded);
        return isGrounded;
    }

    public void Knockback(Vector3 knockback) {
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + knockback, ref velocity, 3f);
    }

    private bool WallCollision() {
        wallCollision = Physics.Raycast(_WallCheck.position, _WallCheck.right, wallCheckDistance);
        return wallCollision;
    }
}

//TODO
//
// Sprint
// Jump Attack
// Stab
// Crouch
// Dash
// Die
// Cast
// Block
// Hurt
// Dizzy