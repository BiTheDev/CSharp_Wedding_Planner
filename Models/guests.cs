using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WeddingPlanner.Models
{
    public class guests{

        [Key]
        public int id{get;set;}
        public int usersid{get;set;}

        public int weddingsid{get;set;}

        public users user{get;set;}
        public weddings wedding{get;set;}
    }
}