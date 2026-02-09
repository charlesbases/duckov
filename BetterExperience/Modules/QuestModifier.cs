using Duckov.Quests;
using Duckov.Quests.Tasks;

namespace BetterExperience.Modules
{
    /// <summary>
    /// 任务限制条件模块
    /// </summary>
    public class QuestModifier : ModuleBase
    {
        private readonly Modifier _modifier = new Modifier();
        private static readonly string EmptyString = string.Empty;

        public override void Enable()
        {
            SceneLoader.onFinishedLoadingScene += OnFinishedLoadingScene;
        }

        public override void Disable()
        {
            SceneLoader.onFinishedLoadingScene -= OnFinishedLoadingScene;
            _modifier.Clear();
        }

        private void OnFinishedLoadingScene(SceneLoadingContext obj)
        {
            var questManager = QuestManager.Instance;
            if (questManager == null) return;

            var activeQuests = questManager.ActiveQuests;
            if (activeQuests == null) return;

            foreach (var quest in activeQuests)
            {
                ModifyQuest(quest);
            }
        }

        private void ModifyQuest(Quest quest)
        {
            if (quest.Tasks == null) return;

            foreach (var task in quest.Tasks)
            {
                var killTask = task as QuestTask_KillCount;
                if (killTask != null)
                    ModifyQuestKillTask(killTask);
            }
        }

        private void ModifyQuestKillTask(QuestTask_KillCount task)
        {
            _modifier.SetValue(task, "requireBuff", false);
            _modifier.SetValue(task, "withWeapon", false);
            _modifier.SetValue(task, "requireHeadShot", false);
            _modifier.SetValue(task, "withoutHeadShot", false);
            _modifier.SetValue(task, "requireSceneID", EmptyString);
        }
    }
}
