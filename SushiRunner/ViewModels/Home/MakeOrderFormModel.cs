using System.ComponentModel.DataAnnotations;

namespace SushiRunner.ViewModels.Home
{
    public class MakeOrderFormModel
    {
        public int TotalPrice { get; set; }
        [Required] public string CustomerName { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string PaymentType { get; set; }
        [Required] public string Address { get; set; }
        public string Comment { get; set; }
    }
}
