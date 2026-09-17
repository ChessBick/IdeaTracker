using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Application.Dtos
{
    public class UpdateIdeaRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string>? Tags { get; set; }
    }
}
