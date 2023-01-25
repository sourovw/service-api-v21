using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{
    public class EmailTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
        public string Variables { get; set; }
    }
}