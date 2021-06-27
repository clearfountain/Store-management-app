using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return from p in _appDbContext.Pies
                       .Include(c => c.Category)
                       select p;
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return from p in _appDbContext.Pies
                       .Include(c => c.Category)
                       where p.IsPieOfTheWeek
                       select p;
            }
        }

        public Pie GetPieById(int pieId)
        {
            return (from p in _appDbContext.Pies
                   .Include(c => c.Category)
                    where p.PieId == pieId
                    select p)
                   .FirstOrDefault();
        }
    }
}
