using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItem;
        }
        
        void OnDisable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItem;
        }

        private void OnSetupItem(ItemHoveringUI uiInstance, Item item)
        {
            // 修改物体
            SetupItem(item);
            // 修改预制体
            SetupItem(ItemAssetsCollection.GetPrefab(item.TypeID));
        }

        private void SetupItem(Item item)
        {
            if (item == null)
                return;
            
            // 堆叠数
            if (item.Stackable && item.MaxStackCount < 999)
            {
                item.MaxStackCount = item.MaxStackCount < 99 ? 99 : 999;
            }
            // 耐久度
            item.DurabilityLoss = 0;
        }
    }
}