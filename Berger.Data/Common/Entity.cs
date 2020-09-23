using System.ComponentModel.DataAnnotations;
using Berger.Data.Attributes;

namespace Berger.Data.Common
{
    [IgnoreEntity]
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }

    }

}
