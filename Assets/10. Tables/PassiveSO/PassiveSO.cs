using UnityEngine;


public abstract class PassiveSO : SkillSo
{
    public EmotionType TriggerEmotion;
    public Unit Owner { get; private set; }

    public virtual bool CanTrigger(BaseEmotion currentEmotion)
    {
        return true;
    }


    public virtual PassiveSO CloneForRuntime(Unit owner)
    {
        PassiveSO inst = Instantiate(this);
        inst.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
        inst.Owner = owner;
        return inst;
    }
}