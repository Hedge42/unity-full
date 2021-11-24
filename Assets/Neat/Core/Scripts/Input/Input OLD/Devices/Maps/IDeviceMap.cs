using System.Collections.Generic;

namespace Neat.InputManagement.DeviceMaps
{
    public interface IDeviceMap
    {
        Dictionary<GamepadControl, IInputControl> GetDefault(int index);

        IInputControl GetControl(GamepadControl c);
    }
}
