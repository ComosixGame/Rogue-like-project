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

    //fix player
    private void Awake() {
        gameManager = GameManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        _absAttach = GetComponent<AbsAttach>();
        _absAttach.Init();

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
        if(readyAttack){
            agent.ResetPath();
            agent.SetDestination(transform.position);
        }
        else{
            if(agent.remainingDistance <= agent.stoppingDistance) {
                idleTimer += Time.deltaTime;
                if(idleTimer >= 0.5f) {
                    Vector3 pos = Utils.RandomNavmeshLocation(transform.position, agent.height * 2);
                    agent.ResetPath();
                    agent.SetDestination(pos);
                    idleTimer = 0;
                }
            }
        }
    }

    private void HandleAttack(){
        if(!readyAttack) {
            timerAttack += Time.deltaTime;
            if(timerAttack >= delayAttack){
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
            if(Vector3.Angle(transform.forward, dirLook) <= 0.1f) {
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

