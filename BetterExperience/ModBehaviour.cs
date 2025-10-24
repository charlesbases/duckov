using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += onSetupItemSetStackCount;
            ItemHoveringUI.onSetupItem += onSetupItemSetDurability;
        }
        
        void OnDisable()
        {
            ItemHoveringUI.onSetupItem -= onSetupItemSetStackCount;
            ItemHoveringUI.onSetupItem -= onSetupItemSetDurability;
        }
        
        private void onSetupItemSetStackCount(ItemHoveringUI uiInstance, Item item)
        {
            if (item.Stackable && item.MaxStackCount < 999)
            {
                item.MaxStackCount = item.MaxStackCount < 99 ? 99 : 999;
            }
        }
        
        private void onSetupItemSetDurability(ItemHoveringUI uiInstance, Item item)
        {
            item.DurabilityLoss = 0;
        }
    }
}