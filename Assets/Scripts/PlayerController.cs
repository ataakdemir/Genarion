using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    [Header("Movement")]
    public float speed;
    float horizontalInput;
    float verticalInput;

    [Header("Jump")]
    public float jumpForce;
    public LayerMask groundMask;
    public bool grounded = true;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    private bool isAttackLocked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();

        // Input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Camera-based movement direction
        Transform cam = Camera.main.transform;
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(cam.right, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;

        // Eğer hareket edebiliyorsak:
        if (CanMove())
        {
            // Hareket
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

            // Dönüş
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
            }

            // Animator'a hız bildir
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            animator.SetFloat("Speed", flatVelocity.magnitude);
        }
        else
        {
            // Hareketi durdur ama dönüş serbest
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);
            }

            // Hızı sıfırla
            animator.SetFloat("Speed", 0f);
            return;
        }

        // Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            HandleJump();
        }

        // Saldırı
        if (Input.GetMouseButtonDown(0) && grounded)
        {
            HandleAttack();
        }
    }

    void GroundCheck()
    {
        grounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
        animator.SetBool("isJumping", !grounded);
    }

    void HandleJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void HandleAttack()
    {
        animator.SetTrigger("isAttacking"); // Trigger parametresi Animator'da tanımlı olmalı
        StartCoroutine(AttackLock());
    }

    IEnumerator AttackLock()
    {
        isAttackLocked = true;
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return !stateInfo.IsName("Sword And Shield Attack") || stateInfo.normalizedTime >= 1f;
        });
        yield return new WaitForSeconds(0.05f); // animator'ın state geçişi yapması için süre
        isAttackLocked = false;
    }

    bool CanMove()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Eğer saldırı animasyonundayken veya kilitliyse hareket etme
        if (stateInfo.IsName("Sword And Shield Attack") && stateInfo.normalizedTime < 1f)
            return false;

        if (isAttackLocked)
            return false;

        return true;
    }
}
