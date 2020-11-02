using Microsoft.Xna.Framework;
using TBEngine.Services;

namespace GameJam.Services {
    /// <summary>
    /// Configuration service for managing global variables and settings
    /// </summary>
    public sealed class ConfigurationService : ConfigurationServiceBase {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceMNG"><see cref="GraphicsDeviceManager"/></param>
        public ConfigurationService(GraphicsDeviceManager deviceMNG) : base(deviceMNG) {
        }

    }
}
