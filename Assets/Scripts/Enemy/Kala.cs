using System;
using System.Collections.Generic;

namespace Enemies
{
    public enum SkillKey
    {
        Balls,
        Geprek

    }
    public class Kala : Enemy
    {

        public Dictionary<SkillKey, float> cooldowns;

        public Dictionary<SkillKey, float> currCooldowns;


        public Kala(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            CreateSkillCooldownsDictionary();
        }

        public void CreateSkillCooldownsDictionary()
        {
            cooldowns = new Dictionary<SkillKey, float>
            {
                { SkillKey.Balls, 15 },
                { SkillKey.Geprek, 34 }
            };
            currCooldowns = new Dictionary<SkillKey, float>
            {
                { SkillKey.Balls, 15 },
                { SkillKey.Geprek, 34 }
            };
        }

    }
}