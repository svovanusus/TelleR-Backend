using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelleRPlatformApi.Models;
using TelleRPlatformApi.Repositories;
using TelleRPlatformApi.Tools.UnitOfWork;

namespace TelleRPlatformApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UnitOfWork<AppDbContext> _uow;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UnitOfWork<AppDbContext> uow)
        {
            _logger = logger;
            _uow = uow;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<User> Get()
        {
            
            return _uow.GetRepository<IUserRepository>().GetAll().ToArray();
        }
    }
}
