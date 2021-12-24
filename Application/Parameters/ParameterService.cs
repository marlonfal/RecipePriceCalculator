using System;
using System.Globalization;
using System.Linq;
using Application.Common.Interfaces;
using Application.Common.Logging;
using Domain.Entities;
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

        public Parameter Get(Domain.Enums.Parameters parameter)
        {
            try
            {
                return _context.Parameters.FirstOrDefault(x => x.Key == parameter.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ExceptionHelper.GetCurrentMethod());
            }

            return null;
        }

        public decimal Get_Decimal(Domain.Enums.Parameters parameter)
        {
            try
            {
                var p = Get(parameter);
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
