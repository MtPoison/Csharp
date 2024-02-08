
namespace MapEntities;

public class EnemyMap
{
    private int LocalX;
    private int LocalY;
    private int WorldX;
    private int WorldY;
    private bool CombatStart;
    public string AI_Level { get; set; }
    public ConsoleColor Color { get; set; }

    public EnemyMap(int worldX, int worldY, int localX, int localY, string aiLevel)
    {
        WorldX = worldX;
        WorldY = worldY;
        LocalX = localX;
        LocalY = localY;
        AI_Level = aiLevel;
    }

    public int WORLDX
    {

        get => WorldX;
        set => WorldX = value;
    }

    public int WORLDY
    {

        get => WorldY;
        set => WorldY = value;
    }

    public int LOCALX
    {

        get => LocalX;
        set => LocalX = value;
    }

    public int LOCALY
    {

        get => LocalY;
        set => LocalY = value;
    }

    public bool COMBATSTART
    {
        get => CombatStart;
        set => CombatStart = value;
    }
}