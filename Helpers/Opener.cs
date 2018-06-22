using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Buddy.Coroutines;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ShinraCo.Settings;
using ShinraCo.Spells;
using ShinraCo.Spells.Main;
using ShinraCo.Spells.Opener;
using Resource = ff14bot.Managers.ActionResourceManager;

namespace ShinraCo
{
    public static partial class Helpers
    {
        public static int OpenerStep;
        public static bool OpenerFinished;

        private static List<Spell> current;
        private static bool usePotion;
        private static int potionStep;
        private static HashSet<uint> potionType;
        private static DateTime resetTime;

        private static BardSpells Bard { get; } = new BardSpells();
        private static BlackMageSpells BlackMage { get; } = new BlackMageSpells();
        private static MachinistSpells Machinist { get; } = new MachinistSpells();
        private static RedMageSpells RedMage { get; } = new RedMageSpells();
        private static SummonerSpells Summoner { get; } = new SummonerSpells();

        public static async Task<bool> ExecuteOpener()
        {
            if (OpenerFinished || Me.ClassLevel < 70) return false;

            if (Shinra.Settings.CooldownMode == CooldownModes.Disabled)
            {
                AbortOpener("Please enable cooldown mode to use an opener.");
                return false;
            }

            #region GetOpener

            switch (Me.CurrentJob)
            {
                case ClassJobType.Bard:
                    current = BardOpener.List;
                    usePotion = Shinra.Settings.BardPotion;
                    potionStep = 0;
                    potionType = PotionIds.Dex;
                    break;

                case ClassJobType.BlackMage:
                    current = BlackMageOpener.List;
                    usePotion = Shinra.Settings.BlackMagePotion;
                    potionStep = 7;
                    potionType = PotionIds.Int;
                    break;

                case ClassJobType.Machinist:
                    current = MachinistOpener.List;
                    usePotion = Shinra.Settings.MachinistPotion;
                    potionStep = 0;
                    potionType = PotionIds.Dex;
                    break;

                case ClassJobType.Paladin:
                    current = PaladinOpener.List;
                    usePotion = Shinra.Settings.PaladinPotion;
                    potionStep = 8;
                    potionType = PotionIds.Str;
                    break;

                case ClassJobType.RedMage:
                    current = RedMageOpener.List;
                    usePotion = Shinra.Settings.RedMagePotion;
                    potionStep = 3;
                    potionType = PotionIds.Int;
                    break;

                case ClassJobType.Summoner:
                    current = SummonerOpener.List;
                    usePotion = Shinra.Settings.SummonerPotion;
                    potionStep = 2;
                    potionType = PotionIds.Int;
                    break;

                default:
                    current = null;
                    break;
            }

            if (current == null) return false;

            #endregion

            if (usePotion && OpenerStep == potionStep)
            {
                if (await UsePotion(potionType)) return true;
            }

            var spell = current.ElementAt(OpenerStep);
            resetTime = DateTime.Now.AddSeconds(10);

            #region Job-Specific

            switch (Me.CurrentJob)
            {
                case ClassJobType.Bard:
                    if (Resource.Bard.Repertoire == 3)
                    {
                        await Bard.PitchPerfect.Cast(null, false);
                    }
                    break;

                case ClassJobType.BlackMage:
                    if ((spell.Name == BlackMage.BlizzardIV.Name || spell.Name == BlackMage.FireIV.Name) && !Resource.BlackMage.Enochian)
                    {
                        AbortOpener("Aborted opener due to Enochian.");
                        return true;
                    }
                    break;

                case ClassJobType.Machinist:
                    if (PetManager.ActivePetType != PetType.Rook_Autoturret)
                    {
                        var castLocation = Shinra.Settings.MachinistTurretLocation == CastLocations.Self ? Me : Target;

                        if (await Machinist.RookAutoturret.Cast(castLocation, false))
                        {
                            return true;
                        }
                    }
                    if (Pet != null)
                    {
                        if (await Machinist.Hypercharge.Cast(null, false))
                        {
                            return true;
                        }
                    }
                    break;

                case ClassJobType.RedMage:
                    if (!ActionManager.HasSpell("Swiftcast"))
                    {
                        AbortOpener("Aborting opener as Swiftcast is not set.");
                        return false;
                    }
                    if (spell.Name == RedMage.Verstone.Name && !Me.HasAura("Verstone Ready"))
                    {
                        AbortOpener("Aborting opener due to cooldowns.");
                        return false;
                    }
                    if (spell.Name == RedMage.EnchantedRiposte.Name && (Resource.RedMage.WhiteMana < 80 || Resource.RedMage.BlackMana < 80))
                    {
                        AbortOpener("Aborted opener due to mana levels.");
                        return true;
                    }
                    break;

                case ClassJobType.Summoner:
                    if (!ActionManager.HasSpell("Swiftcast"))
                    {
                        AbortOpener("Aborting opener as Swiftcast is not set.");
                        return false;
                    }
                    if (PetManager.ActivePetType == PetType.Ifrit_Egi && PetManager.PetMode != PetMode.Sic)
                    {
                        if (await Coroutine.Wait(1000, () => PetManager.DoAction("Sic", Me)))
                        {
                            Logging.Write(Colors.GreenYellow, @"[Shinra] Casting >>> Sic");
                            return await Coroutine.Wait(3000, () => PetManager.PetMode == PetMode.Sic);
                        }
                    }
                    if (OpenerStep == 1)
                    {
                        if (PetManager.ActivePetType == PetType.Garuda_Egi && PetManager.PetMode == PetMode.Obey)
                        {
                            if (await Summoner.Contagion.Cast())
                            {
                                return true;
                            }
                        }
                        if (Resource.Arcanist.Aetherflow < 3 || Summoner.Aetherflow.Cooldown() > 15000)
                        {
                            AbortOpener("Aborting opener due to Aetherflow charges.");
                            return false;
                        }
                    }
                    if (spell.Name == Summoner.SummonIII.Name)
                    {
                        if (!Shinra.Settings.SummonerOpenerGaruda || PetManager.ActivePetType == PetType.Ifrit_Egi ||
                            !Me.HasAura(Summoner.Role.Swiftcast.Name))
                        {
                            OpenerStep++;
                            return true;
                        }
                    }
                    if (spell.Name == Summoner.Fester.Name && Resource.Arcanist.Aetherflow > 0)
                    {
                        if (spell.Cooldown() > 0)
                        {
                            return true;
                        }
                    }
                    break;
            }

            #endregion

            if (await spell.Cast(null, false))
            {
                Debug($"Executed opener step {OpenerStep} >>> {spell.Name}");
                OpenerStep++;
                if (spell.Name == "Swiftcast")
                {
                    await Coroutine.Wait(1000, () => Me.HasAura("Swiftcast"));
                }

                #region Job-Specific

                // Machinist
                if (spell.Name == Machinist.Flamethrower.Name)
                {
                    await Coroutine.Wait(3000, () => Me.HasAura(Machinist.Flamethrower.Name));
                    await Coroutine.Wait(5000, () => Resource.Machinist.Heat == 100 || !Me.HasAura(Machinist.Flamethrower.Name));
                }

                // Red Mage
                if (spell.Name == RedMage.Manafication.Name)
                {
                    await Coroutine.Wait(3000, () => ActionManager.CanCast(RedMage.CorpsACorps.Name, Target));
                }

                #endregion
            }
            else if (spell.Cooldown(true) > 2500 && spell.Cooldown() > 500 && !Me.IsCasting)
            {
                Debug($"Skipped opener step {OpenerStep} due to cooldown >>> {spell.Name}");
                OpenerStep++;
            }

            if (OpenerStep >= current.Count)
            {
                AbortOpener("Opener finished.");
            }
            return true;
        }

        public static void AbortOpener(string msg)
        {
            Debug(msg);
            OpenerFinished = true;
        }

        public static void ResetOpener()
        {
            if (Me.InCombat || DateTime.Now < resetTime) return;

            OpenerStep = 0;
            OpenerFinished = false;
        }
    }
}
