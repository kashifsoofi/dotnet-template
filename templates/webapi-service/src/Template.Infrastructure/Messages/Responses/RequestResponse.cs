namespace Template.Infrastructure.Messages.Responses
{
    using System;

    public class RequestResponse
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}