using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace subrip.Models
{
    public class Subtitle
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Offset obrigatório")]
        public string Offset { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Nenhum arquivo selecionado")]
        public IFormFile File { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime DateUpdate { get; set; }

        public Subtitle()
        {
        }
        public Subtitle(string time, IFormFile file)
        {
            Offset = time;
            File = file;
            DateUpdate = DateTime.Now;
        }

    }
}

