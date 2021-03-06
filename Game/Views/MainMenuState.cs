﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameJam.Components;
using GameJam.Services;
using GameJam.Types;
using GameJam.Utils;
using TBEngine.Components.Elements;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;

using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Views {
    /// <summary>
    /// State of main menu
    /// </summary>
    public sealed class MainMenuState : State {

        // References
        private readonly ConfigurationService _config;
        private readonly ContentService _content;
        private readonly InputService _input;
        private readonly StateService _state;
        private readonly GameConsole _console;

        private List<Button> _buttons;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentService"/></param>
        /// <param name="input"><see cref="InputService"/></param>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        /// <param name="state"><see cref="StateService"/></param>
        /// <param name="console"><see cref="GameConsole"/></param>
        public MainMenuState(ContentService content, InputService input, ConfigurationService config, StateService state, GameConsole console) {
            _console = console;
            _content = content;
            _config = config;
            _input = input;
            _state = state;

            Initialize(content);
        }

        /// <summary>
        /// State's initialization
        /// </summary>
        /// <param name="content"><see cref="ContentServiceBase"/></param>
        public override void Initialize(ContentServiceBase content) {
            Width = _config.ViewWidth;
            Height = _config.ViewHeight;
            
            _buttons = new List<Button>( ) {
                new MenuButton(_input, _content) { X = 32, Y = Height - 384, Width = 256, Height = 48, Align = AlignType.CM, Text = "new_game", OnClick = ( ) => _console.ExecuteCommand("new_game") },
                new MenuButton(_input, _content) { X = 32, Y = Height - 288, Width = 256, Height = 48, Align = AlignType.CM, Text = "how_to_play", OnClick = ( ) => _state.ChangeState(GameStateType.Tutorial) },
                new MenuButton(_input, _content) { X = 32, Y = Height - 240, Width = 256, Height = 48, Align = AlignType.CM, Text = "settings", OnClick = ( ) => {
                    _state.ChangeState(GameStateType.Settings);
                    ((SettingsState)_state.GetCurrentState( )).OnResume = ( ) => _state.ChangeState(GameStateType.MainMenu);
                }},
                new MenuButton(_input, _content) { X = 32, Y = Height - 192, Width = 256, Height = 48, Align = AlignType.CM, Text = "credits", OnClick = ( ) => _state.ChangeState(GameStateType.Credits) },
                new MenuButton(_input, _content) { X = 32, Y = Height - 96, Width = 256, Height = 48, Align = AlignType.CM, Text = "exit_app", OnClick = ( ) => _console.ExecuteCommand("exit") }
            };
            // Temporary, if there is game save, continue button should be displayed
            _buttons.Add(new MenuButton(_input, _content) { X = 32, Y = Height - 432, Width = 256, Height = 48, Align = AlignType.CM, Text = "continue" });
            
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

                _buttons.ForEach(button => button.Display( ));
            });
        }

    }
}
