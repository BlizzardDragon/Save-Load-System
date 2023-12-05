namespace SaveLoad
{
    public interface ISaveLoader
    {
        void SaveData(IGameRepository repository);
        void LoadData(IGameRepository repository);
    }
}
