using UnityEngine;


public abstract class PassiveSO : SkillSo
{
    public EmotionType TriggerEmotion;

    public virtual bool CanTrigger(BaseEmotion currentEmotion)
    {
        return true;
    }

    public Unit Owner { get; private set; }

    public void Initialize(Unit owner)
    {
        Owner = owner;
    }

    public abstract void ExecutePassive();
}