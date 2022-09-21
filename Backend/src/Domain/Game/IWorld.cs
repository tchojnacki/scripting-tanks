namespace Backend.Domain.Game;

internal interface IWorld
{
    public const double SeaHeight = -0.5;
    public static readonly Vector Gravity = new(0, -2, 0);

    double Radius { get; }
    IEnumerable<Tank> Tanks { get; }

    void Spawn(Entity entity);
    void Destroy(Entity entity);
}
