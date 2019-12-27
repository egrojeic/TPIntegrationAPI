using IntegrationAPIs.Models;
using System.Collections.Generic;

namespace IntegrationAPIs.Models
{
    public class SubstitutionsResponse
    {
        public List<Substitutions> Substitutions { get; set; }
        public MsgResponse Response { get; set; }
    }
}