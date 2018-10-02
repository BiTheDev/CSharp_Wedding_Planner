using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class WeddingViewModel{

        [Required]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage = "Letter only")]
        public string wedder_1{get;set;}

        [Required]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage = "Letter only")]
        public string wedder_2{get;set;}

        [Required]
        public DateTime date{get;set;}

        [Required]
        public string address{get;set;}
    }
}