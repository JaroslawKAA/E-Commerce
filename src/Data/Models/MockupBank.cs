namespace Codecool.CodecoolShop.Models
{
    public class MockupBank
    {
        private readonly int _accountState = 2000;

        public bool IsPaymentSuccesfull(decimal totalAmount)
        {
            return totalAmount < _accountState;
        }
    }
}