using Bird;
using Utility;

namespace Boost
{
    public class ColorChanger : BoostCell
    {
        public override void ApplyBoost(Goose goose) =>
                goose.SetColor(ColorPalette.GetRandomActiveColor());
    }
}
