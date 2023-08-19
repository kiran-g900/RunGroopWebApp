using LayerDAL.Data.Enum;
using LayerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDAL.ViewModel
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
