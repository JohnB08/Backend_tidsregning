using System;
using Backend_tidsregning.Core.Entities.MongoDb;
namespace Backend_tidsregning.Core.Entities.DTO;

public class CreateNewEmployeeFromBodyDTO
{
    public string EmailHash
    {
        get;set;
    }
    public string Name { get; set; }

    public Employee MapToNewEmployee()
    {
        var employee = new Employee()
        {
            Name = Name,
            EmailHash = BCrypt.Net.BCrypt.HashPassword(EmailHash)
        };
        return employee;
    }
}
