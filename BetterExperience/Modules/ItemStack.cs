using System.Collections.Generic;
using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience.Modules
{
    /// <summary>
    /// 物品堆叠数量模块
    /// </summary>
    public class ItemStack : ModuleBase
    {
        private const int MaxStackCount = 9999;
        private readonly HashSet<int> _modifiedPrefabIDs = new HashSet<int>();

        public override void Enable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItem;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;
        }

        public override void Disable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItem;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;
            _modifiedPrefabIDs.Clear();
        }

        private void OnSetupItem(ItemHoveringUI ui, Item item)
        {
            if (item == null) return;

            ModifyStackCount(item);
            TryModifyPrefab(item.TypeID);
        }

        private void OnSetupMeta(ItemHoveringUI ui, ItemMetaData meta)
        {
            TryModifyPrefab(meta.id);
        }

        private void TryModifyPrefab(int typeID)
        {
            if (_modifiedPrefabIDs.Contains(typeID))
                return;

            var prefab = ItemAssetsCollection.GetPrefab(typeID);
            if (prefab == null) return;
            ModifyStackCount(prefab);
            _modifiedPrefabIDs.Add(typeID);
        }

        private static void ModifyStackCount(Item item)
        {
            if (item.Stackable)
                item.MaxStackCount = MaxStackCount;
        }
    }
}
