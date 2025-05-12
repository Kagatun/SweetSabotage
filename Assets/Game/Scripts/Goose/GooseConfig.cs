using UnityEngine;

[CreateAssetMenu(fileName = "GooseConfig", menuName = "Configs/GooseConfig")]
public class GooseConfig : ScriptableObject
{
    public float Stun { get; private set; } = 5;
    public int MultiplierStun { get; private set; } = 2;
    public float TimeWait { get; private set; } = 0.5f;
    public float TimeRemove { get; private set; } = 0.4f;
}