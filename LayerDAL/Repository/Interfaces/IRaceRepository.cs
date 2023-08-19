using LayerDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDAL.Repository.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetRaces();
        Task<Race> GetRace(int id);
        Task<Race> GetRaceNoTracking(int id);
        Task<IEnumerable<Race>> GetAllRacesByCity(string city);
        bool Add(Race race);
        bool Delete(Race race);
        bool Update(Race race);
    }
}
