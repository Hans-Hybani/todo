using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class TodoList
    {   
        public int Id { get; set; }

        [Required(ErrorMessage = "Veuillez inscrire un titre, s'il vous plaît.")]
        [StringLength(200, ErrorMessage = "Le titre ne doit pas dépasser 200 caractères.")]
        public string Title { get; set; }
        
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }

        [StringLength(2000, ErrorMessage = "Les notes ne doivent pas dépasser 2000 caractères.")]
        public string Notes { get; set; }
    }
}