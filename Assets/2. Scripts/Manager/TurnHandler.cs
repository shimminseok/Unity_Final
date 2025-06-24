using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

public class TurnHandler
{
    private Queue<Unit> turnQueue = new Queue<Unit>();


    public void Initialize(List<Unit> unitList)
    {
        //속도 같다.
        //에너미, 다 똑같을꺼다.
        turnQueue = new Queue<Unit>(unitList.OrderByDescending(u => u.StatManager.GetValue(StatType.Speed)));
    }

    public void StartNextTurn()
    {
    }

    public void EndTurn()
    {
    }

    public void RefillTurnQueue()
    {
    }
}