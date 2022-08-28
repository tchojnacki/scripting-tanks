using Backend.Domain;

namespace Backend.Services;

public interface ICustomizationProvider
{
    string AssignDisplayName();
    TankColors AssignTankColors();
}
