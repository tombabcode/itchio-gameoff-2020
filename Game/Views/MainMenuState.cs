using GameJam.Components;
using GameJam.Services;
using GameJam.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TBEngine.Components.Elements;
using TBEngine.Components.State;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;
using AH = TBEngine.Utils.AudioHelper;

namespace GameJam.Views {
    public sealed class MainMenuState : View {

        private ContentService _content;
        private List<Button> _buttons;

        public MainMenuState(ContentService content, InputService input, ConfigurationService config, GameConsole console) : base(content, config.WindowWidth, config.WindowHeight) {
            _content = content;
            _buttons = new List<Button>( ) {
                new TextButton(input, content) { X = 32, Y = Height - 256, Width = 256, Height = 32, Align = AlignType.CM, Text = "new_game",
                    OnHover = ( ) => AH.Sound(content.AUDIO_ButtonHover), OnClick = ( ) => console.ExecuteCommand("new_game") },
                new TextButton(input, content) { X = 32, Y = Height - 96, Width = 256, Height = 32, Align = AlignType.CM, Text = "exit_app", 
                    OnHover = ( ) => AH.Sound(content.AUDIO_ButtonHover), OnClick = ( ) => console.ExecuteCommand("exit") }
            };
        }

        public override void Update(GameTime time) {
            _buttons.ForEach(button => button.Update(time));
        }
        
        public override void Render( ) {
            DH.RenderScene(Scene, ( ) => {
                DH.Box(32, 0, 256, (int)Height, ColorsManager.DarkGray * .25f);

                _buttons.ForEach(button => {
                    if (button.IsHover)
                        DH.Box((int)button.X, (int)button.Y, (int)button.Width, (int)button.Height, ColorsManager.DarkGray * .25f);
                    button.Display( );
                });
            });
        }

    }
}
