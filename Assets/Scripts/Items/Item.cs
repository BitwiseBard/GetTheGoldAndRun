
public interface Item {
    void Equip();
    bool Use();
    bool SingleUse();
    bool NeedDirection();
    void SetDirection(int x, int y);
    string GetName();
    int GetSpawnChance();
    bool GetSingleSlot();
}
