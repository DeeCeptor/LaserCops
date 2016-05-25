using UnityEngine;
using System.Collections;

public class OnlyOnDifficulty : MonoBehaviour
{
    public GameState.Difficulty difficulty = GameState.Difficulty.Normal;
    Vector3 offset = new Vector3(-1, 1, 0);

    void Start()
    {
        switch (difficulty)
        {
            case GameState.Difficulty.Easy:
                break;
            case GameState.Difficulty.Normal:
                if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
                    Destroy(this.gameObject);
                break;
            case GameState.Difficulty.Hard:
                if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy
                    || GameState.game_state.current_difficulty == GameState.Difficulty.Normal)
                    Destroy(this.gameObject);
                break;
        }
    }


    void OnDrawGizmos()
    {
        UnityEditor.Handles.Label(this.transform.position + offset, difficulty + "");
    }
}
