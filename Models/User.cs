using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User {
            [Key]
            public int UserId { get; set; }
            // MySQL VARCHAR and TEXT types can be represeted by a string
            [Required]
            [MinLength(2)]
            public string FirstName { get; set; }
            [Required]
            [MinLength(2)]
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [MinLength(8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            // The MySQL DATETIME type can be represented by a DateTime
            public DateTime CreatedAt {get;set;}=DateTime.Now;
            public DateTime UpdatedAt {get;set;}=DateTime.Now;
            [NotMapped]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string Confirm {get;set;}

            public List<Wedding> PlannedWeddings{get;set;}
            public List<Guest> Invitees{get;set;}

        }
    }