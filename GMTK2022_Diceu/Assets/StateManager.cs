using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    PlayerTurn,
    EnemyTurn,
};


public class StateManager : MonoBehaviour
{
    Dictionary<Enemy, bool> enemiesTurnDone;
    
    public State state { get; private set; }
    private State stateLastFrame;

    public static StateManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;

        stateLastFrame = State.PlayerTurn;
        state = State.PlayerTurn;

        ResetEnemies();

        Pathfinding.UpdateBlocks();
    }

    private void Update()
    {

        stateLastFrame = state;
    }

    public void ResetEnemies()
    {
        enemiesTurnDone = new Dictionary<Enemy, bool>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in enemies)
        {
            enemiesTurnDone[enemy.GetComponent<Enemy>()] = false;
        }
        
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemiesTurnDone.Remove(enemy);
    }

    public void TurnDone(Enemy enemy)
    {
        enemiesTurnDone[enemy] = true;
        // check if all enemies are done
        foreach (KeyValuePair<Enemy, bool> pair in enemiesTurnDone)
        {
            if (!pair.Value)
                return; // one was not done, abort
        }
        // all done, set next state
        state = State.PlayerTurn;
        ResetEnemies();
    }
    public void TurnDone(Player player)
    {
        if(enemiesTurnDone.Count != 0)
            state = State.EnemyTurn;
    }

    public bool IsTurnDone(Enemy enemy)
    {
        return enemiesTurnDone[enemy];
    }

    public bool WasStateChanged()
    {
        return stateLastFrame != state;
    }
}
