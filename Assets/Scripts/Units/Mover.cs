using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

public class Mover : MonoBehaviour
{
    [SerializeField] float currentMoveSpeed = .2f;
    [SerializeField] float normalMoveSpeed = .2f;
    [SerializeField] float acceptableDistanceToDestination = .4f;
    public bool isMoving = false;

    Health health;
    Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject destination;

    

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }


    private void CheckMoveDirection()   // Flip sprite
    {
        if (isMoving)
        {
            if(Mathf.Sign(destination.transform.position.x - transform.position.x) > 0)
            {
                //transform.localScale = new Vector2(-0.6f, -0.6f);
                spriteRenderer.flipX = true;
            }
            else
            {
                //transform.localScale = new Vector2(0.6f, 0.6f);
                spriteRenderer.flipX = false;

            }
        }

    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Cancel()
    {
        isMoving = false;
        destination = null;
    }

    public bool HasDestination()
    {
        if(destination == null)
        {
            return false;
        }
        return true;
    }

    public void MoveTo(GameObject destination)
    {
        this.destination = destination;
        if(this.destination != null)
        {
            isMoving = true;
            CheckMoveDirection();
        }
        else
        {
            isMoving = false;
        }
        
    }

    public void UpdateMovement()
    {
        if (destination == null) return;
        else
        {
            isMoving = true;
        }
        if (IsCurrentTargetWithinRange())
        {
            isMoving = false;
            destination = null;
        }
        else
        {
            float step = currentMoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, destination.transform.position, step);
            //CheckMoveDirection();
            animator.SetBool("Idle", false);
            //rigidbody.MovePosition
        }
    }

    public void SetMoveSpeedFactor(float moveSpeedFactor) // Factor between 0 and 1
    {
        currentMoveSpeed = normalMoveSpeed * moveSpeedFactor;
    }

    public void ResetMoveSpeed()
    {
        currentMoveSpeed = normalMoveSpeed;
    }

    public bool IsCurrentTargetWithinRange() 
    {
        float currentTargetDistance = Mathf.Abs(destination.transform.position.x - transform.position.x);
        if (currentTargetDistance <= acceptableDistanceToDestination && (Mathf.Abs(destination.transform.position.y - transform.position.y) < .5f))
        {
            animator.SetBool("Idle", true);
            return true;
        }
        else
        {
            return false;
        }

    }
}
