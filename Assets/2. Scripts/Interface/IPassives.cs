public interface IPassives
{
    void OnAttackRepeat();
}

/// <summary>
/// 데미지를 입었을때 발동 되는 패시브
/// </summary>
public interface IDamageReaction
{
    void OnDamageReceived();
}

public interface IEmotionStackApplier
{
    void ApplyStack(BaseEmotion emotion);
}

public interface ITurnStartTrigger
{
    void OnTurnStart(Unit unit);
}

public interface IEmotionDamageModifier
{
    float ModifyEmotionDamage(float baseDamage);
}