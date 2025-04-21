using Bird;

namespace Boost
{
    public class DirectionChanger : BoostCell
    {
        public override void ApplyBoost(Goose goose) =>
                goose.ReverseDirection();
    }
}
