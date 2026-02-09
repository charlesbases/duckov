using System.Collections.Generic;
using Duckov.UI;
using ItemStatsSystem;

namespace BetterExperience.Modules
{
    public class ItemModifier : ModuleBase
    {
        private const bool EnableValue = false;
        private const bool EnableStack = true;
        private const bool EnableWeight = true;
        private const bool EnableDurability = true;

        private const int MaxStackCount = 9999;
        private const int ItemValue = 10000;

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

            ModifyValue(item);
            ModifyWeight(item);
            ModifyStackCount(item);
            ModifyDurability(item);
            
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

            ModifyValue(prefab);
            ModifyWeight(prefab);
            ModifyStackCount(prefab);
            ModifyDurability(prefab);
            
            _modifiedPrefabIDs.Add(typeID);
        }

        private void ModifyValue(Item item)
        {
            if(!EnableValue)
                return;

            if (item.CanBeSold && item.Value < ItemValue)
                item.Value = ItemValue;
        }

        private void ModifyWeight(Item item)
        {
            if (!EnableWeight)
                return;
            
            if (item.UnitSelfWeight != 0)
                _modifier.SetValue(item, "weight", 0f);
        }
        
        private void ModifyStackCount(Item item)
        {
            if (!EnableStack)
                return;
            
            if (item.Stackable)
                item.MaxStackCount = MaxStackCount;
        }
        
        private void ModifyDurability(Item item)
        {
            if (!EnableDurability)
                return;
            
            item.DurabilityLoss = 0;
        }
    }
}