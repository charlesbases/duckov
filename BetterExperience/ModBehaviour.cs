using System.Collections.Generic;
using BetterExperience.Modules;
using BaseModBehaviour = Duckov.Modding.ModBehaviour;

namespace BetterExperience
{
    public class ModBehaviour : BaseModBehaviour
    {
        private readonly List<ModuleBase> _modules = new List<ModuleBase>();

        private void Awake()
        {
            RegisterModules();
        }

        private void OnEnable()
        {
            foreach (var module in _modules)
            {
                module.Enable();
            }
        }

        private void OnDisable()
        {
            foreach (var module in _modules)
            {
                module.Disable();
            }
        }

        /// <summary>
        /// 注册所有功能模块
        /// </summary>
        private void RegisterModules()
        {
            _modules.Add(new ItemModifier());

            _modules.Add(new ShopModifier());
            
            _modules.Add(new QuestModifier());
            
            _modules.Add(new LevelModifier());
        }
    }
}
