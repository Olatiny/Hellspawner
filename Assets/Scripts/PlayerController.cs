using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float gravity = 75f;

    [SerializeField]
    private float jumpVelocity = 15f;

    [SerializeField]
    private float jumpHoldTime = .15f;

    [SerializeField]
    private float walkSpeed = 10f;

    [SerializeField]
    private float maxFallSpeed = 20f;

    private PlayerInput playerInput;
    private PlayerInputActions inputActions;

    private Rigidbody2D myRigidBody;
    private Collider2D myCollider;

    bool grounded = true;
    bool holdingJump = false;
    Coroutine jumpTimer = null;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        playerInput = GetComponent<PlayerInput>();
        inputActions = new();
        inputActions.PlatformerControls.Enable();

        inputActions.PlatformerControls.Jump.performed += Jump;
        inputActions.PlatformerControls.Jump.canceled += Jump;
        inputActions.PlatformerControls.Attack.performed += Attack;
        inputActions.PlatformerControls.Dash.performed += Dash;
        inputActions.PlatformerControls.Laser.performed += Laser;
        inputActions.PlatformerControls.Frost.performed += Frost;
    }

    private void FixedUpdate()
    {
        // Gravity if not jumping
        if (!holdingJump && myRigidBody.velocity.y > -maxFallSpeed)
            myRigidBody.velocity += gravity * Time.fixedDeltaTime * Vector2.down;

        // Check if Grounded 
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1.5f);
        Debug.DrawRay(gameObject.transform.position, Vector2.down * 1.5f, Color.black, .1f);
        grounded = hit.collider != null && hit.collider != myCollider;

        // Movement
        Vector2 direction = inputActions.PlatformerControls.Move.ReadValue<Vector2>();
        myRigidBody.velocity = new(direction.x * walkSpeed, myRigidBody.velocity.y);
    }

    public void Jump(CallbackContext context)
    {
        if (context.performed && grounded)
        {
            holdingJump = true;
            myRigidBody.velocity = jumpVelocity * Vector2.up;
            jumpTimer = StartCoroutine(JumpHoldTimer(jumpHoldTime));
        }
        else if (context.canceled)
        {
            holdingJump = false;

            if (jumpTimer != null)
            {
                StopCoroutine(jumpTimer);
                jumpTimer = null;
            }
        }
    }

    public void Attack(CallbackContext context)
    {

    }

    public void Dash(CallbackContext context)
    {

    }

    public void Laser(CallbackContext context)
    {

    }

    public void Frost(CallbackContext context)
    {

    }

    IEnumerator JumpHoldTimer(float seconds = 0.5f)
    {
        if (!holdingJump)
            yield return null;

        yield return new WaitForSeconds(seconds);
        holdingJump = false;
        jumpTimer = null;
    }
}
