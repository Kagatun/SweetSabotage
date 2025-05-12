using Bird;

namespace Boost
{
    public class Accelerator : BoostCell
    {
        public override void ApplyBoost(Goose goose)
        {
            goose.ApplySpeedBoost(Config.SpeedBoost, Config.Time);
        }
    }
}