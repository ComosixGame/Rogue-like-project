using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform weaponHolder;
    public AbsPlayerAttack weapon;
    [HideInInspector] public AbsPlayerAttack playerAttack;
    public float speed;
    [SerializeField] public Rig rigAim;
    [SerializeField] private Transform targetAim;
    private float timerAttack;
    private bool readyAttack;
    private Vector3 dirMove;
    private InputAssets inputs;
    private CharacterController controller;
    private Animator animator;
    private int velocityHash;
    private int dodgeHash;
    private int speedDodgeHash;
    private int attackHash;
    private int aimHash;
    private GameManager gameManager;
    private ObjectPoolerManager ObjectPoolerManager;


    private void Awake() {
        gameManager = GameManager.Instance;
        ObjectPoolerManager = ObjectPoolerManager.Instance;
        inputs = new InputAssets();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        dodgeHash = Animator.StringToHash("Dodge");
        speedDodgeHash = Animator.StringToHash("SpeedDodge");
        attackHash = Animator.StringToHash("Attack");
        aimHash = Animator.StringToHash("Aim");
    }

    private void Start() {
        playerAttack = Instantiate(weapon, weaponHolder.position, weaponHolder.rotation);
        playerAttack.transform.SetParent(weaponHolder);
    }

    private void OnEnable() {
        inputs.PlayerController.Enable();
        inputs.PlayerController.Move.performed += GetDirection;
        inputs.PlayerController.Move.canceled += GetDirection;
    }

    private void Update() {
        Move();
        HandleRotation();
        HandleAnimationMove();
        HandleAttack();
    }


    private void OnDisable() {
        inputs.PlayerController.Disable();
        inputs.PlayerController.Move.performed -= GetDirection;
        inputs.PlayerController.Move.canceled -= GetDirection;
    }

    private void Move() {
        //tính động lực di chuyển nhân vật
        Vector3 motionMove = dirMove * speed * Time.deltaTime;
        //tính độ rơi bởi trọng lực
        Vector3 motionFall =  Vector3.zero;
        if(!controller.isGrounded) {
            motionFall  = Vector3.up * -9.8f * Time.deltaTime;
        }

        controller.Move(motionMove + motionFall);
    }

    private void HandleAttack() {
        if(readyAttack) {
            timerAttack += Time.deltaTime;
            animator.SetBool(aimHash, true);
            rigAim.weight = 1;
            if(timerAttack >= playerAttack.fireRateTime) {
                timerAttack = 0;
                bool notReload = playerAttack.Attack();
                if(notReload) {
                    animator.SetTrigger(attackHash);
                }
            }
        } else {
            animator.SetBool(aimHash, false);
            rigAim.weight = 0;
        }
    }    

    private void HandleRotation() {
        Vector3 dirLook = dirMove;
        if(readyAttack) {
            List<Transform> enemies = gameManager.GetEnemies();
            if(enemies.Count > 0) {
                //chọn kẻ thù gần nhất
                Transform nearestEnemy = enemies.OrderBy(enemy => Vector3.Distance(enemy.position, transform.position)).First();
                dirLook = nearestEnemy.position - transform.position;
                dirLook.y = 0;
                // di chuyển điểm nhắm đến kẻ thù, nhưng vẫn giữ nguyển pos y
                targetAim.position = Vector3.MoveTowards(
                        new Vector3(targetAim.position.x, targetAim.position.y, targetAim.position.z),
                        new Vector3(nearestEnemy.position.x, targetAim.position.y, nearestEnemy.position.z),
                        9999f * Time.deltaTime
                        );
            }
        }

        if(dirLook != Vector3.zero) {
            Quaternion rotLook = Quaternion.LookRotation(dirLook);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotLook, 20f * Time.deltaTime);
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
        readyAttack = velocity == 0 && gameManager.enemiesCount > 0;;
    }
}
