using UnityEngine;

[CreateAssetMenu(fileName = "BoostConfig", menuName = "Configs/BoostConfig")]
public class BoostConfig : ScriptableObject
{
    public int MinNumber { get; private set; } = 0;
    public int MaxNumber { get; private set; } = 100;
    public int Percent { get; private set; } = 10;
    public int SpeedBoost { get; private set; } = 1;
    public int Time { get; private set; } = 2;
}