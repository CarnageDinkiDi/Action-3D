using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneController : PlayerMovement
{
    public int jumpCount=0;
    public bool CanMove;
    public GameObject axe;

    private void Awake()
    {
        CanMove = true;
    }
    protected override void Update()
    {
        if (CanMove)
        {
            base.Update();

            if (characterController.isGrounded)
            {
                jumpCount = 0;
            }

        }


    }
    protected override void Jump()
    {
        if (Input.GetButtonDown("JUMP"))
        {
            if (jumpCount == 0)
            {
                ResetImpactY();
                addforce(Vector3.up, jumpForce);
                if (characterController.isGrounded)
                {
                    jumpCount = 1;
                }
                else
                {
                    jumpCount = 2;
                }
            }
            else if (jumpCount == 1)
            {
                ResetImpactY();
                addforce(Vector3.up, jumpForce);
                jumpCount = 2;
            }
        }
    }
}
