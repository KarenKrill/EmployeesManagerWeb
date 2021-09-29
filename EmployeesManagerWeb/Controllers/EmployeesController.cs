using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeesManagerWeb.Models;

namespace EmployeesManagerWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private static EmployeesManager employeesManager = new EmployeesManager();
        private static Progress<uint> addEmployeeProgress = null;
        private static uint addEmployeeProgressValue = 0;
        private void AddEmployeeProgress_ProgressChanged(object sender, uint e) { addEmployeeProgressValue = e; }
        public EmployeesController()
        {
            if (addEmployeeProgress == null)
            {
                addEmployeeProgress = new Progress<uint>();
                addEmployeeProgress.ProgressChanged += AddEmployeeProgress_ProgressChanged;
            }
        }
        [HttpGet]
        public Employee[] GetAll()
        {
            Employee[] employees = employeesManager.GetList();
            return employees;
        }
        [HttpPost]
        public async Task<bool> Post(Employee employee, System.Threading.CancellationToken cancelToken)
        {
            addEmployeeProgressValue = 0;
            bool res = await employeesManager.AddAsync(employee, addEmployeeProgress, cancelToken);
            return res;
        }
        [HttpGet("EmployeeAddingProgress")]
        public uint EmployeeAddingProgress(){ return addEmployeeProgressValue; }
        [HttpDelete("{id:int}")]
        public bool Delete(uint id){ return employeesManager.Remove(id); }
        [HttpGet("search/{id:int}")]
        public Employee Search(uint id){ return employeesManager.Search(id); }
    }
}
