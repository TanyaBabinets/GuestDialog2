using System.ComponentModel.DataAnnotations;

namespace GuestDialog2.Models
{
	public class Users
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }

		public string? LastName { get; set; }
        [Display(Name = "Гость ")]
        public string? Login { get; set; }

		public string? Password { get; set; }

		public string? Salt { get; set; }

		public ICollection<Messages>? messages { get; set; }
	}
}
