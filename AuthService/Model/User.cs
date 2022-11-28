namespace AuthService.Model
{


    public class User
    {

        public Guid id {get; set;}
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        
    }

}