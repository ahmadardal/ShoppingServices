using Microsoft.EntityFrameworkCore;
using PaymentService.Model;

namespace PaymentService
{

    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

        public DbSet<Payment> Payments => Set<Payment>();
    }

}