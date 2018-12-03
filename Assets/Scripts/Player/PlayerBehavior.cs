using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehavior : MonoBehaviour {

    public Sprite rightSprite, leftSprite;

    public bool autoFire = false;
    public float bulletCooldown = 0.1f;
    public float shootyForce = 15;
    public float runSpeed = 2f;
    public float maxFall = 5f;
    public float jumpForce = 5f;
    public float jumpGravMult = 0.9f;
    public float runAccel = 4f;
    public float drag = 3f;

    public bool facingLeft = false;
    public BulletBehavior bulletTemplate;
    public Rigidbody2D rb2;
    protected GlobalGameData ggd;
    protected Collider2D feet;
    protected Transform shootyPoint;
    protected AudioManagerBehavior audioManager;

    protected LayerMask ground;
    private bool jumped = false;
    private float lastShot = 0;
    private bool canShoot = true;
    private bool oldLeft = false;
    protected float lastMoveInput = 0;

    private SpriteRenderer renderer;
    private Animator anim;

    // Use this for initialization
    public virtual void Start()
    {
        audioManager = FindObjectOfType<AudioManagerBehavior>();
        rb2 = GetComponent<Rigidbody2D>();
        ggd = FindObjectOfType<GlobalGameData>();
        ground = LayerMask.GetMask("Ground");
        feet = transform.Find("feet").GetComponent<Collider2D>();
        shootyPoint = transform.Find("shootyPoint");
        renderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    //void Update () {
    //	
    //}

    // once per phys update
    void FixedUpdate()
    {
        preHandle();

        handleShoot();
        handleMovement();
        handleJump();

        handleAnims();

        postHandle();
    }

    protected void handleMovement()
    {
        Vector2 result = Vector2.zero;
        if (rb2.velocity.y > 0)
        {
            result += Vector2.down * ggd.gravity * jumpGravMult;
        }
        else if(!feet.IsTouchingLayers(ground))
        {
            result += Vector2.down * ggd.gravity;
        }

        float mv = getMovement();
        lastMoveInput = mv;
        if (mv < -ggd.accuracyLimit)
        {
            facingLeft = true;
        }
        else if (mv > ggd.accuracyLimit)
        {
            facingLeft = false;
        }

        result += Vector2.right * mv * runAccel;

        if (Mathf.Abs(rb2.velocity.x) > runSpeed && Mathf.Sign(result.x) == Mathf.Sign(rb2.velocity.x))
        {
            result.Set(0, result.y);
        }
        if (-rb2.velocity.y > maxFall && result.y < 0)
        {
            result.Set(result.x, 0);
        }

        if (rb2.velocity.x !=0 &&feet.IsTouchingLayers(ground) && (Mathf.Abs(result.x) <= ggd.accuracyLimit || Mathf.Sign(result.x) != Mathf.Sign(rb2.velocity.x)))
        {
            result.Set(-Mathf.Sign(rb2.velocity.x) * drag, result.y);
        }

        rb2.AddForce(result, ForceMode2D.Force);
        if (Mathf.Abs(result.x) > 0 && Mathf.Abs(rb2.velocity.x) < ggd.accuracyLimit)
        {
            rb2.velocity.Set(0, rb2.velocity.y);
        }
    }

    public void remoteJump(Vector2 dir)
    {
        audioManager.playClipHere("jump",transform.position);
        rb2.AddForce(dir * jumpForce, ForceMode2D.Impulse);
    }

    void handleJump()
    {
        float j = getJump();
        if(feet.IsTouchingLayers(ground)){
            if (j > ggd.accuracyLimit && !jumped)
            {
                jumped = true;
                audioManager.playClipHere("jump", transform.position);
                rb2.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if (j <= ggd.accuracyLimit)
            {
                jumped = false;
            }
        }
    }

    void handleShoot()
    {
        if(shootyPoint == null){return;}
        Vector2 shootDir = getShoot();
        if (Time.fixedTime - lastShot >= bulletCooldown
            && (autoFire || shootDir.magnitude < ggd.accuracyLimit))
        {
            canShoot = true;
        }
        if (canShoot && bulletTemplate != null && shootDir.magnitude > ggd.accuracyLimit)
        {
            canShoot = false;
            lastShot = Time.fixedTime;
            GameObject go = GameObject.Instantiate(bulletTemplate.gameObject);
            BulletBehavior created = go.GetComponent<BulletBehavior>();
            if (shootDir.x < 0)
            {
                created.initVel = shootDir * shootyForce;
                created.transform.position = new Vector2(transform.position.x - shootyPoint.localPosition.x,
                    transform.position.y);
                go.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                created.initVel = shootDir * shootyForce;
                created.transform.position = shootyPoint.position;
            }
            created.GetComponent<Rigidbody2D>().velocity = rb2.velocity;
            audioManager.playClipHere("shoot", transform.position);
        }
    }

    //override me
    public virtual float getMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(input) >= ggd.accuracyLimit){
            return input;
        }
        return 0f;
    }

    //override me
    public virtual float getJump()
    {
        return Input.GetAxisRaw("Jump");
    }

    //override me daddy
    public virtual Vector2 getShoot()
    {
        if (facingLeft)
        {
            return new Vector2(-Input.GetAxisRaw("Fire"),0);
        }
        else
        {
            return new Vector2(Input.GetAxisRaw("Fire"), 0);
        }
        
    }

    public void handleAnims()
    {
        if (facingLeft != oldLeft && facingLeft)
        {
            renderer.sprite = leftSprite;
        }
        else if (facingLeft != oldLeft)
        {
            renderer.sprite = rightSprite;
        }
        oldLeft = facingLeft;
        if ( isOnGround() && (lastMoveInput > ggd.accuracyLimit || lastMoveInput < -ggd.accuracyLimit) )
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    public virtual void preHandle()
    {

    }

    public virtual void postHandle()
    {

    }

    public bool isOnGround()
    {
        return feet.IsTouchingLayers(ground);
    }

    public void onDeath()
    {
        GunBehavior ngb = GetComponent<GunBehavior>();
        MusclesBehavior nmb = GetComponent<MusclesBehavior>();
        audioManager.playClipHere("explode", transform.position);
        if (ngb == null && nmb == null)
        {
            Destroy(gameObject);
        }
        else if (nmb != null)
        {
            nmb.remoteDrop();
            nmb.transform.position = new Vector2(-100, -100); //just move him elsewhere...
        }
        else if (ngb != null)
        {
            string current = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(current);
        }
    }
}
