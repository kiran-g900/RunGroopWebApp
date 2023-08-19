using LayerDAL.Data;
using LayerDAL.Entities;
using LayerDAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDAL.Repository.Implementation
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;
        public RaceRepository(ApplicationDbContext context) 
        {
            _context = context; 
        }

        public async Task<IEnumerable<Race>> GetRaces()
        {
            return await _context.races.ToListAsync();
        }

        public async Task<Race> GetRace(int id)
        {
            return await _context.races.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race> GetRaceNoTracking(int id)
        {
            return await _context.races.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await _context.races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Add(Race Race)
        {
            _context.Add(Race);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Race Race)
        {
            _context.Remove(Race);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Race Race)
        {
            _context.Update(Race);
            _context.SaveChanges();
            return true;
        }
    }
}
