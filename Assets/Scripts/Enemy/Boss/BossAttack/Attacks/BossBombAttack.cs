using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossBombAttack : AbsBossAttack
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int amount;
    [SerializeField] private float delayAttack;
    [SerializeField] private Bomb bomb;
    [SerializeField] private GameObjectPool warningSign;
    private List<GameObjectPool> warningSigns;
    [SerializeField] private Transform player;
    private int timesFire = 1;
    private GameManager gameManager;
    private ObjectPoolerManager objectPooler;

    private void Awake() {
        gameManager = GameManager.Instance;        
        objectPooler = ObjectPoolerManager.Instance;

        warningSigns = new List<GameObjectPool>();
    }

    public override void Attack()
    {   
        StartCoroutine(Fire());
    }

    IEnumerator Fire() {
        while(timesFire <= amount) {
            Vector3 target = gameManager.player.position;
            GameObjectPool warningSignClone = objectPooler.SpawnObject(warningSign, target + Vector3.up * 0.01f, Quaternion.identity);
            Vector3 dir = target - transform.position;
            Bomb bombClone = objectPooler.SpawnObject(bomb, shootPoint.position, shootPoint.rotation) as Bomb;
            
            //ẩn warningSign
            bombClone.OnExplosive += ()=> objectPooler.DeactiveObject(warningSignClone);

            float radius = bombClone.radius;
            warningSignClone.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            float dis = Vector3.Distance(target, transform.position);
            Vector3 v = Utility.CalculateVelocity(target, shootPoint.position, dis * 0.08f);
            bombClone.GetComponent<Rigidbody>().AddForce(v , ForceMode.Impulse);
            timesFire ++;
            yield return new WaitForSeconds(delayAttack);
        }
        AttackeComplete();
        timesFire = 1;
    }
}
