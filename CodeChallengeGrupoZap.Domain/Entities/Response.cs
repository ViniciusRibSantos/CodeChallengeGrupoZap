using System.Collections.Generic;

namespace CodeChallengeGrupoZap.Domain.Entities
{
    public class Response
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<Immobile> Listings { get; set; }
    }
}