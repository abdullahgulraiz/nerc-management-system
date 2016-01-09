using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NERCManagementSystem.Models
{
    public class CreateOrganizerViewModel
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Display(Name = "Registration Number")]
        public string RegNo { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, Display(Name = "Type")]
        public int OrganizerTypeID { get; set; }
        [Required, Display(Name = "Task")]
        public int OrganizerTaskID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class EditOrganizerViewModel
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Display(Name = "Registration Number")]
        public string RegNo { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, Display(Name = "Type")]
        public int OrganizerTypeID { get; set; }
        [Required, Display(Name = "Task")]
        public int OrganizerTaskID { get; set; }
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class InstitutionTeamsViewModel
    {
        public string Name { get; set; }
        public City City { get; set; }
        public ICollection<Team> Teams { get; set; }
    }

    public class InstitutionMembersVM
    {
        public string Name { get; set; }
        public City City { get; set; }
        public ICollection<Member> Members { get; set; }
    }

    public class AddTeamVM
    {
        public class Category
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public bool isChecked { get; set; }
        }
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Display(Name="Institution")]
        public int InstitutionID { get; set; }
        public List<Category> Categories { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditTeamVM
    {
        public class Category
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public bool isChecked { get; set; }
        }
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Display(Name = "Institution")]
        public int InstitutionID { get; set; }
        public List<Category> Categories { get; set; }
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class DeleteTeamVM
    {
        public string Name { get; set; }
        public string Institution { get; set; }
        public ICollection<Category> Categories { get; set; }
        public int NumMembers { get; set; }
    }

    public class TeamMembersVM
    {
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Institution Institution { get; set; }
        public ICollection<Member> Members { get; set; }
    }

}