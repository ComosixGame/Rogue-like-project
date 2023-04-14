using UnityEngine;
using UnityEngine.AI;

public class Utility
{
    //tính lực ném theo đường parabol tới 1 điểm
    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
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

    //tính random điểm có thể đi trên nav mesh
    public static Vector3 RandomNavmeshLocation(Vector3 origin, float radius)
    {
        Vector3 finalPosition = Vector3.zero;
        float RandomDistance = Random.Range(5, 11);
        Vector3 randomDirection = Random.insideUnitSphere * RandomDistance;
        randomDirection += origin;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }


    //trả về mảng được xáo trộn từ 1 mảng
    public static T[] ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            T temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }

        return array;
    }
    

    //cắt Rect thành các phần nhỏ
    public static Rect[] SplitRect(Rect rectToSplit, int n){


        Rect[] rects = new Rect[n];

        for(int i = 0; i < n; i++){

            rects[i] = new Rect(rectToSplit.position.x + (i * rectToSplit.width / n), rectToSplit.position.y, rectToSplit.width / n, rectToSplit.height);
        
        }

        int padding = (int)rects[0].width - 40;
        int space = 5;

        rects[0].width -= padding + space;
        rects[2].width -= padding + space;

        rects[1].x -= padding;
        rects[1].width += padding * 2;

        rects[2].x += padding + space;
        

        return rects;

    }
}
