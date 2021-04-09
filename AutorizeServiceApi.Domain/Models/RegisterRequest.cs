using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutorizeServiceApi.Domain.Models
{
    public class RegisterRequest
    {
        [Required]
        [Display(Name = "Имя пользвоателя"), MaxLength(256, ErrorMessage = "Максимальная длина 256 символов")]
        [RegularExpression(@"(^[А-ЯЁ][а-яё]{2,150}$)|(^[A-z][a-z]{2,150}$)", ErrorMessage = "Некорректный формат имени")]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Пароль"), DataType(DataType.Password)] 
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; }

        public string ReturnUrl { get; set; }
    }
}
