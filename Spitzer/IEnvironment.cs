using System;
using System.Threading.Tasks;

namespace Spitzer
{
    public interface IEnvironment
    {
        Task<Theme> GetOperatingSystemTheme();
    }

    public enum Theme { Light, Dark }

}
