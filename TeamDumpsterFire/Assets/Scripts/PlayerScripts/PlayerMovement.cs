using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    public float dashSpeed = 6.5f;
    public float dashTime = 0.35f;
    public float dashCooldown = 0.5f;
    Vector2 dir;
    public Rigidbody2D rb;
    bool canMove;
    bool canDash;
    bool dashing;
    string facingH;
    string facingV;
    public ParticleSystem dashParticles;
    public Animator anim;
    public SpriteRenderer sprite;

    public InputActionReference Movement;
    public InputActionReference dash;
    public PauseMenuBehaviour pause;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        canDash = true;
        dashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementDirection = Movement.action.ReadValue<Vector2>();

        float x = movementDirection.x;
        float y = movementDirection.y;

        //Debug.Log(x + ", " + y);
        dir = new Vector2(x, y).normalized;
        switch (y)
        {
            case 1:
                facingV = "up";
                break;
            case -1:
                facingV = "down";
                break;
        }
        switch (x)
        {
            case -1:
                facingH = "left";
                break;
            case 1:
                facingH = "right";
                break;
        }

        if (x == 0 && y == 0)
        {
            //ClearWalkCycle();
            //anim.SetBool("idle", true);

            if(!pause.isPaused)
            {
                anim.SetBool("walking", false);
            }

            
            //Debug.Log("what");
        }
        else
        {
            ClearWalkCycle();

			if (!pause.isPaused)
			{
				anim.SetBool("walking", true);
			}

			
            if (x != 0)
            {

				if (!pause.isPaused)
				{
                    anim.SetBool("side", true);
				}


				if (!pause.isPaused)
				{
					if (x > 0)
					{
						sprite.flipX = true;
					}
					else
					{
						sprite.flipX = false;
					}
				}

            }
            else if (y > 0)
            {
				if (!pause.isPaused)
				{
                    anim.SetBool("up", true);
				}

				
            }
            else
            {
				if (!pause.isPaused)
				{
                    anim.SetBool("down", true);
				}

				
            }
        }
        //Debug.Log(facingH + ", " + facingV);

        if (dash.action.WasPressedThisFrame() && canDash)
        {
            canMove = false;
            canDash = false;
            dashing = true;
            dashParticles.Play();
            if (x == 0 && y == 0)
            {
                switch (facingH)
                {
                    case "left":
                        dir.x = -1;
                        break;
                    case "right":
                        dir.x = 1;
                        break;
                }
                switch (facingV)
                {
                    case "up":
                        dir.y = 1;
                        break;
                    case "down":
                        dir.y = -1;
                        break;
                }
            }
            StartCoroutine("DashTime");
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
        }
        if (dashing)
        {
            rb.velocity = new Vector2(dir.x * dashSpeed, dir.y * dashSpeed);
        }
        //dir = new Vector2(dir.x - 0.1f, dir.y - 0.1f);
    }

    void ClearWalkCycle()
    {
		//anim.SetBool("walking", false);
		//anim.SetBool("idle", false);

		if (!pause.isPaused)
		{
            anim.SetBool("side", false);
            anim.SetBool("down", false);
            anim.SetBool("up", false);
		}

		
    }

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;
        //canDash = true;
        canMove = true;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
