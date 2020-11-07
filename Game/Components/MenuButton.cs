using GameJam.Services;
using GameJam.Types;
using Microsoft.Xna.Framework;
using TBEngine.Components.Elements;

using DH = TBEngine.Utils.DisplayHelper;
using AH = TBEngine.Utils.AudioHelper;
using LANG = TBEngine.Utils.TranslationService;
using GameJam.Utils;

namespace GameJam.Components {
    public sealed class MenuButton : TextButtonBase {

        private InputService _input;
        private ContentService _content;

        public MenuButton(InputService input, ContentService content) {
            _input = input;
            _content = content;

            OnHover = ( ) => AH.Sound(_content.AUDIO_ButtonHover);
        }

        public override void Update(GameTime time) {
            IsHover = _input.IsOver(X, Y, Width, Height);

            if (_input.HasEnterArea(X, Y, Width, Height)) {
                OnHover?.Invoke( );
            }

            if (IsHover && _input.IsLMBPressedOnce( ))
                OnClick?.Invoke( );
        }

        public override void Display( ) {
            if (IsHover)
                DH.Box((int)X, (int)Y, (int)Width, (int)Height, ColorsManager.DarkGray * .25f);

            DH.Text(
                Font ?? _content.GetFont(IsHover ? FontType.Big : FontType.Standard), 
                TextTranslate ? LANG.Get(Text).ToUpper( ) : Text.ToUpper( ), 
                Center, false, TextColor, Align
            );
        }

    }
}
