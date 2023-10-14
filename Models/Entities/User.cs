﻿using Microsoft.AspNetCore.Identity;
using MindHeal.Data;
using MindHeal.Models.Entities.Enum;

namespace MindHeal.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public SuperAdmin SuperAdmin { get; set; }
        public Therapist Therapist { get; set; }
        public Client Client { get; set; }
        public Role Role { get; set; }
        public IList<UserRole> UserRoles { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
    }
} 