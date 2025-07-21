using UnityEngine;
using UnityEngine.Serialization;


public class SkillSo : ScriptableObject
{
    public int ID;

    [FormerlySerializedAs("PassiveIcon")]
    public Sprite SkillIcon;

    public string skillName;
    public string skillDescription;
    public JobType jobType;
}