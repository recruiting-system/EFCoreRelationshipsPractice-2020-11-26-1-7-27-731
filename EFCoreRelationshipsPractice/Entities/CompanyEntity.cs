using EFCoreRelationshipsPractice.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreRelationshipsPractice.Entities
{
    public class CompanyEntity
    {
        public CompanyEntity()
        {
        }

        public CompanyEntity(CompanyDto companyDto)
        {
            this.Name = companyDto.Name;
            this.Profile = new ProfileEntity(companyDto.Profile);
            this.Employees = companyDto.Employees.Select(employeeDto => new EmployeeEntity(employeeDto)).ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ProfileEntity Profile { get; set; }
        public List<EmployeeEntity> Employees { get; set; }
}

    public class ProfileEntity
    {
        public ProfileEntity()
        {
        }

        public ProfileEntity(ProfileDto profileDto)
        {
            this.CertId = profileDto.CertId;
            this.RegisteredCapital = profileDto.RegisteredCapital;
        }

        public int Id { get; set; }
        public int RegisteredCapital { get; set; }
        public string CertId { get; set; }
        public CompanyEntity Company { get; set; }
        [ForeignKey("CompanyIdForeignKey")]
        public int CompanyId { get; set; }
    }

    public class EmployeeEntity
    {
        public EmployeeEntity()
        {
        }

        public EmployeeEntity(EmployeeDto employeeDto)
        {
            this.Age = employeeDto.Age;
            this.Name = employeeDto.Name;
        }

        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }

        public CompanyEntity Company { get; set; }
        [ForeignKey("CompanyIdForeignKey")]
        public int CompanyId { get; set; }
    }
}
