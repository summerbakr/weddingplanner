using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models 

{
    public class Wedding 
    {
        [Key]
        public int WeddingId{get;set;}

        [Required]
        public string WedderOne{get;set;}

        [Required]
        public string WedderTwo{get;set;}

        [Required]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date{get;set;}

        [Required]
        public string Address{get;set;}

        public DateTime CreatedAt{get;set;}=DateTime.Now;

        public DateTime UpdatedAt{get;set;}=DateTime.Now;

        public int UserId{get;set;}
        public List<Guest> EventGuests{get;set;}


        
        

    }
}