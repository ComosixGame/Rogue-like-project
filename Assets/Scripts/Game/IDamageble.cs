using Vector3 = UnityEngine.Vector3;
public interface IDamageble
{
    public void TakeDamge(float damage, Vector3 force);
    public void Destroy();
}
