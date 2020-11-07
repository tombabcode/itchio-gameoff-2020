using GameJam.Components;
using GameJam.Services;
using GameJam.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Views {
    public sealed class PauseState : State {

        // References
        private ConfigurationService _config;
        private ContentService _content;
        private InputService _input;

        public Action OnResume { get; set; }

        private List<MenuButton> _buttons;

        public PauseState(ContentService content, InputService input, ConfigurationService config) {
            _content = content;
            _config = config;
            _input = input;

            Initialize(content);
        }

        public override void Initialize(ContentServiceBase content) {
            Width = _config.ViewWidth;
            Height = _config.ViewHeight;

            _buttons = new List<MenuButton>( ) {
                new MenuButton(_input, _content) { X = 32, Y = Height - 96, Width = 256, Height = 48, Text = "back", Align = AlignType.CM,
                    OnClick = ( ) => OnResume?.Invoke( ) }
            };

            base.Initialize(content);
        }

        public override void Update(GameTime time) {
            _buttons.ForEach(button => button.Update(time));
        }

        public override void Render(GameTime time) {
            DH.RenderScene(Scene, ( ) => {
                DH.Raw(_content.TEXUI_MenuBG, 32, 0);

                _buttons.ForEach(button => {
                    if (button.IsHover)
                        DH.Box((int)button.X, (int)button.Y, (int)button.Width, (int)button.Height, ColorsManager.DarkGray * .25f);
                    button.Display( );
                });
            });
        }

    }
}
