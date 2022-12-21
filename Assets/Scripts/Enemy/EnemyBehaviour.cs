using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private float idleTimer;

    public GameObject bullet;

    public Transform shootPosition;
    private float delayAttack = 3.0f, timerAttack, timerVolleyOfBullets = 1000.0f;
    private bool readyAttack;

    public Transform _player;

    [SerializeField] private Transform _targetAim;
    private bool isFireBullet, isMove;
    [Range(0,360), SerializeField] private float angel;
    private float angelPerShot;
    public AbsAttach _absAttach;
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        _absAttach.Init();
    }
   

    private void Update() {
        Move();
        HandleRotation();
        _absAttach.HandleAttack(shootPosition);
    }

    private void Move() {
        //khi ban thi dung lai
        if(isFireBullet == true){
            agent.ResetPath();
        }
        else{
            if(agent.remainingDistance <= agent.stoppingDistance) {
                idleTimer += Time.deltaTime;
                if(idleTimer >= 0.5f) {
                    Vector3 pos = RandomNavmeshLocation(agent.height * 2);
                    agent.SetDestination(pos);
                    idleTimer = 0;
                }
            }
        }
        isFireBullet = false;
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

    private void HandleRotation(){
        //Lay huong de ban
        Vector3 dirLook = (_player.position - transform.position).normalized;
        dirLook.y = 0;
        
        //Thuc hien xoay 
        if(dirLook != Vector3.zero){
            Quaternion rotLook = Quaternion.LookRotation(dirLook);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotLook, 20f * Time.deltaTime);
        }
        
    }
}

