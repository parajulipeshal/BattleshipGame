namespace Logic
{
    public interface IStorage
    {
        void Save(AppData data);
        AppData Load();
    }
}
