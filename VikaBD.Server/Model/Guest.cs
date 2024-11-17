using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VikaBD.Server.Model
{
    [Table("quest", Schema = "public")]
    public class Guest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("answer")]
        public bool? Answer { get; set; }

        [Column("key")]
        public string Key { get; set; }
    }
}
