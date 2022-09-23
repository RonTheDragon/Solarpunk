using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float     MovementSpeed;
    public float     JumpHeight;
    public float     JetPackPower;
    public float     GroundCheckSize = 0.1f;
    public ParticleSystem PurpleParticle;
    public ParticleSystem WaterParticle;
    public LayerMask Walkable;

    GameObject MultiTool;

    int SelectedWeapon;
    float         MovementX = 0;
    Rigidbody2D   rb;
    BoxCollider2D _collider;
    Camera        cam;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        MultiTool = transform.GetChild(0).gameObject;
        cam = Camera.main;
    }

    void Update()
    {
        Walking();
        Jumping();
        Aim();
        ToolSwitching();

        switch (SelectedWeapon)
        {
            case 0: JetPackGun(); break;
            case 1: WaterGun(); break;
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
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MovementX = -1;
        }
        else
        {
            MovementX = 0;
        }
    }

    void Jumping()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(0, JumpHeight);
            }
        }
    }

    void Aim()
    {
        Vector3 MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 AimDirection = (MousePos - MultiTool.transform.position).normalized;
        float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
        MultiTool.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void ToolSwitching()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectedWeapon++;
            if (SelectedWeapon > 1) SelectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelectedWeapon--;
            if (SelectedWeapon < 0) SelectedWeapon = 1;
        }
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
            raycolor = Color.green;
        }
        else
        {
            raycolor = Color.red;
        }

        Debug.DrawRay(_collider.bounds.center + new Vector3(_collider.bounds.extents.x, _collider.bounds.extents.y), Vector2.down * (_collider.bounds.extents.y * 2), raycolor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, -_collider.bounds.extents.y), Vector2.down * (_collider.bounds.extents.y * 2), raycolor);
        Debug.DrawRay(_collider.bounds.center - new Vector3(_collider.bounds.extents.x, _collider.bounds.extents.y), Vector2.right * (_collider.bounds.extents.x * 2), raycolor);
        Debug.DrawRay(_collider.bounds.center + new Vector3(-_collider.bounds.extents.x, _collider.bounds.extents.y), Vector2.right * (_collider.bounds.extents.x * 2), raycolor);

        //Anim.SetBool("IsGrounded", daddypls);
        return ImGrounded;
    }

    void JetPackGun()
    {
        if (Input.GetMouseButton(0))
        {
            rb.velocity = -MultiTool.transform.right * JetPackPower * Time.deltaTime;
            if (!PurpleParticle.isEmitting)
            PurpleParticle.Play();
        }
        else
        {
            PurpleParticle.Stop();
        }
        WaterParticle.Stop();
    }

    void WaterGun()
    {
        if (Input.GetMouseButton(0))
        {
        if (!WaterParticle.isEmitting)
                WaterParticle.Play();
        }
        else
        {
            WaterParticle.Stop();
        }
            PurpleParticle.Stop();
    }
}
