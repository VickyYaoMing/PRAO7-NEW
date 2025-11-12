using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private float speed = 5f;
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void ProcessMove(Vector2 input)
    {
        HandleWalkingAnimation(input);
        Vector3 moveDir = new Vector3(-input.y, 0f, input.x);

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                720f * Time.deltaTime
            );
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = (moveDir.normalized * speed) + new Vector3(0f, verticalVelocity, 0f);
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleWalkingAnimation(Vector2 input)
    {
        bool isWalking = input != Vector2.zero;
        animator.SetBool(StringLiterals.WALKING_ANI, isWalking);
    }
}
