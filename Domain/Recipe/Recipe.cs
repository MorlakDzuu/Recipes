using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Recipe
{
    public class Recipe
    {
        public string Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CookingTime { get; set; }
        public int PortionsCount { get; set; }
        public string PhotoUrl { get; set; }
        public List<Stage> Stages { get; set; }
        public int UserId { get; set; }
    }
}
