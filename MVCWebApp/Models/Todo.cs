using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWebApp.Models
{
    public class Todo
    {
        //dont need to annotate really if the colemn names are the same as the property names
        // but, jut for the sake of completeness
        [Column("Id")]
        public long Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }

        [Column("IsComplete")]
        public bool IsComplete { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("DateAdded")]
        public string DateAdded { get; set; }

        [Column("ImageBLOB")]
        public byte[] ImageBLOB { get; set; }

        [Column("Category")]
        public string Category { get; set; }
    }
}
