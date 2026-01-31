using Duckov.Economy;
using Duckov.Economy.UI;

namespace BetterExperience.Modules
{
    /// <summary>
    /// 商店自动补货模块
    /// </summary>
    public class StockShopRefresh : ModuleBase
    {
        public override void Enable()
        {
            StockShop.OnAfterItemSold += OnAfterItemSold;
        }

        public override void Disable()
        {
            StockShop.OnAfterItemSold -= OnAfterItemSold;
        }

        private static void OnAfterItemSold(StockShop shop)
        {
            var instance = StockShopView.Instance;
            if (instance == null || !instance.isActiveAndEnabled)
                return;

            if (instance.Target != shop)
                return;

            var selection = instance.GetSelection();
            if (selection == null)
                return;

            var target = selection.Target;
            if (target != null)
                target.CurrentStock = target.MaxStock;
        }
    }
}
