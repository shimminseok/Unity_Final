using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public          int  Speed { get; protected set; }
    public abstract void StartTurn();

    public abstract void EndTurn();
}