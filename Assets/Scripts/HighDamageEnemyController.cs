using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighDamageEnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        if(!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    
    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.PoisonHealth();
        }
    }

    public void Fix()
    {
        EnemyController.fixCount = EnemyController.fixCount + 1;
        if (EnemyController.fixCount >= 16)
        {
            RubyController player = GameObject.Find("Ruby").GetComponent<RubyController>();
            player.audioSource.clip = player.winMusic;
            player.audioSource.Play();    
            player.playerInput = false;
        }
        EnemyController robot = GameObject.Find("Robot").GetComponent<EnemyController>();
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        EnemyController.count = EnemyController.count + 1;
        robot.SetCountText();
    }

    
}
