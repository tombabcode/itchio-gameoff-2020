using Microsoft.Xna.Framework;
using TBEngine.Services;

namespace GameJam.Services {
    /// <summary>
    /// Configuration service for managing global variables and settings
    /// </summary>
    public sealed class ConfigurationService : ConfigurationServiceBase {

        public int WindowWidth => TryGet("window_width", out int res) ? res : 1280;
        public int WindowHeight => TryGet("window_height", out int res) ? res : 720;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceMNG"><see cref="GraphicsDeviceManager"/></param>
        public ConfigurationService(GraphicsDeviceManager deviceMNG) : base(deviceMNG) {
            Register("window_width", 1280);
            Register("window_height", 720);
        }

    }
}
