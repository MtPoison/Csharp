
namespace MapEntities;

public class EnemyMap
{
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public int WorldX { get; set; }
    public int WorldY { get; set; }
    public bool CombatStart = false;

    public EnemyMap(int worldX, int worldY, int localX, int localY)
    {
        WorldX = worldX;
        WorldY = worldY;
        LocalX = localX;
        LocalY = localY;
    }
}