namespace MyCoolWebServer.GameStoreApplication.ViewModels.Account
{
    using Common;
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class RegisterViewModel
    {
        [Display(Name = "E-mail")]
        [Required]
        [MaxLength(
            ValidationConstants.Account.EmailMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Full Name")]
        [MinLength(
            ValidationConstants.Account.NameMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Account.NameMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string FullName { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.Account.PasswordMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Account.PasswordMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        [Password]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
