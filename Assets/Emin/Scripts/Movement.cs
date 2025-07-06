using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")] 
    public float speed = 5f;
    public float runSpeed = 8f; 
    public float injuredSpeed = 3f;    
    private float _horizontalInput;
    private float _verticalInput;

    [Header("Jump")]
    public float jumpForce;
    public LayerMask groundMask;
    public bool grounded = true;
    public Transform groundCheck;
    public float groundDistance = 0.2f;

    private GoblinAnimationContoller _goblinAnimationController;
    private Camera _cam;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _goblinAnimationController = GetComponent<GoblinAnimationContoller>();
        _cam = Camera.main;
    }

    void Update()
    {
        GroundCheck();

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isInjured = Input.GetKey(KeyCode.LeftControl);

        float currentSpeed;

        if (isRunning)
            currentSpeed = runSpeed;
        else if (isInjured)
            currentSpeed = injuredSpeed;
        else
            currentSpeed = speed;

        Vector3 camForward = Vector3.Scale(_cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(_cam.transform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = (camForward * _verticalInput + camRight * _horizontalInput).normalized;

        if (moveDirection != Vector3.zero)
        {
            _rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, _rb.linearVelocity.y, moveDirection.z * currentSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 10f * Time.deltaTime);

            if (isRunning)
                _goblinAnimationController.GetRunAnimation();
            else if (isInjured)
                _goblinAnimationController.GetInjuredWalkAnimation();
            else
                _goblinAnimationController.GetWalkAnimation();
        }
        else
        {
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);

            Vector3 lookDirection = new Vector3(camForward.x, 0, camForward.z).normalized;
            if (lookDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }

            _goblinAnimationController.GetIdleAnimation();
        }
        
    }

    void GroundCheck()
    {
        grounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
    }

   
}
