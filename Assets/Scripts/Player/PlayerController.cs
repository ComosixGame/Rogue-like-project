using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public AnimationClip animationClip;
    public float speed;
    public float dodgeTime;
    public LayerMask layeDodgeable;
    private Vector3 dirMove;
    private bool startDodge, dodging;
    private InputAssets inputs;
    private CharacterController controller;
    private Animator animator;
    private int velocityHash;
    private int dodgeHash;
    private int speedDodgeHash;

    private void Awake() {
        inputs = new InputAssets();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        dodgeHash = Animator.StringToHash("Dodge");
        speedDodgeHash = Animator.StringToHash("SpeedDodge");
    }

    private void OnEnable() {
        inputs.PlayerController.Enable();
        inputs.PlayerController.Move.performed += GetDirection;
        inputs.PlayerController.Move.canceled += GetDirection;
        inputs.PlayerController.Dodge.performed += HandleInputDodge;
    }

    private void Update() {
        Move();
        HandleRotation();
        HandleAnimationMove();
    }

    private void OnDisable() {
        inputs.PlayerController.Disable();
        inputs.PlayerController.Move.performed -= GetDirection;
        inputs.PlayerController.Move.canceled -= GetDirection;
        inputs.PlayerController.Dodge.performed -= HandleInputDodge;
    }

    private void Move() {
        //tính động lực di chuyển nhân vật
        Vector3 motionMove = dirMove * speed * Time.deltaTime;
        //tính độ rơi bởi trọng lực
        Vector3 motionFall =  Vector3.zero;
        if(!controller.isGrounded) {
            motionFall  = Vector3.up * -9.8f * Time.deltaTime;
        }
        if(!dodging) {
        //di chuyển
            controller.Move(motionMove + motionFall);
        } else {
        // thực hiện lăn
            controller.Move(transform.forward.normalized * speed * Time.deltaTime);
            if(!startDodge) {
                startDodge = true;
                Invoke("ResetDodge", dodgeTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(!dodging) return;
        HandleDodgeObtacle(other, true);
    }

    private void OnTriggerStay(Collider other) {
        if(!dodging) return;
        HandleDodgeObtacle(other, true);

    }

    private void OnTriggerExit(Collider other) {
        HandleDodgeObtacle(other, false);

    }
    

    private void HandleRotation() {
        if(dodging) return;
        if(dirMove != Vector3.zero) {
            Quaternion rotLook = Quaternion.LookRotation(dirMove);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotLook, 20f * Time.deltaTime);
        }
    }

    private void HandleInputDodge(InputAction.CallbackContext ctx) {
        if(dodging) return;
        dodging = ctx.ReadValueAsButton();
        animator.SetTrigger(dodgeHash);
        float speed = animationClip.length / dodgeTime;
        animator.SetFloat(speedDodgeHash, speed);

    }

    private void ResetDodge() {
        dodging = false;
        startDodge = false;
    }

    private void HandleDodgeObtacle (Collider other, bool isTrigger) {
        // thực hiên lăn qua vật cản có thể lăn được
        GameObject gameObject = other.gameObject;
        if((layeDodgeable & (1<< gameObject.layer)) != 0) {
            if(gameObject.TryGetComponent(out Collider collider)) {
                collider.isTrigger = isTrigger;
            }
        }
    }

    private void GetDirection(InputAction.CallbackContext ctx) {
        //lấy hướng di chuyển
        Vector2 dir = ctx.ReadValue<Vector2>();
        dirMove = new Vector3(dir.x, 0, dir.y);
    }

    private void HandleAnimationMove() {
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float velocity = horizontalVelocity.magnitude/speed;
        if(velocity > 0) {
            animator.SetFloat(velocityHash, velocity);
        } else {
            float v = animator.GetFloat(velocityHash);
            v = v> 0.01f ? Mathf.Lerp(v, 0, 20f * Time.deltaTime): 0;
            animator.SetFloat(velocityHash, v);
        }
    }
}
