using Dominio.Entidade;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public string Provider { get; set; }

        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(ResourceType = typeof(Traducao.Resource), Name = "login_email_lbl")]
        [EmailAddress(ErrorMessageResourceName = "email_formato_invalido", ErrorMessageResourceType =typeof(Traducao.Resource))]
        [StringLength(120, ErrorMessageResourceName = "mensagem_erro_maximo_caracter", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Traducao.Resource), Name = "login_senha_lbl")]
        [StringLength(20, ErrorMessageResourceName = "mensagem_erro_maximo_caracter", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public virtual Cliente Cliente { get; set; }

        public int IdCliente { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "mensagem_erro_campo_obrigatorio", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [EmailAddress]
        [Display(ResourceType = typeof(Traducao.Resource), Name = "login_email_lbl")]
        public string Email { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = "ChangePassword_msg_erro_required_OldPassword", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "ChangePassword_lbl_OldPassword", ResourceType = typeof(Traducao.Resource))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "ChangePassword_msg_erro_required_NewPassword", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(100, ErrorMessageResourceName = "ChangePassword_msg_erro_length_NewPassword", MinimumLength = 6, ErrorMessageResourceType = typeof(Traducao.Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "ChangePassword_lbl_NewPassword", ResourceType = typeof(Traducao.Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ChangePassword_lbl_ConfirmPassword", ResourceType = typeof(Traducao.Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "ChangePassword_msg_inconsistencia_ConfirmPassword", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class MailerViewModel {

        [Required(ErrorMessage = "Nome não pode estar vazio")]
        [DataType(DataType.Text)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "Adicione um e-mail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite uma mensagem")]
        [DataType(DataType.Text)]
        [Display(Name = "Mensagem")]
        public string Mensagem { get; set; }

    }
}
