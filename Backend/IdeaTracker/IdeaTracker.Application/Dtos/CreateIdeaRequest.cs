using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Application.Dtos
{
    public class CreateIdeaRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string>? Tags { get; set; }
    }
}
