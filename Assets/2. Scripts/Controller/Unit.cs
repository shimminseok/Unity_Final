using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public StatManager StatManager { get; protected set; }

    public abstract void StartTurn();

    public abstract void EndTurn();
}