namespace Application.Communication
{
    public class SMSRequestDTO
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Template { get; set; }
    }
}