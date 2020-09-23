using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Helpers
{
    public class HttpContextStorageContainer<T> : IHttpContextStorageContainer<T> where T : class
    {

        private readonly IHttpContextAccessor _accessor;
        public string ItemKey { get; set; }

        public HttpContextStorageContainer(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        public T Get()
        {
            var current = _accessor.HttpContext;
            T objectContext = null;
            if (current.Items.ContainsKey(ItemKey))
            {
                objectContext = (T)current.Items[ItemKey];
            }
            return objectContext;
        }

        public void Clear()
        {
            var current = _accessor.HttpContext;
            if (current.Items.ContainsKey(ItemKey))
            {
                current.Items[ItemKey] = null;
            }
        }

        public void Store(T itemObj)
        {
            var current = _accessor.HttpContext;
            if (current.Items.ContainsKey(ItemKey))
            {
                current.Items[ItemKey] = itemObj;
            }
            else
            {
                current.Items.Add(ItemKey, itemObj);
            }
        }
    }
}
