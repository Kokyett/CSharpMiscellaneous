using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miscellaneous.Database {
    internal class Person {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }
}
