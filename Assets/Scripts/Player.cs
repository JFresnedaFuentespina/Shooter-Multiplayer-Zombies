using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum PlayerState { ALIVE, DEAD }
    public PlayerState currentState = PlayerState.ALIVE;
}
