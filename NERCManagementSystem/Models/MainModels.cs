using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NERCManagementSystem.Models
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Institution> Institutions { get; set; }
    }
    public class Institution
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CityID { get; set; }
        public virtual City City { get; set; }
    }
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ApplicationUserID { get; set; }
        public int InstitutionID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
    public class Member
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string CNIC { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string FatherCNIC { get; set; }
        public string FatherPhone { get; set; }
        public string FatherEmail { get; set; }
        public int MemberTypeID { get; set; }
        public int TeamID { get; set; }
        public virtual MemberType MemberType { get; set; }
        public virtual Team Team { get; set; }

    }
    public class MemberType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
    public class OrganizerType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Organizer> Organizers { get; set; }

    }
    public class Organizer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int OrganizerTypeID { get; set; }
        public int OrganizerTaskID { get; set; }
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual OrganizerType OrganizerType { get; set; }
        public virtual OrganizerTask OrganizerTask { get; set; }
    }
    public class OrganizerTask
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Organizer> Organizers { get; set; }
    }
}