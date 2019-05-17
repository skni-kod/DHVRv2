using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Idle,
    PrepareToStart,
    MainGame,
    Win,
    Loose,
    
}

[CreateAssetMenu]
public class GameStateVariable : ScriptableObject
{
    public GameState gameState;

}
