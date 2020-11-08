using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TBEngine.Services;
using TBEngine.Utils;

namespace GameJam.Services {
    /// <summary>
    /// Configuration service for managing global variables and settings
    /// </summary>
    public sealed class ConfigurationService : ConfigurationServiceBase {

        // Defaults
        public const string DEF_Language = "en";
        public const bool DEF_WindowFullscreen = false;
        public const int DEF_WindowWidth = 1280;
        public const int DEF_WindowHeight = 720;
        public const Keys DEF_KEY_Console = Keys.OemTilde;
        public const Keys DEF_KEY_Pause = Keys.P;
        public const Keys DEF_KEY_MoveUp = Keys.W;
        public const Keys DEF_KEY_MoveDown = Keys.S;
        public const Keys DEF_KEY_MoveLeft = Keys.A;
        public const Keys DEF_KEY_MoveRight = Keys.D;
        public const bool DEF_DebugMode = false;
        public const float DEF_MasterVolume = 1f;
        public const float DEF_MusicVolume = .4f;
        public const float DEF_SoundVolume = .25f;

        // Helpers
#pragma warning disable IDE0075 // Uprość wyrażenie warunkowe
        public string Language => TryGet("language", out string res) ? res : DEF_Language;
        public bool WindowFullscreen => TryGet("window_fullscreen", out bool res) ? res : DEF_WindowFullscreen;
        public int WindowWidth => TryGet("window_width", out int res) ? res : DEF_WindowWidth;
        public int WindowHeight => TryGet("window_height", out int res) ? res : DEF_WindowHeight;
        public int ViewWidth => WindowWidth;
        public int ViewHeight => WindowHeight;
        public Keys KEY_Console => TryGet("key_console", out Keys res) ? res : DEF_KEY_Console;
        public Keys KEY_Pause => TryGet("key_pause", out Keys res) ? res : DEF_KEY_Pause;
        public Keys KEY_MoveUp => TryGet("key_move_up", out Keys res) ? res : DEF_KEY_MoveUp;
        public Keys KEY_MoveDown => TryGet("key_move_down", out Keys res) ? res : DEF_KEY_MoveDown;
        public Keys KEY_MoveLeft => TryGet("key_move_left", out Keys res) ? res : DEF_KEY_MoveLeft;
        public Keys KEY_MoveRight => TryGet("key_move_right", out Keys res) ? res : DEF_KEY_MoveRight;
        public bool DebugMode => TryGet("debug_mode", out bool res) ? res : DEF_DebugMode;
        public float MasterVolume => TryGet("master_volume", out float res) ? res : DEF_MasterVolume;
        public float MusicVolume => TryGet("music_volume", out float res) ? res : DEF_MusicVolume;
        public float SoundVolume => TryGet("sound_volume", out float res) ? res : DEF_SoundVolume;
#pragma warning restore IDE0075 // Uprość wyrażenie warunkowe

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceMNG"><see cref="GraphicsDeviceManager"/></param>
        public ConfigurationService(GraphicsDeviceManager deviceMNG) : base(deviceMNG) {
            RegisterMultiple(new Dictionary<string, object>( ) {
                { "language", DEF_Language },
                { "window_fullscreen", DEF_WindowFullscreen },
                { "window_width", DEF_WindowWidth },
                { "window_height", DEF_WindowHeight },
                { "key_console", DEF_KEY_Console },
                { "key_pause", DEF_KEY_Pause },
                { "key_move_up", DEF_KEY_MoveUp },
                { "key_move_down", DEF_KEY_MoveDown },
                { "key_move_left", DEF_KEY_MoveLeft },
                { "key_move_right", DEF_KEY_MoveRight },
                { "debug_mode", DEF_DebugMode },
                { "master_volume", DEF_MasterVolume },
                { "music_volume", DEF_MusicVolume },
                { "sound_volume", DEF_SoundVolume }
            });
        }

        /// <summary>
        /// Update and apply app's configuration
        /// </summary>
        public override void AssignIngameConfiguration( ) {
            GraphicsDeviceMNG.IsFullScreen = WindowFullscreen;
            GraphicsDeviceMNG.PreferredBackBufferWidth = WindowWidth;
            GraphicsDeviceMNG.PreferredBackBufferHeight = WindowHeight;
            GraphicsDeviceMNG.ApplyChanges( );

            AudioHelper.SoundVolume = SoundVolume;
            AudioHelper.MusicVolume = MusicVolume;
            AudioHelper.MasterVolume = MasterVolume;
            AudioHelper.Update( );

            LogService.Add($"Saving configuration");

            CreateOrSaveConfiguration( );
        }

        /// <summary>
        /// Change value for given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="saveConfig"></param>
        public void Change(string key, object value, bool saveConfig = false) {
            if (Configuration.ContainsKey(key)) {
                Configuration[key] = value;
                if (saveConfig)
                    CreateOrSaveConfiguration( );
            }
        }

    }
}
