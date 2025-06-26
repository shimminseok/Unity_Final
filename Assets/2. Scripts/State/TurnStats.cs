public class MoveToTargetState : ITurnState
{
    public void OnEnter(Unit unit)
    {
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }

    public ITurnState CheckTransition(Unit unit)
    {
        return null;
    }
}

public class ActState : ITurnState
{
    public void OnEnter(Unit unit)
    {
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }

    public ITurnState CheckTransition(Unit unit)
    {
        return null;
    }
}

public class ReturnState : ITurnState
{
    public void OnEnter(Unit unit)
    {
    }

    public void OnUpdate(Unit unit)
    {
    }

    public void OnExit(Unit unit)
    {
    }

    public ITurnState CheckTransition(Unit unit)
    {
        return null;
    }
}

// public class EndTurnState : ITurnState
// {
//     public void OnEnter(Unit unit)
//     {
//     }
//
//     public void OnUpdate(Unit unit)
//     {
//     }
//
//     public void OnExit(Unit unit)
//     {
//     }
//
//     public ITurnState CheckTransition(Unit unit)
//     {
//         return null;
//     }
// }