public interface IPooledItem
{
    public void Activate();
    public void DeSpawn();
    void SetPoolSpawner(ulong poolSpawner);
}