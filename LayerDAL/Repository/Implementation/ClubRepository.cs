using LayerDAL.Data;
using LayerDAL.Entities;
using LayerDAL.Repository.Interfaces;
using LayerDAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDAL.Repository.Implementation
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;
        public ClubRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<Club>> GetClubs()
        {
           return await _context.clubs.ToListAsync();
        }

        public async Task<Club> GetClub(int id)
        {
            return await _context.clubs.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetClubNoTracking(int id)
        {
            return await _context.clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Add(Club club)
        {
            _context.Add(club);            
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            _context.SaveChanges();
            return true;
        }
    }
}
