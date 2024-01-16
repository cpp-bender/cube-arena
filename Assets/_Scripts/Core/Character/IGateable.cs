public interface IGateable
{
    public void IncreaseStackCount(int stackCount, Cube cubePrefab);
    public void DecreaseStackCount(int stackCount);
    public int StackCount();
}
