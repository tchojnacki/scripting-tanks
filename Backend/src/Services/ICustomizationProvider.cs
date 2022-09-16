using Backend.Domain;

namespace Backend.Services;

internal interface ICustomizationProvider
{
    string AssignDisplayName();
    TankColors AssignTankColors();
    TankColors AssignTankColors(int seed);
}
