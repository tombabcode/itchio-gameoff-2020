using GameJam.Services;
using Microsoft.Xna.Framework;
using TBEngine.Components.Elements;

using DH = TBEngine.Utils.DisplayHelper;

namespace GameJam.Components {
    public sealed class TextButton : TextButtonBase {

        private InputService _input;
        private ContentService _content;

        public TextButton(InputService input, ContentService content) {
            _input = input;
            _content = content;
        }

        public override void Update(GameTime time) {
            if (_input.HasSwitchedArea(X, Y, Width, Height)) {
                IsHover = _input.IsOver(X, Y, Width, Height);
                OnHover?.Invoke( );
            }

            if (IsHover && _input.IsLMBPressedOnce( ))
                OnClick?.Invoke( );
        }

        public override void Display( ) {
            DH.Text(Font ?? _content.GetFont( ), Text, Center, TextTranslate, TextColor, Align);
        }

    }
}
