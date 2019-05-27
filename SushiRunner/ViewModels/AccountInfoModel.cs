namespace SushiRunner.ViewModels
{
    public class AccountInfoModel
    {
        public AccountInfoChangeModel InfoChange { get; set; }
        public PasswordChangeModel PasswordChange { get; set; }
    }

    public class AccountInfoChangeModel
    {
        public string FullName { get; set; }
        public string DefaultAddress { get; set; }
    }

    public class PasswordChangeModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }
    }
}
