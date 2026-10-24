using System.ComponentModel.DataAnnotations;
namespace StudentDetails
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Range(1, 120)]
        public int Age { get; set; }
    }
}