namespace ShinraCo.Spells.Main
{
    public class BasicClassSpells
    {
        public ArcanistSpells Arcanist { get; } = new ArcanistSpells();
        public ArcherSpells Archer { get; } = new ArcherSpells();
        public ConjurerSpells Conjurer { get; } = new ConjurerSpells();
        public GladiatorSpells Gladiator { get; set; } = new GladiatorSpells();
        public LancerSpells Lancer { get; } = new LancerSpells();
        public MarauderSpells Marauder { get; } = new MarauderSpells();
    }
}