namespace SAMU192Core.DTO
{
    public class FCMTokenDTO
    {

        public FCMTokenDTO() { }

        public FCMTokenDTO(string token)
        {
            this.token = token;
        }

        string token;
        public string Token { get => token; set => token = value; }
    }
}
