using UnityEngine;
using UnityEngine.AI;
using MyCustomAttribute;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private float idleTimer;
    public Transform shootPosition;
    private bool readyAttack, fired;
    private Transform _player;
    public float delayAttack = 3.0f;
    [ReadOnly, SerializeField] private float timerAttack;
    public AbsAttach _absAttach;
    private bool isRotation;
    [ReadOnly] public bool inStun;
    private GameManager gameManager;
    private LoadSceneManager loadSceneManager;
    private int run;
    private Animator _animator;
    private int _velocityMove;
    private int _StandAiming;
    [SerializeField] private float maxSpeed;

    //fix player
    private void Awake() {
        gameManager = GameManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        _absAttach = GetComponent<AbsAttach>();
        _absAttach.Init();
        _animator = GetComponent<Animator>();

        _velocityMove = Animator.StringToHash("Velocity");
        _StandAiming = Animator.StringToHash("StandAiming");
    }

    private void OnEnable() {
        _absAttach.OnAttacked += ResetAttack;
        loadSceneManager.OnSceneLoaded += sceneLoad;
        loadSceneManager.OnLoadScene += sceneLoaded;
    }

    private void Start() {
        _player = gameManager.player;
    }

    public void sceneLoad(Scene scene){
        _player = gameManager.player;
    }

    public void sceneLoaded(){
        _player = gameManager.player;
    }

    private void Update() {
        if(inStun) {
            agent.ResetPath();
            agent.isStopped = true;
        } else{
            Move();
            HandleAttack();
        }
    }

    private void OnDisable() {
        _absAttach.OnAttacked -= ResetAttack;
        loadSceneManager.OnSceneLoaded -= sceneLoad;
        loadSceneManager.OnLoadScene -= sceneLoaded;
    }

    private void Move() {
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float Velocity = horizontalVelocity.magnitude / maxSpeed;
        if(readyAttack){
            agent.ResetPath();
            agent.SetDestination(transform.position);
            _animator.SetFloat(_velocityMove, Velocity);
        }
        else{
            if(agent.remainingDistance <= agent.stoppingDistance) {
                idleTimer += Time.deltaTime;
                if(idleTimer >= 0.5f) {
                    Vector3 pos = Utility.RandomNavmeshLocation(transform.position, agent.height * 2);
                    agent.ResetPath();
                    agent.SetDestination(pos);
                    idleTimer = 0;
                   _animator.SetFloat(_velocityMove, Velocity);
                }
            }
            _animator.SetFloat(_velocityMove, Velocity);
        }
    }

    private void HandleAttack(){
        if(!readyAttack) {
            timerAttack += Time.deltaTime;
            if(timerAttack >= delayAttack){
                _animator.SetBool(_StandAiming, true);  
                timerAttack = 0;
                readyAttack = true;
            }
        } else {
            Vector3 dirLook = (_player.transform.position - transform.position).normalized;
            dirLook.y = 0;
            //Thuc hien xoay 
            if(dirLook != Vector3.zero){
                Quaternion rotLook = Quaternion.LookRotation(dirLook);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotLook, 5f * Time.deltaTime);
            }
            if(Vector3.Angle(transform.forward, dirLook) <= 0.3f) {
                if(!fired) {
                    fired = true;  
                    _absAttach.Attack(shootPosition);
                }
            }
        }
    }

    private void ResetAttack() {
        readyAttack = false;
        fired = false;
    }
}

