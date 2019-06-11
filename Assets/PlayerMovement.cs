using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float mass;
    public float damping;

    protected CharacterController characterController;
    protected float velocityY;
    protected Vector3 currentImpact;

    private float speed;
    public float SprintMultiplier;
    private readonly float gravity = Physics.gravity.y;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    protected virtual void Update()
    {
        Move();

        Jump();
    }

    protected virtual void Move()
    {
        speed = Input.GetButton("L3") ? moveSpeed * SprintMultiplier : moveSpeed;

        Vector3 movement = new Vector3(Input.GetAxisRaw("MoveX"),0f,- Input.GetAxisRaw("MoveY")).normalized;

        movement = transform.TransformDirection(movement);
        if(characterController.isGrounded && velocityY < 0)
        {
            velocityY = 0f;
        }
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = movement * speed+ Vector3.up * velocityY;
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }
        characterController.Move(velocity * Time.deltaTime);
        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);
    }

    public void ResetImpact()
    {
        currentImpact = Vector3.zero;
        velocityY = 0f;
    }
    protected void ResetImpactY()
    {
        currentImpact.y = 0f;
        velocityY = 0f;
    }

    protected virtual void Jump()
    {
        if (Input.GetButtonDown("JUMP"))
        {
            addforce(Vector3.up, jumpForce);
        }
    }

    public void addforce(Vector3 direction,float magnitude)
    {
        currentImpact += direction.normalized * magnitude / mass;
    }
 
}
