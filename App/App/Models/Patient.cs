using System;
using SQLite;
using System.ComponentModel.DataAnnotations;
namespace App.Models
{
    public class Patient
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage="Nome não pode estar vazio")]
        public string Name { get; set; }

        public string Cpf { get; set; }

        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{dd/MM/yyyy}")]
        public DateTime Birth { get; set; }

        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

    }

}
