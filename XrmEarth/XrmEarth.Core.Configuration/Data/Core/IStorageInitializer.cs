namespace XrmEarth.Core.Configuration.Data.Core
{
    public interface IStorageInitializer<T>
    {
        void Save(T storageObject);
        T Load();
        T Load(T storageInstance);
    }
}
