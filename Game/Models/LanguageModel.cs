using System.Collections.Generic;

namespace GameJam.Models {
    public sealed class LanguageModel {
        public Dictionary<string, string> Buttons { get; set; } = new Dictionary<string, string>( );
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>( );
    }
}
