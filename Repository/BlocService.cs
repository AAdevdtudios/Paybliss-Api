using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Models.Dto;

namespace Paybliss.Repository
{
    public class BlocService
    {
        private readonly DataContext _context;

        public BlocService(DataContext context)
        {
            _context = context;
        }

    }
}
