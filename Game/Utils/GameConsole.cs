using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using GameJam.Services;
using TBEngine.Utils.Elements;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;
using COLOR = GameJam.Utils.ColorsManager;
using LOG = TBEngine.Services.LogService;
using TEXT = TBEngine.Utils.TextService;

namespace GameJam.Utils {
    /// <summary>
    /// In-game console
    /// </summary>
    public sealed class GameConsole : GameConsoleBase {

        // Services
        private ConfigurationService _config;

        // Should display "|" sign that occures while typing
        private bool _typeWall;

        // Helpers
        private SpriteFont Font => Content.GetFont(FontType.Tiny);
        private Keys KEY_Console => _config.TryGet("key_console", out Keys key) ? key : Keys.OemTilde;
        private int LineHeight => 17;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="input"><see cref="InputService"/></param>
        /// <param name="content"><see cref="ContentService"/></param>
        /// <param name="width">Width of the console</param>
        /// <param name="height">Height of the console</param>
        public GameConsole(InputService input, ContentService content, ConfigurationService config, int width, int height) : base(input, content, width, height) {
            _config = config;
        }

        /// <summary>
        /// Update console's logic
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public override void Update(GameTime time) {
            if (IsVisible) {
                if (Input.IsKeyPressedOnce(Keys.Back) || Input.GetPressedKey(500) == Keys.Back)
                    Command = Command.Length > 0 ? Command.Remove(Command.Length - 1) : "";
                else if (Input.IsKeyPressedOnce(Keys.Enter) && Command.Length > 0) {
                    ExecuteCommand(Command);
                    Command = "";
                } else {
                    char? consoleKey = TEXT.GetCharacterFromKey(KEY_Console, Input);
                    Command += TEXT.GetTextFromKeys(Input, consoleKey.HasValue ? new[] { consoleKey.Value } : null);
                }

                _typeWall = time.TotalGameTime.Milliseconds % 1000 >= 500;
            }

            if (Input.IsKeyPressedOnce(KEY_Console))
                IsVisible = !IsVisible;
        }

        public override void Render(GameTime time) {
            if (!IsVisible)
                return;

            DH.RenderScene(Scene, color: COLOR.DarkestGray * .75f, logic: ( ) => {
                DH.Box(0, (int)Height, (int)Width, 35, COLOR.DarkestGray, AlignType.LB);
                DH.Line(0, Height - 35, Width, Height - 35, color: COLOR.DarkGray);

                // Command input
                DH.Text(Font, $"Command: {Command}" + (_typeWall ? "|" : ""), new Vector2(10, Height - 10), false, COLOR.DarkGray, AlignType.LB);

                // Displaying logs
                if (LOG.Logs.Count == 0)
                    return;

                int row = 0;
                LOG.Logs.ForEach(log => {
                    Color color = LOG.GetColor(log);

                    // Display message
                    if (!string.IsNullOrWhiteSpace(log.Message))
                        foreach (string data in TEXT.GetDividedText(Font, $"[{log.CDate.ToLongTimeString( )}][{log.Type}]: {log.Message}", Width - 20, false))
                            DH.Text(Font, data, new Vector2(10, 10 + (row++) * LineHeight), false, color);

                    // Display exception
                    if (log.Exception == null)
                        return;

                    string[] raw = TEXT.ExpandException(log.Exception);

                    // Add time and type at the beginning
                    raw[0] = raw[0].Insert(0, $"[{log.CDate.ToLongTimeString( )}][{log.Type}]: ");

                    foreach (string data in raw)
                        foreach (string displayData in TEXT.GetDividedText(Font, data, Width - 20, false))
                            DH.Text(Font, displayData, new Vector2(10, 10 + (row++) * LineHeight), false, color);
                });
            });
        }

    }
}
