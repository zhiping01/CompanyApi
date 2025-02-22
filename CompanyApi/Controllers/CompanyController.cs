﻿using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost]
        public ActionResult<Company> Create(CreateCompanyRequest request)
        {
            if (companies.Exists(company => company.Name.Equals(request.Name)))
            {
                return BadRequest();
            }
            Company companyCreated = new Company(request.Name);
            companies.Add(companyCreated);
            return StatusCode(StatusCodes.Status201Created, companyCreated);
        }

        [HttpGet]
        public ActionResult<Company[]> Get()
        {
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public ActionResult<Company> GetById(string Id)
        {
            Company company = companies.Find(company => company.Id == Id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }
        [HttpDelete]
        public void ClearData()
        { 
            companies.Clear();
        }

        [HttpGet]
        [Route("getRange")]
        public ActionResult<List<Company>> GetByPageSizeAndPageIndex([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            List<Company> getCompanies = companies.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Ok(getCompanies);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateById(string Id, CreateCompanyRequest request)
        {
            Company company = companies.Find(company => company.Id == Id);
            if (company == null)
            {
                return NotFound();
            }
            company.Name = request.Name;
            return NoContent();
        }

        [HttpPost("{companyId}")]
        public ActionResult CreateEmployeeByCompanyId(string companyId, [FromBody] CreateEmployeeRequest request)
        {
            Company company = companies.Find(company => company.Id == companyId);
            if (company == null)
            {
                return BadRequest();
            }
            Employee createdEmployee = company.AddEmployee(request);
            return StatusCode(StatusCodes.Status201Created, createdEmployee);
        }

        [HttpDelete("{companyId}/employees/{employeeId}")]
        public ActionResult DeleteByEmpAndCompanyId(string companyId, string employeeId)
        {
            Company company = companies.Find(company => company.Id == companyId);
            if (company == null)
            {
                return BadRequest();
            }
            if(company.DeleteEmployee(employeeId) == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
