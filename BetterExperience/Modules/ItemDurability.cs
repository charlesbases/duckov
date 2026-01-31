using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience.Modules
{
    /// <summary>
    /// 物品耐久度模块
    /// </summary>
    public class ItemDurability : ModuleBase
    {
        public override void Enable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItem;
        }

        public override void Disable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItem;
        }

        private static void OnSetupItem(ItemHoveringUI ui, Item item)
        {
            if (item == null) return;
            item.DurabilityLoss = 0;
        }
    }
}
