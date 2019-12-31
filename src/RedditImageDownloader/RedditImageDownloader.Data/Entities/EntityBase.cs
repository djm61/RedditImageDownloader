using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedditImageDownloader.Data.Entities
{
    public class EntityBase : IEntityBase
    {
        public EntityBase()
        {
            Active = EntityLiterals.Yes;
            Deleted = EntityLiterals.No;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(1)]
        public string Active { get; set; }
        [MaxLength(1)]
        public string Deleted { get; set; }
        public DateTime RowVersion { get; set; }

        public override string ToString()
        {
            return $"Id[{Id}], Active[{Active}], Deleted[{Deleted}], RowVersion[{RowVersion:yyyy-MM-dd hh:mm:ss}], {base.ToString()}";
        }
    }
}
