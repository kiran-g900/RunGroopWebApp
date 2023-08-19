using LayerDAL.Entities;
using LayerDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDAL.Repository.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetClubs();
        Task<Club> GetClub(int id);
        Task<Club> GetClubNoTracking(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);
        bool Add(Club club);
        bool Delete(Club club);
        bool Update(Club club);
    }
}
