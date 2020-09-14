using System.ComponentModel.DataAnnotations;
using BergerMsfaApi.Attributes;

namespace BergerMsfaApi.Core
{
    [IgnoreEntity]
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }

    }

}
