namespace SAMU192Core.DTO
{
    public class FCMInstanceIdDTO
    {
        public FCMInstanceIdDTO() { }
        public FCMInstanceIdDTO(string id)
        {
            this.id = id;
        }

        string id;
        public string ID { get => id; set => id = value; }
    }
}
