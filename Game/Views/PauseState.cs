using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using GameJam.Components;
using GameJam.Services;
using GameJam.Utils;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Views {
    /// <summary>
    /// Game's pause state
    /// </summary>
    public sealed class PauseState : State {

        // References
        private readonly ConfigurationService _config;
        private readonly ContentService _content;
        private readonly InputService _input;

        public Action OnResume { get; set; }

        private List<MenuButton> _buttons;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentService"/></param>
        /// <param name="input"><see cref="InputService"/></param>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        public PauseState(ContentService content, InputService input, ConfigurationService config) {
            _content = content;
            _config = config;
            _input = input;

            Initialize(content);
        }
        
        /// <summary>
        /// State's initialization
        /// </summary>
        /// <param name="content"><see cref="ContentServiceBase"/></param>
        public override void Initialize(ContentServiceBase content) {
            Width = _config.ViewWidth;
            Height = _config.ViewHeight;

            _buttons = new List<MenuButton>( ) {
                new MenuButton(_input, _content) { X = 32, Y = Height - 96, Width = 256, Height = 48, Text = "back", Align = AlignType.CM,
                    OnClick = ( ) => OnResume?.Invoke( ) }
            };

            base.Initialize(content);
        }

        /// <summary>
        /// State's update
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public override void Update(GameTime time) {
            _buttons.ForEach(button => button.Update(time));
        }

        /// <summary>
        /// Render state
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
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
