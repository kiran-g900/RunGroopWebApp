using LayerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDAL.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Club>  Clubs { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
