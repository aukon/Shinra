namespace ShinraCo.Spells.PVP
{
    public class RedMagePVP
    {
        public Spell JoltII { get; } = new Spell
        {
            Name = "Jolt II",
            ID = 9444,
            Level = 0,
            GCDType = GCDType.On,
            SpellType = SpellType.Damage,
            CastType = CastType.Target
        };
    }
}