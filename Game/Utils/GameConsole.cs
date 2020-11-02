using Microsoft.Xna.Framework;
using TBEngine.Utils;
using GameJam.Services;
using TBEngine.Types;
using Microsoft.Xna.Framework.Input;
using TBEngine.Models;

using DH = TBEngine.Utils.DisplayHelper;
using COLOR = GameJam.Utils.ColorsManager;
using LOG = TBEngine.Services.LogService;
using TEXT = TBEngine.Utils.TextService;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Utils {
    public sealed class GameConsole : GameConsoleBase {

        private bool _typeWall;
        private ConfigurationService _config;

        private int _width => Scene.Width;
        private int _height => Scene.Height;
        private int _offset => 17;

        private SpriteFont _font => Content.GetFont(FontType.Tiny);

        public GameConsole(InputService input, ContentService content, ConfigurationService config, int width, int height) : base(input, content, width, height) {
            _config = config;
        }

        public override void Update(GameTime time) {
            if (IsVisible) {
                if (Input.IsKeyPressedOnce(Keys.Back) || Input.GetPressedKey(500) == Keys.Back)
                    Command = Command.Length > 0 ? Command.Remove(Command.Length - 1) : "";
                else if (Input.IsKeyPressedOnce(Keys.Enter) && Command.Length > 0) {
                    ExecuteCommand(Command);
                    Command = "";
                } else
                    Command += TextService.GetTextFromKeys(Input, new char[] { '`' });

                _typeWall = time.TotalGameTime.Milliseconds % 1000 >= 500;
            }

            if (Input.IsKeyPressedOnce(KEY_Console))
                IsVisible = !IsVisible;
        }

        public override void Render( ) {
            if (!IsVisible)
                return;

            DH.RenderScene(Scene, color: COLOR.DarkGray * .25f, logic: ( ) => {
                DH.Box(0, _height, _width, 35, COLOR.DarkestGray, AlignType.LB);
                DH.Line(0, _height - 35, _width, _height - 35, color: COLOR.DarkGray);

                // Command input
                DH.Text(_font, $"Command: {Command}" + (_typeWall ? "|" : ""), new Vector2(10, _height - 10), false, COLOR.DarkGray, AlignType.LB);

                // Displaying logs
                if (LOG.Logs.Count == 0)
                    return;

                int row = 0;
                LOG.Logs.ForEach(log => {
                    Color color = LOG.GetColor(log);

                    // Display message
                    if (!string.IsNullOrWhiteSpace(log.Message))
                        foreach (string data in TEXT.GetDividedText(_font, $"[{log.CDate.ToLongTimeString( )}][{log.Type}]: {log.Message}", _width - 20, false))
                            DH.Text(_font, data, new Vector2(10, 10 + (row++) * _offset), false, color);

                    // Display exception
                    if (log.Exception == null)
                        return;

                    string[] raw = TEXT.ExpandException(log.Exception);

                    // Add time and type at the beginning
                    raw[0] = raw[0].Insert(0, $"[{log.CDate.ToLongTimeString( )}][{log.Type}]: ");

                    foreach (string data in raw)
                        foreach (string displayData in TEXT.GetDividedText(_font, data, _width - 20, false))
                            DH.Text(_font, displayData, new Vector2(10, 10 + (row++) * _offset), false, color);
                });
            });
        }

    }
}
