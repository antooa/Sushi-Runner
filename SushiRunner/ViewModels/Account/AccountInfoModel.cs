namespace SushiRunner.ViewModels.Account
{
    
    // TODO: add validation
    public class AccountInfoModel
    {
        public AccountInfoChangeModel AccountInfoChange { get; set; }
        public PasswordChangeModel PasswordChange { get; set; }
    }

    public class AccountInfoChangeModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class PasswordChangeModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }
    }
}