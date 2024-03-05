using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuestDialog2.Models
{
	public class Messages
	{
            public int Id { get; set; }
        [Display(Name = "Отзыв ")]
        public string? message { get; set; }
        [Display(Name = "Оценка ")]
        public int? mark { get; set; }
        [Display(Name = "Дата ")]
        public DateTime? Datetime { get; set; }
        [NotMapped]
        public string? date { get; set; }
        [Display(Name = "Логин user: ")]
        public Users? user { get; set; }
        [NotMapped]
        [Display(Name = "Логин userId: ")]
        public int UserId { get; set; }
    }
    }

