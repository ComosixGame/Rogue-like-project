using UnityEngine;
using UnityEngine.AI;

public class Utils
{
    //tính lực ném theo đường parabol tới 1 điểm
    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time) {
        Vector3 dis = target - origin;
        Vector3 disXZ = dis;
        disXZ.y = 0;

        float Sy = dis.y;
        float Sxz = disXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 reuslt = disXZ.normalized;
        reuslt *= Vxz;
        reuslt.y = Vy;

        return reuslt;
    }

    public static Vector3 RandomNavmeshLocation(Vector3 origin, float radius) {
        //tính random điểm có thể đi trên nav mesh
        Vector3 finalPosition = Vector3.zero;
        float RandomDistance = Random.Range(5, 11);
        Vector3 randomDirection = Random.insideUnitSphere * RandomDistance;
        randomDirection += origin;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;  
        }
        return finalPosition;
    }
}
