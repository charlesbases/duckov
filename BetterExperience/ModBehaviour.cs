using System.Reflection;
using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        void OnEnable()
        {
            // 最大堆叠
            ItemHoveringUI.onSetupItem += OnSetupItemSetStackCount;
            // 枪械耐久
            ItemHoveringUI.onSetupItem += OnSetupItemSetDurability;
        }
        
        void OnDisable()
        {
            // 最大堆叠
            ItemHoveringUI.onSetupItem -= OnSetupItemSetStackCount;
            // 枪械耐久
            ItemHoveringUI.onSetupItem -= OnSetupItemSetDurability;
        }
        
        private void OnSetupItemSetStackCount(ItemHoveringUI uiInstance, Item item)
        {
            if (item.Stackable && item.MaxStackCount < 999)
            {
                item.MaxStackCount = item.MaxStackCount < 99 ? 99 : 999;
            }
        }
        
        private void OnSetupItemSetDurability(ItemHoveringUI uiInstance, Item item)
        {
            item.DurabilityLoss = 0;
        }
    }
}