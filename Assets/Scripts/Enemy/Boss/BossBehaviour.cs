using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using MyCustomAttribute;

public class BossBehaviour : MonoBehaviour
{
   private NavMeshAgent agent;
    private float idleTimer;
    private Transform _player;
    [SerializeField, ReadOnly] private AbsBossAttack[] bossAttacks;
    private int indexAttack;
    private bool readyAttack, fired;
    [SerializeField] private float delayAttack;
    private float timerAttack;
    private GameManager gameManager;
    private LoadSceneManager loadSceneManager;

    //fix player
    private void Awake() {
        gameManager = GameManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        bossAttacks = GetComponents<AbsBossAttack>();
    }

    private void OnEnable() {
        foreach(AbsBossAttack bossAttack in bossAttacks) {
            bossAttack.OnAttackeComplete += ResetAttack;
        }
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
        Move();
        HandleAttack();
    }

    private void OnDisable() {
        foreach(AbsBossAttack bossAttack in bossAttacks) {
            bossAttack.OnAttackeComplete -= ResetAttack;
        }
        loadSceneManager.OnSceneLoaded -= sceneLoad;
        loadSceneManager.OnLoadScene -= sceneLoaded;
    }

    private void Move() {
        if(readyAttack){
            agent.ResetPath();
            agent.SetDestination(transform.position);
        } else {
            if(agent.remainingDistance <= agent.stoppingDistance) {
                idleTimer += Time.deltaTime;
                if(idleTimer >= 0.5f) {
                    Vector3 pos = Utility.RandomNavmeshLocation(transform.position, agent.height * 2);
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
                    bossAttacks[indexAttack].Attack();
                    indexAttack++;
                    if(indexAttack >= bossAttacks.Length) {
                        indexAttack = 0;
                    }
                }
            }


        }
    }

    private void ResetAttack() {
        readyAttack = false;
        fired = false;
    }
}
