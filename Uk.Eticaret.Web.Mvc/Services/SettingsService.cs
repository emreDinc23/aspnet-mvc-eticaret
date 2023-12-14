using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.Persistence;

namespace Uk.Eticaret.Web.Mvc.Services
{
    public class SettingsService
    {
        private readonly AppDbContext _context;

        public SettingsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> GetSetting(string key)
        {
            var result = await _context.Settings.FirstOrDefaultAsync(e => e.Name == key);
            return result?.Value;
        }
    }
}