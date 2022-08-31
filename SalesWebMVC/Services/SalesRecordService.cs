using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;
        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from salesRecord in _context.SalesRecord select salesRecord;
            if (minDate.HasValue)
            {
                result = result.Where(salesRecord => salesRecord.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(salesRecord => salesRecord.Date <= maxDate.Value);
            }
            return await result.Include(salesRecord => salesRecord.Seller)
                .Include(salesRecord => salesRecord.Seller.Department)
                .OrderByDescending(salesRecord => salesRecord.Date).ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from salesRecord in _context.SalesRecord select salesRecord;
            if (minDate.HasValue)
            {
                result = result.Where(salesRecord => salesRecord.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(salesRecord => salesRecord.Date <= maxDate.Value);
            }
            return await result.Include(salesRecord => salesRecord.Seller)
                .Include(salesRecord => salesRecord.Seller.Department)                
                .OrderByDescending(salesRecord => salesRecord.Date)
                .GroupBy(salesRecord => salesRecord.Seller.Department).ToListAsync();
        }
    }
}
