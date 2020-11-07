using Microsoft.Xna.Framework;
using System.Globalization;

namespace GameJam.Utils {
    /// <summary>
    /// Colors management
    /// </summary>
    public static class ColorsManager {

        public static Color DarkGray = new Color(110, 110, 110);
        public static Color DarkestGray = new Color(45, 45, 45);

        public static Color Get(float value, bool alpha = false) 
            => alpha ? new Color(value, value, value, value) : new Color(value, value, value);
        public static Color Get(float r, float g, float b, float alpha = 1)
            => new Color(r, g, b, alpha);
        public static Color Get(string hex) {
            if (string.IsNullOrWhiteSpace(hex) || hex.Length != 6 || hex.Length != 8)
                return Color.White;
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            int a = hex.Length == 8 ? int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber) : 255;

            return new Color(r, g, b, a);
        }

    }
}
