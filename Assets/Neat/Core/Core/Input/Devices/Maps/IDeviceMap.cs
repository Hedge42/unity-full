using System.Collections.Generic;

namespace Neat.Input.DeviceMaps
{
    public interface IDeviceMap
    {
        Dictionary<GamepadControl, IInputControl> GetDefault(int index);

        IInputControl GetControl(GamepadControl c);
    }
}
