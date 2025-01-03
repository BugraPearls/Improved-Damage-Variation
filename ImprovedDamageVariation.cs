using Terraria;
using Terraria.ModLoader;

namespace ImprovedDamageVariation
{
    public class ImprovedDamageVariation : Mod
    {
        public override void Load()
        {
            On_Main.DamageVar_float_int_float += ModifyDamages; //Detouring DamageVar, which is used by Vanilla to calculate the damage variation
        }
        public static int Randomizer(float numToBeRandomized)
        {
            int amount = (int)numToBeRandomized; //we want integral val
            numToBeRandomized %= 1; //non integral val

            if (numToBeRandomized < 0 && Main.rand.NextFloat() <= numToBeRandomized * -1)
            {
                amount--;
            }
            else if (Main.rand.NextFloat() <= numToBeRandomized)
            {
                amount++;
            }
            return amount;
        }
        private static int ModifyDamages(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
        {
            orig(dmg,percent, luck);
            float damageAmount = dmg * (1f + Main.rand.NextFloat(-percent, percent) * 0.01f);

            int timesToRoll = Randomizer(luck);
            if (timesToRoll > 0)
            {
                for (int i = 0; i < timesToRoll; i++)
                {
                    float luckRolledDamage = dmg * (1f + Main.rand.NextFloat(-percent, percent) * 0.01f);
                    if (luckRolledDamage > damageAmount)
                        damageAmount = luckRolledDamage;

                }
            }
            else if (timesToRoll < 0)
            {
                for (int i = 0; i < timesToRoll * -1; i++)
                {
                    float negativeLuckRolledDamage = dmg * (1f + Main.rand.NextFloat(-percent, percent) * 0.01f);
                    if (negativeLuckRolledDamage < damageAmount)
                        damageAmount = negativeLuckRolledDamage;
                }
            }
            return Randomizer(damageAmount);
        }

        ////This is Vanilla Code, DamageVar()
        //private static int ModifyDamages(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
        //{
        //    float num = dmg * (1f + (float)Main.rand.Next(-percent, percent + 1) * 0.01f);
        //    if (luck > 0f)
        //    {
        //        if (Main.rand.NextFloat() < luck)
        //        {
        //            float num2 = dmg * (1f + (float)Main.rand.Next(-percent, percent + 1) * 0.01f);
        //            if (num2 > num)
        //                num = num2;
        //        }
        //    }
        //    else if (luck < 0f && Main.rand.NextFloat() < 0f - luck)
        //    {
        //        float num3 = dmg * (1f + (float)Main.rand.Next(-percent, percent + 1) * 0.01f);
        //        if (num3 < num)
        //            num = num3;
        //    }
        //    return (int)Math.Round(num);
        //}
    }
}
