using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TBEngine.Services;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace GameJam.Services {
    /// <summary>
    /// Configuration service for managing global variables and settings
    /// </summary>
    public sealed class ConfigurationService : ConfigurationServiceBase {

        // Const
        public const int DEF_WindowWidth = 1280;
        public const int DEF_WindowHeight = 720;
        public const Keys DEF_KEY_Console = Keys.OemTilde;
        public const Keys DEF_KEY_MoveUp = Keys.W;
        public const Keys DEF_KEY_MoveDown = Keys.S;
        public const Keys DEF_KEY_MoveLeft = Keys.A;
        public const Keys DEF_KEY_MoveRight = Keys.D;

        // Helpers
        public int WindowWidth => TryGet("window_width", out int res) ? res : DEF_WindowWidth;
        public int WindowHeight => TryGet("window_height", out int res) ? res : DEF_WindowHeight;
        public int ViewWidth => WindowWidth;
        public int ViewHeight => WindowHeight;
        public Keys KEY_Console => TryGet("key_console", out Keys res) ? res : DEF_KEY_Console;
        public Keys KEY_MoveUp => TryGet("key_move_up", out Keys res) ? res : DEF_KEY_MoveUp;
        public Keys KEY_MoveDown => TryGet("key_move_down", out Keys res) ? res : DEF_KEY_MoveDown;
        public Keys KEY_MoveLeft => TryGet("key_move_left", out Keys res) ? res : DEF_KEY_MoveLeft;
        public Keys KEY_MoveRight => TryGet("key_move_right", out Keys res) ? res : DEF_KEY_MoveRight;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceMNG"><see cref="GraphicsDeviceManager"/></param>
        public ConfigurationService(GraphicsDeviceManager deviceMNG) : base(deviceMNG) {
            RegisterMultiple(new Dictionary<string, object>( ) {
                { "window_width", DEF_WindowWidth },
                { "window_height", DEF_WindowHeight },
                { "key_console", DEF_KEY_Console },
                { "key_move_up", DEF_KEY_MoveUp },
                { "key_move_down", DEF_KEY_MoveDown },
                { "key_move_left", DEF_KEY_MoveLeft },
                { "key_move_right", DEF_KEY_MoveRight }
            });
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
