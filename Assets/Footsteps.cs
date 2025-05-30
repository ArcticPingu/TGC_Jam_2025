using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public PlayerController player;
    public void Step()
    {
        player.PlayStep();
    }
}
