using Berger.Data.Attributes;

namespace Berger.Data.Common
{
    [IgnoreEntity]
    public interface IEntity<T> : IBaseEntity
    {
        T Id { get; set; }

    }

}
