using MindHeal.Models.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace MindHeal.Models.Entities
{
    public class Client : BaseEntity
    {
        public User User { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string State { get; set; }
    }
}
