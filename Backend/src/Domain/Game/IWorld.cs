namespace Backend.Domain.Game;

internal interface IWorld
{
    public const double SeaHeight = -0.5; // m
    public static readonly Vector Gravity = new(0, -2, 0); // m/s^2

    double Radius { get; }
    IEnumerable<Tank> Tanks { get; }

    void Spawn(Entity entity);
    void Destroy(Entity entity);
}
