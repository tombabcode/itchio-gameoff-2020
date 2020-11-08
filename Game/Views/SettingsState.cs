using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameJam.Components;
using GameJam.Services;
using GameJam.Models;
using GameJam.Types;
using GameJam.Utils;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;
using TBEngine.Utils;

using LANG = TBEngine.Utils.TranslationService;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Views {
    /// <summary>
    /// Game's settings menu
    /// </summary>
    public sealed class SettingsState : State {

        // References
        private readonly ConfigurationService _config;
        private readonly ContentService _content;
        private readonly InputService _input;
        private readonly StateService _state;
        private readonly SpriteFont _font;

        // Actions
        public Action OnResume { get; set; }

        // Components
        private MenuButton _backButton;
        private MenuButton _saveButton;

        private Dictionary<string, object> _cfg;
        private List<string> _changedConfig;
        private Dictionary<string, string> _translations;
        private List<DisplayMode> _supportedResolutions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content"><see cref="ContentService"/></param>
        /// <param name="input"><see cref="InputService"/></param>
        /// <param name="config"><see cref="ConfigurationService"/></param>
        /// <param name="state"><see cref="StateService"/></param>
        public SettingsState(ContentService content, InputService input, ConfigurationService config, StateService state) {
            _input = input;
            _config = config;
            _content = content;
            _state = state;
            _font = content.GetFont(FontType.Small);

            OnStateLoad = ( ) => {
                _cfg = config.Configuration.ToDictionary(entry => entry.Key, entry => entry.Value);
                _changedConfig = new List<string>( );
                _translations = LANG.GetAllTranslations<LanguageModel>( );
                _supportedResolutions = content.Device.Adapter.SupportedDisplayModes
                    .Where(mode => mode.Width >= 1024 && mode.Height >= 768)
                    .OrderBy(entry => entry.Width)
                    .ToList( );
            };

            Initialize(content);
        }

        /// <summary>
        /// State's initialization
        /// </summary>
        /// <param name="content"><see cref="ContentServiceBase"/></param>
        public override void Initialize(ContentServiceBase content) {
            Width = _config.ViewWidth;
            Height = _config.ViewHeight;
            base.Initialize(content);

            _backButton = new MenuButton(_input, _content) { X = 32, Y = Height - 96, Width = 256, Height = 48, Text = "back", Align = AlignType.CM, OnClick = ( ) => OnResume?.Invoke( ) };
            _saveButton = new MenuButton(_input, _content) { X = 32, Y = Height - 144, Width = 256, Height = 48, Text = "save_changes", Align = AlignType.CM, OnClick = ( ) => {
                _changedConfig.ForEach(key => _config.Configuration[key] = _cfg[key]);
                _config.AssignIngameConfiguration( );
                _content.UpdateDynamicContent(_config);
                _state.ReinitializeStates(_content, _config);
            }};
        }

        /// <summary>
        /// State's update
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public override void Update(GameTime time) {
            _changedConfig = _cfg.Where(entry => _config.Configuration[entry.Key] != entry.Value).Select(entry => entry.Key).ToList( );

            if (_input.IsLMBPressedOnce( )) {
                if (_input.IsOver(288, 56, 75, 32)) ChangeLanguage(true);
                else if (_input.IsOver(363, 56, 75, 32)) ChangeLanguage(false);
                else if (_input.IsOver(288, 128, 150, 32)) _cfg["window_fullscreen"] = !(bool)_cfg["window_fullscreen"];
                else if (_input.IsOver(288, 160, 75, 32)) ChangeResolution(true);
                else if (_input.IsOver(363, 160, 75, 32)) ChangeResolution(false);
                else if (_input.IsOver(288, 232, 75, 32)) ChangeVolume("master", -5);
                else if (_input.IsOver(363, 232, 75, 32)) ChangeVolume("master", 5);
                else if (_input.IsOver(288, 264, 75, 32)) ChangeVolume("music", -5);
                else if (_input.IsOver(363, 264, 75, 32)) ChangeVolume("music", 5);
                else if (_input.IsOver(288, 296, 75, 32)) ChangeVolume("sound", -5);
                else if (_input.IsOver(363, 296, 75, 32)) ChangeVolume("sound", 5);
            }

            if (_changedConfig.Count > 0)
                _saveButton.Update(time);
            _backButton.Update(time);
        }

        /// <summary>
        /// State's render
        /// </summary>
        /// <param name="time"><see cref="GameTime"/></param>
        public override void Render(GameTime time) {
            DH.RenderScene(Scene, ( ) => {
                DH.Raw(_content.TEXUI_MenuBG, 32, 0);

                _backButton.Display( );
                if (_changedConfig.Count > 0)
                    _saveButton.Display( );

                int half_size = (_config.WindowWidth - 288) / 2;
                
                DH.Text(_font, "section_general", 296, 48, align: AlignType.LB);
                DH.Line(288, 56, 288 + half_size, 56);
                DisplayOption(0, "language", ( ) => _translations[_cfg["language"].ToString( )], 56);

                DH.Text(_font, "section_graphics", 296, 120, align: AlignType.LB);
                DH.Line(288, 128, 288 + half_size, 128);
                DisplayOption(0, "window_mode", ( ) => LANG.Get("mode_" + ((bool)_cfg["window_fullscreen"] ? "fullscreen" : "window")), 128);
                DisplayOption(1, "resolution", ( ) => $"{_cfg["window_width"]}x{_cfg["window_height"]}", 160);

                DH.Text(_font, "section_audio", 296, 224, align: AlignType.LB);
                DH.Line(288, 232, 288 + half_size, 232);
                DisplayOption(0, "master_volume", ( ) => $"{((float)_cfg["master_volume"] * 100):0}%", 232);
                DisplayOption(1, "music_volume", ( ) => $"{((float)_cfg["music_volume"] * 100):0}%", 264);
                DisplayOption(2, "sound_volume", ( ) => $"{((float)_cfg["sound_volume"] * 100):0}%", 296);

                DH.Text(_font, "section_controls", 296 + half_size, 48, align: AlignType.LB);
                DH.Line(288 + half_size, 56, _config.WindowWidth, 56);
                DisplayOption(0, "key_console", ( ) => $"{_cfg["key_console"]}", 56, true);
                DisplayOption(1, "key_pause", ( ) => $"{_cfg["key_pause"]}", 88, true);
                DisplayOption(2, "key_move_up", ( ) => $"{_cfg["key_move_up"]}", 120, true);
                DisplayOption(3, "key_move_down", ( ) => $"{_cfg["key_move_down"]}", 152, true);
                DisplayOption(4, "key_move_left", ( ) => $"{_cfg["key_move_left"]}", 184, true);
                DisplayOption(5, "key_move_right", ( ) => $"{_cfg["key_move_right"]}", 216, true);

                DH.Line(288 + half_size, 56, 288 + half_size, 256);
            });
        }

        /// <summary>
        /// Display single option (value, buttons and description)
        /// </summary>
        /// <param name="ID">Index of option in current category. Used for coloring odd/even options</param>
        /// <param name="optionID">Name of an option</param>
        /// <param name="value">Value to display</param>
        /// <param name="y">Position Y</param>
        /// <param name="second">Display in second column</param>
        private void DisplayOption(int ID, string optionID, Func<string> value, int y, bool secondColumn = false) {
            int half_size = (_config.WindowWidth - 288) / 2;
            int x = 288 + (secondColumn ? half_size : 0);

            if (ID % 2 == 0)
                DH.Box(x, y, half_size, 32, ColorsManager.DarkGray * .25f);
            DH.Text(_font, value?.Invoke( ), x + 75, y + 18, false, align: AlignType.CM);
            DH.Text(_font, LANG.Get(optionID).ToUpper( ), x + 160, y + 18, false, align: AlignType.LM);
        }

        /// <summary>
        /// Change language to previous or next from <see cref="_translations"/>
        /// </summary>
        /// <param name="toLeft">Is previous value from <see cref="_translations"/></param>
        private void ChangeLanguage(bool toLeft) {
            List<string> data = _translations.Keys.ToList( );
            int currentID = data.IndexOf(_cfg["language"].ToString( ));
            currentID += toLeft ? -1 : 1;

            if (currentID < 0) currentID = data.Count - 1;
            else if (currentID >= data.Count) currentID = 0;

            _cfg["language"] = _translations.ElementAt(currentID).Key;
            LANG.LoadTranslations<LanguageModel>(_cfg["language"].ToString( ));
        }

        /// <summary>
        /// Change resolution to previous or next from <see cref="_supportedResolutions"/>
        /// </summary>
        /// <param name="toLeft">Is previous value from <see cref="_supportedResolutions"/></param>
        private void ChangeResolution(bool toLeft) {
            DisplayMode mode = _supportedResolutions.Find(res => res.Width == (int)_cfg["window_width"] && res.Height == (int)_cfg["window_height"]);
            int currentID = mode == null ? -1 : _supportedResolutions.IndexOf(mode);

            currentID += toLeft ? -1 : 1;

            if (currentID < 0) currentID = _supportedResolutions.Count - 1;
            else if (currentID >= _supportedResolutions.Count) currentID = 0;

            _cfg["window_width"] = _supportedResolutions[currentID].Width;
            _cfg["window_height"] = _supportedResolutions[currentID].Height;
        }

        /// <summary>
        /// Change master, sound or music volume
        /// </summary>
        /// <param name="id">"master", "sound" or "music" ID</param>
        /// <param name="value">Change value, in percentage (e.g. 5 means 5% (so 0.05 will be added)</param>
        private void ChangeVolume(string id, int value) {
            string key = $"{id}_volume";
            float current = (float)_cfg[key];
            float updated = current + (value / 100f);

            if (updated < 0) _cfg[key] = 0f;
            else if (updated > 1) _cfg[key] = 1f;
            else _cfg[key] = updated;

            switch (id) {
                case "master": AudioHelper.MasterVolume = updated; break;
                case "music": AudioHelper.MusicVolume = updated; break;
                case "sound": AudioHelper.SoundVolume = updated; break;
            }

            AudioHelper.Update( );
        }

    }
}
