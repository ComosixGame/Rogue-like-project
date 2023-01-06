using UnityEngine;

public class CoinItem : AbsItemObjectPool
{
    [SerializeField] private int point;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Instance;
    }

    public override void ActiveItem(Collider other)
    {
        gameManager.UpdateCoin(point);
    }
}
