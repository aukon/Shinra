using ShinraCo.Spells.Role;

namespace ShinraCo.Spells.Main
{
    public class ScholarSpells : ArcanistSpells
    {
        public new HealerSpells Role { get; } = new HealerSpells();
        

    }
}