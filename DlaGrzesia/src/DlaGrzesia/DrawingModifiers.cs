namespace DlaGrzesia
{
    public struct DrawingModifiers
    {
        public readonly bool IncludeDebugData;
        public readonly bool IsGamePaused;

        public DrawingModifiers(bool includeDebugData, bool isGamePaused)
        {
            IncludeDebugData = includeDebugData;
            IsGamePaused = isGamePaused;
        }
    }
}
