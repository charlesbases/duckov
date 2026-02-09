namespace BetterExperience.Modules
{
    public class LevelModifier : ModuleBase
    {
        private static float _sourceEnemySpawnCountFactor;
        private const float TargetEnemySpawnCountFactor = 10.0f;

        public override void Enable()
        {
            _sourceEnemySpawnCountFactor = LevelManager.enemySpawnCountFactor;
            
            LevelManager.enemySpawnCountFactor = TargetEnemySpawnCountFactor;
        }
    
        public override void Disable()
        {
            LevelManager.enemySpawnCountFactor = _sourceEnemySpawnCountFactor;
        }
    }
}