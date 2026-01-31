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
            _modules.Add(new ItemStack());
            _modules.Add(new ItemWeight());
            _modules.Add(new ItemDurability());

            _modules.Add(new StockShopRefresh());
            
            _modules.Add(new Quest());
        }
    }
}
