using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using MyCustomAttribute;

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
    private GameManager gameManager;

    //fix player
    private void Awake() {
        gameManager = GameManager.Instance;
        agent = GetComponent<NavMeshAgent>();
        _absAttach = GetComponent<AbsAttach>();
        _absAttach.Init();

    }

    private void OnEnable() {
        _absAttach.OnAttacked += ResetAttack;
    }

    private void Start() {
        _player = gameManager.player;
    }

    private void Update() {
        Move();
        HandleAttack();
    }

    private void OnDisable() {
        _absAttach.OnAttacked -= ResetAttack;
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
                    Vector3 pos = RandomNavmeshLocation(agent.height * 2);
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

    private Vector3 RandomNavmeshLocation(float radius) {
        //tính random điểm có thể đi trên nav mesh
        Vector3 finalPosition = Vector3.zero;
        float RandomDistance = Random.Range(5, 11);
        Vector3 randomDirection = Random.insideUnitSphere * RandomDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;  
        }
        return finalPosition;
    }
}

