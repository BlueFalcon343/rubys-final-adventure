using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{
    public float speed = 5.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public GameObject projectilePrefab;

    public int health { get { return currentHealth;}}
    public int currentHealth;
    public int cogCount;

    bool isInvincible;
    float invincibleTimer;
    public bool playerInput;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    public AudioSource audioSource;

    public AudioClip thrownClip;
    private Scene activeScene;
    public AudioClip backgroundMusic;
    public AudioClip winMusic;

    public ParticleSystem hitEffect;
    public ParticleSystem healthEffect;
    public ParticleSystem poisonEffect;
    public TextMeshProUGUI cogText;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerInput = true; 
        currentHealth = maxHealth;
        cogCount = 8;
        SetCogText();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        poisonEffect.Stop();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput == true)
        {

        
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);
        
            if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
        
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0)
                isInvincible = false;
            
        }
        

         if (currentHealth <= 0)
        {
            Restart();
        }
        
        if (playerInput == true)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                if(cogCount > 0)
                {
                    cogCount = cogCount - 1;
                    SetCogText();
                    Launch();
                }
            }
            

            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                    NonPlayCharacterRambi characterRambi = hit.collider.GetComponent<NonPlayCharacterRambi>();
                    if (characterRambi != null)
                    {
                        characterRambi.DisplayDialog();
                    }
                    TorgCharacter characterTorg = hit.collider.GetComponent<TorgCharacter>();
                    if (characterTorg != null)
                    {
                        characterTorg.DisplayDialog();
                    }
                }
            }
        }    
    }

    void FixedUpdate()
    {
        if(playerInput == true)
        {
       
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime; 

            rigidbody2d.MovePosition(position);
        }    
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            
            if(isInvincible)
                return;

            isInvincible = true;
            hitEffect.Play();
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    void Launch()
    {
        PlaySound(thrownClip);
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    
    public void SetCogText()
    {
        cogText.text = (cogCount.ToString());
    }

    public void SpeedUp()
    {
        StartCoroutine(Boost());
    }

    public void PoisonHealth()
    {
        poisonEffect.Play();
        StartCoroutine(Poison());
    }

    IEnumerator Boost()
    {
        speed = 10.0f;
        yield return new WaitForSeconds(5);
        speed = 5.0f;
    }

    IEnumerator Poison()
    {
        ChangeHealth(-1);
        yield return new WaitForSeconds(3);
        ChangeHealth(-1);
        yield return new WaitForSeconds(3);
        ChangeHealth(-1);
        poisonEffect.Stop();
    }


    

    
}
