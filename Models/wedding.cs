using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class weddings{
        
        public weddings(){
            guests = new List<guests>();
        }

        [Key]
        public int id {get;set;}
        public int usersid{get;set;}
        public users user{get;set;}
        public string wedder_1{get;set;}
        public string wedder_2{get;set;}
        public DateTime date{get;set;}
        public string location{get;set;}
        public List<guests> guests{get;set;}
        public DateTime created_at{get;set;}
        public DateTime updated_at{get;set;}

    }
}