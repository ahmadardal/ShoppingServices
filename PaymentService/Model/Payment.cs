namespace PaymentService.Model
{



    public class Payment
    {
        public Guid id { get; set; }

        public string userId { get; set; }

        public double total { get; set; }

        public DateTime date { get; set; }
    }
}