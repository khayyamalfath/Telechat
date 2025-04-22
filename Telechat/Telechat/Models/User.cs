namespace Telechat.Models
{
    public class User
    {
        /* Models a record in the Users Table */

        public long Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
