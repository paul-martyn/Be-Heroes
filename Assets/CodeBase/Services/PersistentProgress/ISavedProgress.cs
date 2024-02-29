using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress playerProgress);
    }

    public interface ISavedProgress : ISavedProgressReader
    { 
        void UpdateProgress(PlayerProgress playerProgress);
    }
}