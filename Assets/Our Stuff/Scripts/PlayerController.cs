using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public           float          MovementSpeed;
    public           float          JumpHeight;
    public           float          JetPackPower;
    public           float          GroundCheckSize = 0.1f;
    public           float          MaxSpeed;
    public           GameObject     PurpleCannon;
    public           GameObject     WaterCannon;
    public           GameObject     SuckCannon;
    public           ParticleSystem PurpleParticle;
    public           ParticleSystem WaterParticle;
    public           ParticleSystem SuckParticle;
    public           LayerMask      Walkable;
                     Animator       anim;
    [SerializeField] float          damage;

    public           float maxHp;                      //HEALTH
    public           float HpRegen;
    [SerializeField] float Hp;
    public           bool  alreadyDead;
    private          float damageSoundCooldown;

    private          bool  alreadyLanded;

    GameObject MultiTool;

                     int         SelectedWeapon;
                     float       MovementX = 0;
                     Rigidbody2D rb;
                     Collider2D  _collider;
                     Camera      cam;
    [SerializeField] GameObject  loseMenu;

    [SerializeField] float maxFuel;                    //FUEL
    [SerializeField] float currentFuel;
    [SerializeField] float fuelRegen;
    [SerializeField] float fuelDepletionRate;
    private          float fuelSoundCooldown;

    [SerializeField] float maxWater;                   //WATER
    [SerializeField] float water;
    [SerializeField] float waterDepletionRate;
    private          float waterSoundCooldown;

    private          bool  alreadyStartedSucking;      //SUCKING
    private          float startSuckingSoundCooldown;
    private          float middleSuckingSoundCooldown;

    public HealthBar healthBar;
    public HealthBar fuelBar;
    public HealthBar waterBar;

    AudioManager audioManager;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        rb           = GetComponent<Rigidbody2D>();
        _collider    = GetComponent<Collider2D>();
        MultiTool    = transform.GetChild(0).gameObject;
        cam          = Camera.main;
        anim         = GetComponent<Animator>();
        Hp           = maxHp;
        currentFuel  = maxFuel;
        water        = maxWater;
        healthBar.SetMaxHealth(maxHp);
        fuelBar.SetMaxHealth(maxFuel);
        waterBar.SetMaxHealth(maxWater);
    }

    void Update()
    {
        Walking();
        Jumping();
        Aim();
        ToolSwitching();
        HealthSystem();
        CooldownManager();

        switch (SelectedWeapon)
        {
            case 0: JetPackGun(); break;
            case 1: WaterGun();   break;
            case 2: SuckGun();    break;
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(MovementX, 0) * Time.deltaTime * MovementSpeed;
    }

    void Walking()
    {
        if (Input.GetKey(KeyCode.D))
        {
            MovementX = 1;
            anim.SetInteger("Direction",-1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MovementX = -1;
            anim.SetInteger("Direction", 1);
        }
        else
        {
            MovementX = 0;
            anim.SetInteger("Direction", 0);
        }

        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }
    }

    void Jumping()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += new Vector2(0, JumpHeight);
            }
            else
            {
                rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude - Time.deltaTime);
            }
            if (!alreadyLanded)
            {
                audioManager.PlaySound(Sound.Activation.Custom, "Landing");
                alreadyLanded = true;
            }
        }
        else
        {
            alreadyLanded = false;
        }
    }

    void Aim()
    {
        if (Time.timeScale == 1)
        {
            Vector3 MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 AimDirection = (MousePos - MultiTool.transform.position).normalized;
            float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
            MultiTool.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    void ToolSwitching()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectedWeapon++;
            if (SelectedWeapon > 2) SelectedWeapon = 0;
            Switched();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectedWeapon--;
            if (SelectedWeapon < 0) SelectedWeapon = 2;
            Switched();
        }
        
    }

    void Switched()
    {
        audioManager.PlaySound(Sound.Activation.Custom, "Mode Change");
        alreadyStartedSucking = false;
        WaterCannon.SetActive(false); SuckCannon.SetActive(false); PurpleCannon.SetActive(false);
        switch (SelectedWeapon)
        {
            case 0: PurpleCannon.SetActive(true); break;
            case 1: WaterCannon.SetActive(true);  break;
            case 2: SuckCannon.SetActive(true);   break;
        }
    }

    void Fuel()
    {
        currentFuel -= fuelDepletionRate * Time.deltaTime;
        fuelBar.SetHealth(currentFuel);
    }

    private bool IsGrounded()
    {
        //RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f,
        //Vector2.down, 1, layerMask);
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center - new Vector3(0, _collider.bounds.extents.y + GroundCheckSize),
        new Vector3(_collider.bounds.size.x - 0.2f, GroundCheckSize/2), 0f, Vector2.down, 0, Walkable);
        Color raycolor;

        bool ImGrounded = raycastHit.collider != null;

        if (ImGrounded)
        {
            if (raycastHit.collider.isTrigger)
            {
                ImGrounded = false;
            }
            raycolor = Color.green;
        }
        else
        {
            raycolor = Color.red;
        }

        Debug.DrawRay(_collider.bounds.center + new Vector3(_collider.bounds.extents.x,  _collider.bounds.extents.y),  Vector2.down  * (_collider.bounds.extents.y * 2), raycolor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x,  -_collider.bounds.extents.y), Vector2.down  * (_collider.bounds.extents.y * 2), raycolor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x,  _collider.bounds.extents.y),  Vector2.right * (_collider.bounds.extents.x * 2), raycolor);
        Debug.DrawRay(_collider.bounds.center + new Vector3(-_collider.bounds.extents.x, _collider.bounds.extents.y),  Vector2.right * (_collider.bounds.extents.x * 2), raycolor);

        //Anim.SetBool("IsGrounded", daddypls);
        return ImGrounded;
    }

    void JetPackGun()
    {
        if (Input.GetMouseButton(0) && currentFuel > 0)
        {
           // rb.velocity = MultiTool.transform.right * JetPackPower * Time.deltaTime;
            rb.AddForce(-MultiTool.transform.right * JetPackPower * Time.deltaTime);
            if (!PurpleParticle.isEmitting)
            PurpleParticle.Play();
            Fuel();
            if (fuelSoundCooldown <= 0 && Time.timeScale != 0)
            {
                audioManager.PlaySound(Sound.Activation.Custom, "Jetpack");
                fuelSoundCooldown = 0.5f;
            }
        }
        else
        {
            PurpleParticle.Stop();
        }
        WaterParticle.Stop();
        SuckParticle.Stop();
    }

    void WaterGun()
    {
        if (Input.GetMouseButton(0) && water > 0)
        {
            if (!WaterParticle.isEmitting)
            {
                WaterParticle.Play();
            }
            if (waterSoundCooldown <= 0)
            {
                audioManager.PlaySound(Sound.Activation.Custom, "Water");
                waterSoundCooldown = 1;
            }
            waterSoundCooldown -= Time.deltaTime;
            water -= waterDepletionRate * Time.deltaTime;
            waterBar.SetHealth(water);
        }
        else
        {
            WaterParticle.Stop();
        }
        PurpleParticle.Stop();
        SuckParticle.Stop();
    }

    void SuckGun()
    {
        if (Input.GetMouseButton(0))
        {
            if (!SuckParticle.isEmitting)
            {
                if (!alreadyStartedSucking)
                {
                    audioManager.PlaySound(Sound.Activation.Custom, "Vacuum Start");
                    middleSuckingSoundCooldown = 1.5f;
                }
                SuckParticle.Play();
                alreadyStartedSucking = true;
            }
            if (alreadyStartedSucking && middleSuckingSoundCooldown <= 0)
            {
                audioManager.PlaySound(Sound.Activation.Custom, "Vacuum Middle");
                middleSuckingSoundCooldown = 1;
            }
        }
        else
        {
            SuckParticle.Stop();
            if (alreadyStartedSucking)
            {
                audioManager.PlaySound(Sound.Activation.Custom, "Vacuum Stop");
            }
            alreadyStartedSucking = false;
            startSuckingSoundCooldown = 1.5f;
        }
        PurpleParticle.Stop();
        WaterParticle.Stop();
    }

    void CooldownManager()
    {
        if (damageSoundCooldown > 0)
        {
            damageSoundCooldown -= Time.deltaTime;
        }
        if (fuelSoundCooldown > 0)
        {
            fuelSoundCooldown -= Time.deltaTime;
        }
        if (middleSuckingSoundCooldown > 0)
        {
            middleSuckingSoundCooldown -= Time.deltaTime;
        }
    }

    void HealthSystem()
    {
        if (Hp < 0) { Death(); }

        if (Hp < maxHp) 
        {
            Hp += HpRegen * Time.deltaTime;
            healthBar.SetHealth(Hp);
        }

        if (currentFuel < maxFuel && IsGrounded())
        {
            currentFuel += fuelRegen * Time.deltaTime;
            fuelBar.SetHealth(currentFuel);
        }
    }

    public void Death()
    {
        if (!alreadyDead)
        {
            //play death animation
            audioManager.PlaySound(Sound.Activation.Custom, "Death");
            audioManager.PlaySound(Sound.Activation.Custom, "Lose Theme");
           // GameManager.instance.gameObject.GetComponent<>
            Time.timeScale = 0;
            loseMenu.gameObject.SetActive(true);
            alreadyDead = true;
        }
    }

    

    private void OnCollisionStay2D(Collision2D collision) //TAKE DAMAGE
    {
        if (collision.transform.tag == "Enemy")
        {
            Hp -= damage * Time.deltaTime;
            healthBar.SetHealth(Hp);
            if (damageSoundCooldown <= 0)
            {
                audioManager.PlaySound(Sound.Activation.Custom, "Damage");
                damageSoundCooldown = 3;
            }
        }
    }

    public void RefilWater()
    {
        water = maxWater;
        waterBar.SetHealth(water);
    }
}
