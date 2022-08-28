using Backend.Domain;

namespace Backend.Services;

public class CustomizationProvider : ICustomizationProvider
{
    public string AssignDisplayName() => "TEMPORARY NAME";

    public TankColors AssignTankColors() => new()
    {
        TankColor = "#000000",
        TurretColor = "#000000"
    };
}
