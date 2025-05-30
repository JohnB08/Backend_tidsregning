using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Entities.DTO;
using Backend_tidsregning.Core.Entities.MongoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Backend_tidsregning.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employees(MongoDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<List<Employee>> Get()
        {
            var filter = Builders<Employee>.Filter.Empty;
            var result = await context.Employees.FindAsync(filter);
            var list = await result.ToListAsync();
            return list;

        }
        [Route("/{id}")]
        [HttpGet]
        public async Task<Employee> Get(Guid id)
        {
            var builder = Builders<Employee>.Filter;
            var filter = builder.Eq(employee => employee.Employee_ID, id);
            var result = await context.Employees.FindAsync(filter);
            return result.First();
        }
        [HttpPost]
        public async Task Post([FromBody] CreateNewEmployeeFromBodyDTO employeeDTO)
        {

            var employee = employeeDTO.MapToNewEmployee();
            await context.Employees.InsertOneAsync(employee);
        }

    }
}
