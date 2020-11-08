using System.Globalization;
using Microsoft.Xna.Framework;

namespace GameJam.Utils {
    /// <summary>
    /// Colors management
    /// </summary>
    public static class ColorsManager {

        public static Color DarkGray = new Color(110, 110, 110);
        public static Color DarkestGray = new Color(45, 45, 45);

        /// <summary>
        /// Get gray color
        /// </summary>
        /// <param name="value">Gray strength</param>
        /// <param name="alpha">Alpha value</param>
        /// <returns><see cref="Color"/></returns>
        public static Color Get(float value, bool alpha = false) 
            => alpha ? new Color(value, value, value, value) : new Color(value, value, value);

        /// <summary>
        /// Get RGBA color
        /// </summary>
        /// <param name="r">Red value</param>
        /// <param name="g">Green value</param>
        /// <param name="b">Blue value</param>
        /// <param name="alpha">Alpha value</param>
        /// <returns><see cref="Color"/></returns>
        public static Color Get(float r, float g, float b, float alpha = 1)
            => new Color(r, g, b, alpha);

        /// <summary>
        /// Get value from hexadecimal value
        /// </summary>
        /// <param name="hex">Hexadecimal value</param>
        /// <returns><see cref="Color"/></returns>
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
