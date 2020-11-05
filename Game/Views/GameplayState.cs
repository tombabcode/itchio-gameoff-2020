using GameJam.Gameplay;
using GameJam.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBEngine.Components.State;
using TBEngine.Services;
using TBEngine.Types;
using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Views {
    public sealed class GameplayState : View {

        // References
        private InputService _input;
        private ContentService _content;
        private ConfigurationService _config;

        private Player _player;
        private CameraService _camera;

        private RenderTarget2D GameplayScene;

        public GameplayState(ContentService content, InputService input, ConfigurationService config) : base(content, config.ViewWidth, config.ViewHeight) {
            _input = input;
            _config = config;
            _content = content;

            GameplayScene = new RenderTarget2D(content.Device, config.ViewWidth, config.ViewHeight);
        }

        public void NewGame(string playerName = null) {
            _player = new Player(playerName ?? "Unknown");
            _camera = new CameraService(0.25f, new Vector2(_config.WindowWidth / 2, _config.WindowHeight / 2));
        }

        public override void Update(GameTime time) {
            _player.Update(_input, _config, time);
            _camera.Update( );
        }

        public override void Render( ) {
            DH.RenderScene(GameplayScene, _camera, ( ) => {
                DH.Raw(_content.TEXTest.Texture, (int)_player.PositionX, (int)_player.PositionY, align: AlignType.CB);
            });

            DH.RenderScene(Scene, ( ) => {
                DH.Scene(GameplayScene);
                DH.Text(_content.GetFont( ), _player.Name, 15, 15, false);
            });
        }

    }
}
