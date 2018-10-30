using System.Collections.Generic;

namespace Library.API2.Models
{
    public abstract class LinkedResourceBaseDto
    {
        public List<LinkDto> Links { get; set; }
            = new List<LinkDto>();
    }
}