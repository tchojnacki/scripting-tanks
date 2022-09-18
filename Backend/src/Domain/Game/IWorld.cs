namespace Backend.Domain.Game;

internal interface IWorld
{
    double Radius { get; }
    IEnumerable<Tank> Tanks { get; }

    void Spawn(Entity entity);
    void Destroy(Entity entity);
}
