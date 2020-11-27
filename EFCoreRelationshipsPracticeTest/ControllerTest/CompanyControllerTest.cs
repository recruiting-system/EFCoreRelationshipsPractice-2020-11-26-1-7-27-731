using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EFCoreRelationshipsPractice;
using EFCoreRelationshipsPractice.Dtos;
using EFCoreRelationshipsPractice.Repository;
using EFCoreRelationshipsPractice.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace EFCoreRelationshipsPracticeTest
{
    public class CompanyControllerTest : TestBase
    {
        public CompanyControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_company_employee_profile_success()
        {
            var client = GetClient();
            CompanyDto companyDto = new CompanyDto();
            companyDto.Name = "IBM";
            companyDto.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            var httpContent = JsonConvert.SerializeObject(companyDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/companies", content);

            var allCompaniesResponse = await client.GetAsync("/companies");
            var body = await allCompaniesResponse.Content.ReadAsStringAsync();

            List<CompanyDto> returnCompanies = JsonConvert.DeserializeObject<List<CompanyDto>>(body);

            Assert.Equal(1, returnCompanies.Count);
            Assert.Equal(companyDto.Employees.Count, returnCompanies[0].Employees.Count);
            Assert.Equal(companyDto.Employees[0].Age, returnCompanies[0].Employees[0].Age);
            Assert.Equal(companyDto.Employees[0].Name, returnCompanies[0].Employees[0].Name);
            Assert.Equal(companyDto.Profile.CertId, returnCompanies[0].Profile.CertId);
            Assert.Equal(companyDto.Profile.RegisteredCapital, returnCompanies[0].Profile.RegisteredCapital);

            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            CompanyDbContext context = scopedServices.GetRequiredService<CompanyDbContext>();
            Assert.Equal(1, context.Companies.ToList().Count);
            var firstCompany = await context.Companies
                .Include(companyDto => companyDto.Profile)
                .FirstOrDefaultAsync();
            Assert.Equal(companyDto.Profile.CertId, firstCompany.Profile.CertId);
        }

        [Fact]
        public async Task Should_Return_Companies_When_Get_Companies()
        {
            var client = GetClient();

            CompanyDto companyDto1 = new CompanyDto();
            companyDto1.Name = "IBM";
            companyDto1.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto1.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            CompanyDto companyDto2 = new CompanyDto();
            companyDto2.Name = "IBM";
            companyDto2.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto2.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            var httpContent1 = JsonConvert.SerializeObject(companyDto1);
            var httpContent2 = JsonConvert.SerializeObject(companyDto2);
            StringContent content1 = new StringContent(httpContent1, Encoding.UTF8, MediaTypeNames.Application.Json);
            StringContent content2 = new StringContent(httpContent2, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/companies", content1);
            await client.PostAsync("/companies", content2);

            var allCompaniesResponse = await client.GetAsync("/companies");
            var body = await allCompaniesResponse.Content.ReadAsStringAsync();

            List<CompanyDto> getReturn = JsonConvert.DeserializeObject<List<CompanyDto>>(body);

            Assert.Equal(companyDto1.Employees.Count, getReturn[0].Employees.Count);
            Assert.Equal(companyDto1.Employees[0].Age, getReturn[0].Employees[0].Age);
            Assert.Equal(companyDto1.Employees[0].Name, getReturn[0].Employees[0].Name);
            Assert.Equal(companyDto1.Profile.CertId, getReturn[0].Profile.CertId);
            Assert.Equal(companyDto1.Profile.RegisteredCapital, getReturn[0].Profile.RegisteredCapital);

            Assert.Equal(companyDto2.Employees.Count, getReturn[1].Employees.Count);
            Assert.Equal(companyDto2.Employees[0].Age, getReturn[1].Employees[0].Age);
            Assert.Equal(companyDto2.Employees[0].Name, getReturn[1].Employees[0].Name);
            Assert.Equal(companyDto2.Profile.CertId, getReturn[1].Profile.CertId);
            Assert.Equal(companyDto2.Profile.RegisteredCapital, getReturn[1].Profile.RegisteredCapital);
        }

        [Fact]
        public async Task Should_Create_Company_Successfully_Via_Service()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            CompanyDto companyDto = new CompanyDto();
            companyDto.Name = "IBM";
            companyDto.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            CompanyDbContext context = scopedServices.GetRequiredService<CompanyDbContext>();
            CompanyService companyService = new CompanyService(context);
            await companyService.AddCompany(companyDto);
            Assert.Equal(1, context.Companies.Count());
        }

        [Fact]
        public async Task Should_Delete_Company_Successfully_Via_Service()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            CompanyDto companyDto = new CompanyDto();
            companyDto.Name = "IBM";
            companyDto.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            CompanyDbContext context = scopedServices.GetRequiredService<CompanyDbContext>();
            CompanyService companyService = new CompanyService(context);
            context.Companies.RemoveRange(context.Companies);
            context.SaveChanges();
            var addReturn = await companyService.AddCompany(companyDto);
            await companyService.DeleteCompany(addReturn);
            Assert.Equal(0, context.Companies.Count());
            //Assert.Equal(0, context.Employees.Count());
            //Assert.Equal(0, context.Profiles.Count());
        }

        [Fact]
        public async Task Should_Return_Company_When_Get_Company_By_Id_Via_Service()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            CompanyDto companyDto = new CompanyDto();
            companyDto.Name = "IBM";
            companyDto.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            CompanyDbContext context = scopedServices.GetRequiredService<CompanyDbContext>();
            CompanyService companyService = new CompanyService(context);
            context.Companies.RemoveRange(context.Companies);
            context.SaveChanges();
            var addReturn = await companyService.AddCompany(companyDto);
            var getReturn = await companyService.GetById(addReturn);
            Assert.Equal(companyDto.Employees.Count, getReturn.Employees.Count);
            Assert.Equal(companyDto.Employees[0].Age, getReturn.Employees[0].Age);
            Assert.Equal(companyDto.Employees[0].Name, getReturn.Employees[0].Name);
            Assert.Equal(companyDto.Profile.CertId, getReturn.Profile.CertId);
            Assert.Equal(companyDto.Profile.RegisteredCapital, getReturn.Profile.RegisteredCapital);
        }

        [Fact]
        public async Task Should_Return_Companies_When_Get_Companies_Via_Service()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            CompanyDto companyDto1 = new CompanyDto();
            companyDto1.Name = "IBM";
            companyDto1.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto1.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            CompanyDto companyDto2 = new CompanyDto();
            companyDto2.Name = "IBM";
            companyDto2.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto2.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            CompanyDbContext context = scopedServices.GetRequiredService<CompanyDbContext>();
            CompanyService companyService = new CompanyService(context);
            context.Companies.RemoveRange(context.Companies);
            context.SaveChanges();
            var addReturn1 = await companyService.AddCompany(companyDto1);
            var addReturn2 = await companyService.AddCompany(companyDto2);
            var getReturn = await companyService.GetAll();
            Assert.Equal(companyDto1.Employees.Count, getReturn[0].Employees.Count);
            Assert.Equal(companyDto1.Employees[0].Age, getReturn[0].Employees[0].Age);
            Assert.Equal(companyDto1.Employees[0].Name, getReturn[0].Employees[0].Name);
            Assert.Equal(companyDto1.Profile.CertId, getReturn[0].Profile.CertId);
            Assert.Equal(companyDto1.Profile.RegisteredCapital, getReturn[0].Profile.RegisteredCapital);

            Assert.Equal(companyDto2.Employees.Count, getReturn[1].Employees.Count);
            Assert.Equal(companyDto2.Employees[0].Age, getReturn[1].Employees[0].Age);
            Assert.Equal(companyDto2.Employees[0].Name, getReturn[1].Employees[0].Name);
            Assert.Equal(companyDto2.Profile.CertId, getReturn[1].Profile.CertId);
            Assert.Equal(companyDto2.Profile.RegisteredCapital, getReturn[1].Profile.RegisteredCapital);
        }

        [Fact]
        public async Task Should_delete_company_and_related_employee_and_profile_success()
        {
            var client = GetClient();
            CompanyDto companyDto = new CompanyDto();
            companyDto.Name = "IBM";
            companyDto.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            var httpContent = JsonConvert.SerializeObject(companyDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await client.PostAsync("/companies", content);
            await client.DeleteAsync(response.Headers.Location);
            var allCompaniesResponse = await client.GetAsync("/companies");
            var body = await allCompaniesResponse.Content.ReadAsStringAsync();

            var returnCompanies = JsonConvert.DeserializeObject<List<CompanyDto>>(body);

            Assert.Equal(0, returnCompanies.Count);
        }

        [Fact]
        public async Task Should_create_many_companies_success()
        {
            var client = GetClient();
            CompanyDto companyDto = new CompanyDto();
            companyDto.Name = "IBM";
            companyDto.Employees = new List<EmployeeDto>()
            {
                new EmployeeDto()
                {
                    Name = "Tom",
                    Age = 19
                },
            };

            companyDto.Profile = new ProfileDto()
            {
                RegisteredCapital = 100010,
                CertId = "100",
            };

            var httpContent = JsonConvert.SerializeObject(companyDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/companies", content);
            await client.PostAsync("/companies", content);

            var allCompaniesResponse = await client.GetAsync("/companies");
            var body = await allCompaniesResponse.Content.ReadAsStringAsync();

            var returnCompanies = JsonConvert.DeserializeObject<List<CompanyDto>>(body);

            Assert.Equal(2, returnCompanies.Count);
        }
    }
}