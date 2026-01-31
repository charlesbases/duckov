namespace BetterExperience.Modules
{
    /// <summary>
    /// 模块基类，包含启用和禁用功能
    /// </summary>
    public abstract class ModuleBase
    {
        /// <summary>
        /// 启用模块
        /// </summary>
        public abstract void Enable();

        /// <summary>
        /// 禁用模块
        /// </summary>
        public abstract void Disable();
    }
}
