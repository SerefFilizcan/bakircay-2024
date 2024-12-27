using System;

namespace Match
{
    public static class GameEvents
    {
        public static Action<ItemData> OnItemMatched;
        public static Action OnItemsSpawned;
        public static Action OnWindSkillUsed;

    }
}