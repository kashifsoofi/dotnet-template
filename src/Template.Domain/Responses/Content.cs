namespace Template.Domain.Responses
{
    public class Content
    {
        public int Id { get; }
        public string Value { get; }

        public Content(int id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
