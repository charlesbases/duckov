using System;
using System.Reflection;
using Duckov.Quests;
using Duckov.Quests.Tasks;
using Duckov.UI;
using ItemStatsSystem;
using System.Collections.Generic;

namespace BetterExperience
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        // FieldInfo 缓存
        private static readonly Dictionary<Tuple<Type, string>, FieldInfo> FieldCache = 
            new Dictionary<Tuple<Type, string>, FieldInfo>();

        // 已修改预制体 ID 缓存
        private static readonly HashSet<int> ModifiedPrefabIDs = new HashSet<int>();

        void OnEnable()
        {
            ItemHoveringUI.onSetupItem += OnSetupItem;
            SceneLoader.onFinishedLoadingScene += OnFinishedLoadingScene;
        }

        void OnDisable()
        {
            ItemHoveringUI.onSetupItem -= OnSetupItem;
            SceneLoader.onFinishedLoadingScene -= OnFinishedLoadingScene;

            // 性能优化：清空静态缓存以释放内存
            FieldCache.Clear();
            ModifiedPrefabIDs.Clear();
        }

        private static void OnSetupItem(ItemHoveringUI _, Item item)
        {
            if (item == null)
                return;
            
            // 修改物体实例
            SetupItem(item);

            // 仅修改预制体一次
            if (!ModifiedPrefabIDs.Contains(item.TypeID))
            {
                SetupItem(ItemAssetsCollection.GetPrefab(item.TypeID));
                ModifiedPrefabIDs.Add(item.TypeID);
            }
        }

        private static void OnFinishedLoadingScene(SceneLoadingContext obj)
        {
            foreach (var activeQuest in QuestManager.Instance.ActiveQuests)
            {
                SetQuest(activeQuest);
            }
        }

        private static void SetupItem(Item item)
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
            
            // 重量 
            SetValue(item,"weight",0);
        }

        private static void SetQuest(Quest quest)
        {
            foreach (var task in quest.Tasks)
            {
                if (task is QuestTask_KillCount)
                {
                    SetValue(task,"requireBuff",false);
                    SetValue(task,"withWeapon", false);
                    SetValue(task,"requireHeadShot", false);
                    SetValue(task,"withoutHeadShot", false);
                    SetValue(task,"requireSceneID", "");
                }
            }
        }

        // 3. 优化后的 SetValue
        private static void SetValue(Object obj, string field, Object value)
        {
            if (obj == null) return;

            var key = Tuple.Create(obj.GetType(), field);

            // 尝试从缓存获取
            if (!FieldCache.TryGetValue(key, out var fieldInfo))
            {
                // 如果缓存中没有，执行反射查找
                fieldInfo = key.Item1.GetField(key.Item2, BindingFlags.NonPublic | BindingFlags.Instance);
                // 存入缓存
                FieldCache[key] = fieldInfo;
            }

            // 使用缓存的 FieldInfo 设置值
            fieldInfo?.SetValue(obj, value);
        }
    }
}