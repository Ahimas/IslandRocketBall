using UnityEngine;

public enum PowerupType
{
    None,
    PushUp,
    Rockets,
    Smash
}

public class Powerup : MonoBehaviour
{
    public PowerupType powerupType;

}
