using System;
using System.Reflection;
using Duckov.Quests;
using Duckov.Quests.Tasks;
using Duckov.UI;
using ItemStatsSystem;
using System.Collections.Generic;
using System.Linq;

namespace BetterExperience
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private readonly ItemModifier _itemModifier = new ItemModifier();
        private readonly QuestModifier _questModifier = new QuestModifier();

        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItem;
            ItemHoveringUI.onSetupMeta += OnSetupMeta;
            SceneLoader.onFinishedLoadingScene += OnFinishedLoadingScene;
        }

        void OnDisable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItem;
            ItemHoveringUI.onSetupMeta -= OnSetupMeta;
            SceneLoader.onFinishedLoadingScene -= OnFinishedLoadingScene;

            Clear();
        }

        private void Clear()
        {
            _itemModifier.ClearCache();
            _questModifier.ClearCache();
        }

        private void OnSetupItem(ItemHoveringUI ui, Item item)
        {
            if (item == null) return;
            _itemModifier.ModifyItem(item);
        }
        
        private void OnSetupMeta(ItemHoveringUI ui, ItemMetaData meta)
        {
            _itemModifier.ModifyPrefab(meta.id);
        }

        private void OnFinishedLoadingScene(SceneLoadingContext obj)
        {
            _questModifier.ModifyActiveQuests();
        }
    }

    /// <summary>
    /// 修改器基类，包含反射缓存功能
    /// </summary>
    public abstract class BaseModifier
    {
        private readonly Dictionary<Tuple<Type, string>, FieldInfo> _fieldCache = 
            new Dictionary<Tuple<Type, string>, FieldInfo>();

        protected void SetValue(object obj, string fieldName, object value)
        {
            if (obj == null) return;

            var key = Tuple.Create(obj.GetType(), fieldName);

            if (!_fieldCache.TryGetValue(key, out var fieldInfo))
            {
                fieldInfo = key.Item1.GetField(key.Item2, BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo == null) return;
                _fieldCache[key] = fieldInfo;
            }

            fieldInfo.SetValue(obj, value);
        }

        public void ClearCache()
        {
            _fieldCache.Clear();
        }
    }

    /// <summary>
    /// 物品修改器
    /// </summary>
    public class ItemModifier : BaseModifier
    {
        private readonly HashSet<int> _modifiedPrefabIDs = new HashSet<int>();

        public void ModifyItem(Item item)
        {
            SetupItem(item);
            ModifyPrefab(item.TypeID);
        }

        public void ModifyPrefab(int typeID)
        {
            if (_modifiedPrefabIDs.Contains(typeID)) return;
            
            var prefab = ItemAssetsCollection.GetPrefab(typeID);
            if (prefab == null) return;
            SetupItem(prefab);
            _modifiedPrefabIDs.Add(typeID);
        }

        private void SetupItem(Item item)
        {
            // 耐久
            item.DurabilityLoss = 0;
            // 堆叠数
            if (item.Stackable && item.MaxStackCount < 999) item.MaxStackCount = item.MaxStackCount < 99 ? 99 : 999;
            // 重量
            if (item.UnitSelfWeight != 0) SetValue(item, "weight", 0f);
        }
    }

    /// <summary>
    /// 任务修改器
    /// </summary>
    public class QuestModifier : BaseModifier
    {
        public void ModifyActiveQuests()
        {
            if (QuestManager.Instance == null) return;
            
            var activeQuests = QuestManager.Instance.ActiveQuests;
            if (activeQuests == null) return;

            foreach (var quest in activeQuests)
            {
                ModifyQuest(quest);
            }
        }

        private void ModifyQuest(Quest quest)
        {
            if (quest.Tasks == null) return;
            
            var killTasks = quest.Tasks.OfType<QuestTask_KillCount>();
            foreach (var task in killTasks)
            {
                ModifyQuestKillTask(task);
            }
        }

        private void ModifyQuestKillTask(QuestTask_KillCount task)
        {
            SetValue(task, "requireBuff", false);
            SetValue(task, "withWeapon", false);
            SetValue(task, "requireHeadShot", false);
            SetValue(task, "withoutHeadShot", false);
            SetValue(task, "requireSceneID", string.Empty);
        }
    }
}