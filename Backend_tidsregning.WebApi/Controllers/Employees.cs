using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Entities.DTO;
using Backend_tidsregning.Core.Entities.MongoDb;
using Backend_tidsregning.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Backend_tidsregning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Employees(ICollectionService<Employee> collection, ILogger<Employees> logger) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<Employee>> Get()
        {

            var result = await collection.GetAllAsync();
            return result;

        }
        [Route("/{id}")]
        [HttpGet]
        public async Task<Employee> Get(Guid id)
        {
            logger.LogInformation(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value);
            var result = await collection.FindAsync(id);
            return result;
        }
        [HttpPost]
        public async Task Post([FromBody] CreateNewEmployeeFromBodyDTO employeeDTO)
        {
            await collection.TryAddAsync(employeeDTO.MapToNewEmployee());
        }

    }
}
