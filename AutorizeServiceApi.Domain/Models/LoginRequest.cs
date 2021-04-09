using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutorizeServiceApi.Domain.Models
{
    public class LoginRequest
    {
        [Required]
        [RegularExpression(@"(^[А-ЯЁ][а-яё]{2,150}$)|(^[A-z][a-z]{2,150}$)", ErrorMessage = "Некорректный формат имени")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "Запомнить?")] 
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
