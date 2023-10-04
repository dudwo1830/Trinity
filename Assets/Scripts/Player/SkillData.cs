public class SkillData
{
    enum Target
    {
        Player, 
        Enemy, 
        Enemys,
        EnemyAll,
        All
    }

    public string name { get; private set; } = string.Empty;
    public string description { get; private set; } = string.Empty;
    public bool IsRate { get; private set; } = false;
    public float damage { get; private set; } = 0;
    public float attack { get; private set; } = 0;
    public float defense { get; private set; } = 0;
    public float heal { get; private set; } = 0;
    public float duration { get; private set; } = 0;

}
