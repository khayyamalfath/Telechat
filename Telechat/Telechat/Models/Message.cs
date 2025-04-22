namespace Telechat.Models
{
    public class Message
    {
        /* Models a record in the Messages Table */

        public long Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
    }
}
