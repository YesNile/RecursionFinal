using System.Collections.Generic;
using System.Windows.Media;

namespace HanoiTower
{
    public class Helper
    {
        public int RingsCount;
        public static readonly int RingMinWidth = 240;
        public static readonly int RingHeight = 40;
        public static readonly int Difference = 20;
        public static SolidColorBrush ColorBrash(string color)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(color)!;
        }

        public static class Colors
        {
            public static readonly List<string> ColorsList = new()
                {
                    "#680BAB",
                    "#DE961B",
                    "#7B6FF7",
                    "#DE551B",
                    "#5CA3E2",
                    "#21A3C1",
                    "#1CC1B0",
                    "#1ECC7D",
                    "#A9F324",
                    "#FCFF26"
                };
        }
    }
}
