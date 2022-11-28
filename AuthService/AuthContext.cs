using AuthService.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthService {
    public class AuthContext: DbContext {
        
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();

    }
}