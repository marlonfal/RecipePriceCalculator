using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Logging;
using Microsoft.Extensions.Logging;

namespace Application.Parameters
{
    public class ParameterService : IParameterService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<ParameterService> _logger;

        public ParameterService(IApplicationDbContext context,
            ILogger<ParameterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public decimal GetParameter_Decimal(Domain.Enums.Parameters parameter)
        {
            try
            {
                var p = _context.Parameters.FirstOrDefault(x => x.Key == parameter.ToString());
                if (p != null)
                    return Convert.ToDecimal(p.Value, new CultureInfo("en-US"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return 0;
        }
    }
}
