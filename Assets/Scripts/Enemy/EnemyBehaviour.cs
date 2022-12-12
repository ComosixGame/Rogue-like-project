using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private float idleTimer;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        Move();
    }

    private void Move() {
        if(agent.remainingDistance <= agent.stoppingDistance) {
            idleTimer += Time.deltaTime;
            if(idleTimer >= 0.5f) {
                Vector3 pos = RandomNavmeshLocation(agent.height * 2);
                agent.SetDestination(pos);
                idleTimer = 0;
            }
        }
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

