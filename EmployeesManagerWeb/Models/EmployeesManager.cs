using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesManagerWeb.Models
{
    internal class UidGenerator
    {
        private uint ID;
        public UidGenerator(uint StartWith = 0){ ID = StartWith; }
        public uint GetNextUID(){ return ID++; }
    }
    public class Employee
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public uint Salary { get; set; }
    }
    /// <summary>
    /// Provides work with employee data
    /// </summary>
    public class EmployeesManager
    {
        private ConcurrentDictionary<uint, Employee> employees = new ConcurrentDictionary<uint, Employee>();
        private UidGenerator uidGenerator = new UidGenerator((uint)new Random().Next());
        public Employee[] GetList() { return employees.Values.ToArray(); }
        public bool Add(Employee employee, IProgress<uint> progress = null, System.Threading.CancellationToken cancelToken = default)
        {
            uint n = 80;
            for (uint i = 0; i < n; i++)
            {
                progress?.Report(100 * i / n);
                if(cancelToken.IsCancellationRequested) return false;
                System.Threading.Thread.Sleep(20);
            }
            employee.ID = uidGenerator.GetNextUID();
            progress?.Report(100);
            return employees.TryAdd(employee.ID, employee);
        }
        public async Task<bool> AddAsync(Employee employee, Progress<uint> progress = null, System.Threading.CancellationToken cancellationToken = default)
        {
            bool result = false;
            result = await Task.Run(() => Add(employee, progress, cancellationToken));
            return result;
        }
        public bool Remove(uint ID){ return employees.Remove(ID, out _); }
        public Employee Search(uint ID)
        {
            Employee employee = null;
            employees.TryGetValue(ID, out employee);
            return employee;
        }
    }
}
