using BergerMsfaApi.Attributes;

namespace BergerMsfaApi.Core
{
    [IgnoreEntity]
    public interface IEntity<T> : IBaseEntity
    {
        T Id { get; set; }

    }

}
