using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Guest
    {
        [Key]
        public int GuestId{get;set;}

        public int WeddingId{get;set;}

        public int UserId{get;set;}

        [Required]
        public int Name{get;set;}

        

        public Wedding Wedding{get;set;}

        public User User{get;set;}

        public DateTime CreatedAt{get;set;}=DateTime.Now;

        public DateTime UpdatedAt{get;set;}=DateTime.Now;



    }
}