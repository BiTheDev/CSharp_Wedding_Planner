using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class users{

        public users(){
            UserWedding = new List<weddings>();
            UserGuest = new List<guests>();
        }
        
        [Key]
        public int id{get;set;}
        

        [Required]
        public string first_name{get; set;}

        [Required]
        public string last_name{get; set;}


        [Required]
        public string email {get; set;}

        [Required]
        public string password {get; set;}

        public DateTime created_at{get;set;}
        public DateTime updated_at{get; set;}

        public List<weddings> UserWedding{get;set;}

        public List<guests> UserGuest{get;set;}


    }
}