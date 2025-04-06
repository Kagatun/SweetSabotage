public class Accelerator : BoostCell
{
    public override void ApplyBoost(Goose goose)
    {
        int _speedBoost = 1;
        int _time = 2;
        goose.ApplySpeedBoost(_speedBoost, _time);
    }
}
