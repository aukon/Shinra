using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Buddy.Coroutines;
using ff14bot.Helpers;
using ff14bot.Managers;

namespace ShinraCo
{
    public static partial class Helpers
    {
        public enum PotionIds
        {
            Str = 19886,
            Dex = 19887,
            Int = 19889,
            Mnd = 19890
        }

        public static async Task<bool> UsePotion(PotionIds itemId)
        {
            var item = InventoryManager.FilledSlots.FirstOrDefault(s => s.RawItemId == (uint)itemId);

            if (item == null || !item.CanUse())
            {
                return false;
            }

            item.UseItem();
            await Coroutine.Wait(1000, () => !item.CanUse());
            Logging.Write(Colors.Yellow, $@"[Shinra] Using >>> {item.Name}");
            return true;
        }
    }
}