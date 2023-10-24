using MindHeal.Models.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace MindHeal.Models.Entities
{
    public class Therapist : BaseEntity
    {
        public User User { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        /// <summary>
        public string? UserName { get; set; }
        /// </summary>
        //public Issues Issues { get; set; }
        public string? Certificate { get; set; }
        public string? Credential { get; set; }
        public string? RegNo { get; set; }
        public string? Description { get; set; }
        public string? ProfilePicture { get; set; }
        public Approval? Status { get; set; } = Approval.Pending;
        public IList<TherapistIssues> TherapistIssues { get; set; } = new List<TherapistIssues>();
        public bool IsAvalaible { get; set; } = true;
    }
}
