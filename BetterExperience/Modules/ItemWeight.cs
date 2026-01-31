using System.Collections.Generic;
using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience.Modules
{
    /// <summary>
    /// 物品重量模块
    /// </summary>
    public class ItemWeight : ModuleBase
    {
        private readonly HashSet<int> _modifiedPrefabIDs = new HashSet<int>();
        private readonly Modifier _modifier = new Modifier();

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
            _modifier.Clear();
        }

        private void OnSetupItem(ItemHoveringUI ui, Item item)
        {
            if (item == null) return;

            ModifyWeight(item);
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
            ModifyWeight(prefab);
            _modifiedPrefabIDs.Add(typeID);
        }

        private void ModifyWeight(Item item)
        {
            if (item.UnitSelfWeight != 0)
                _modifier.SetValue(item, "weight", 0f);
        }
    }
}
