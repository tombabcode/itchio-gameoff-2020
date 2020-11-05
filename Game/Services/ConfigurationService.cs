using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TBEngine.Services;

namespace GameJam.Services {
    /// <summary>
    /// Configuration service for managing global variables and settings
    /// </summary>
    public sealed class ConfigurationService : ConfigurationServiceBase {

        // Const
        public const int DEF_WindowWidth = 1280;
        public const int DEF_WindowHeight = 720;
        public const Keys DEF_KEY_Console = Keys.OemTilde;

        // Helpers
        public int WindowWidth => TryGet("window_width", out int res) ? res : DEF_WindowWidth;
        public int WindowHeight => TryGet("window_height", out int res) ? res : DEF_WindowHeight;
        public int ViewWidth => WindowWidth;
        public int ViewHeight => WindowHeight;
        public Keys KEY_Console => TryGet("key_console", out Keys res) ? res : DEF_KEY_Console;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceMNG"><see cref="GraphicsDeviceManager"/></param>
        public ConfigurationService(GraphicsDeviceManager deviceMNG) : base(deviceMNG) {
            Register("window_width", DEF_WindowWidth);
            Register("window_height", DEF_WindowHeight);
            Register("key_console", DEF_KEY_Console);
        }

        public override void AssignIngameConfiguration( ) {
            GraphicsDeviceMNG.PreferredBackBufferWidth = WindowWidth;
            GraphicsDeviceMNG.PreferredBackBufferHeight = WindowHeight;
            GraphicsDeviceMNG.ApplyChanges( );

            LogService.Add($"Saving configuration");

            CreateOrSaveConfiguration( );
        }

    }
}
