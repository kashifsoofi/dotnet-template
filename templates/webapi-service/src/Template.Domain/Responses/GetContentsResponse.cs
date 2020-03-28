using System.Collections.Generic;

namespace Template.Domain.Responses
{
    public class GetContentsResponse
    {
        public ICollection<Content> Results { get; }

        public GetContentsResponse(ICollection<Content> results)
        {
            Results = results;
        }
    }
}
