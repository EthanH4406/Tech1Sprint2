using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Debug.Log(x + ", " + y);
        dir = new Vector2(x, y).normalized;
        switch (x)
        {
            case -1:
                facingH = "left";
                break;
            case 1:
                facingH = "right";
                break;
        }
        switch (y)
        {
            case 1:
                facingV = "up";
                break;
            case -1:
                facingV = "down";
                break;
        }
        Debug.Log(facingH + ", " + facingV);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
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
