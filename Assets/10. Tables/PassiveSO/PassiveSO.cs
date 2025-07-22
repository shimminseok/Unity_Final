using UnityEngine;


public class PassiveSO : SkillSo
{
    public EmotionType TriggerEmotion;
    public virtual bool CanTrigger(BaseEmotion currentEmotion) => true;

    public Unit Owner { get; private set; }

    public void Initialize(Unit owner)
    {
        Owner = owner;
    }
}