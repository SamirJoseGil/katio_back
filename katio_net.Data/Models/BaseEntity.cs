using System;
using System.ComponentModel.DataAnnotations;

namespace katio.Data.Models
{
    public class BaseEntity<TId> where TId : struct
    {
        [Key]
        public TId Id { get; set; }
    }
}
