using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using QompanyVKApp.Data;
using QompanyVKApp.Models;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace QompanyVKApp.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly QompanyDbContext _context;

        public EmployeesController(QompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees;
        }

        [HttpGet("[action]/{vkId}")]
        public IEnumerable<Employee> GetEmployeesByGroupId([FromRoute] string vkId)
        {
            var vk = new VkApi();
                var accessToken = _context.Groups.First(g => g.VKId == vkId).AccessToken;
                if (String.IsNullOrEmpty(accessToken))
                    return null;
            vk.Authorize(new ApiAuthParams
            {
                AccessToken = accessToken//"40099039dc84bbba636fff7051de15181a2da99a439ae66557e5646a64f36d75faebb023eb4cc7e2d86f6"
            });
            var vkUsers = vk.Groups.GetMembers(new GroupsGetMembersParams
            {
                GroupId = vkId,
                Fields = UsersFields.Photo50,
            });
            var employees = VkUserToEmployeeConverter(vkUsers);
            var newEmployees = employees.Where(p => _context.Employees.All(l => p.VKId != l.VKId));
            var deletedEmployees = _context.Employees.Where(p => employees.All(l => p.VKId != l.VKId));
            _context.Employees.AddRange(newEmployees);
            _context.Employees.RemoveRange(deletedEmployees);
            _context.SaveChanges();
            return _context.Employees;
        }

        public IEnumerable<Employee> VkUserToEmployeeConverter(VkCollection<User> users)
        {
            return users.Select(employee => new Employee
            {
                VKId = employee.Id.ToString(),
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Photo = employee.Photo50,
                Group = _context.Groups.First(g => g.VKId == employee.Id.ToString())
            });
        }

        [HttpGet("[action]/{meetingId}")]
        public IEnumerable<Employee> GetEmployeesByMeeting([FromRoute] int meetingId)
        {
            return _context.Employees.Where(employee => _context.EmployeeMeetings
            .Any(em => em.EmployeeId == employee.Id && em.MeetingId == meetingId))
            .Select(employee => employee);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}