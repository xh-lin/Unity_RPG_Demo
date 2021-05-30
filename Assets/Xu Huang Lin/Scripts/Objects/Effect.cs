using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect")]
public class Effect : ScriptableObject
{
    new public string name = "New Effect";

    [System.Serializable]
    public struct StatusEffect
    {
        public StatusEffectType effectType;
        public bool isPercentage;   // only applicable if effectType is in percentage
        public float value;
    }
    // if true, the effect will be removed once unequiped the equipment
    public bool isEquip;
    // if true, the effect will be added in every (interval) sec for (duration) sec
    public bool isContinuous;
    // if true, will undo effect once the effect has been removed, either unequip or duration is over
    // only applicable if isContinuous = false
    public bool willUndo;

    // in second, only applicable if isEquip = false, the effect will be removed after (duration) sec
    public int duration;
    // in second, only applicable if isContinuous = true, the frequency of a continuous effect
    public int interval;

    public StatusEffect[] statusEffects;
}