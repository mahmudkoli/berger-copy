namespace BergerMsfaApi.Helpers
{

    public interface IHttpContextStorageContainer<T> where T : class
    {
        string ItemKey { get; set; }

        T Get();
        void Clear();
        void Store(T itemObj);
    }
}
