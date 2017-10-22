using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QompanyVKApp.Data;
using QompanyVKApp.Models;
using VkNet;
using VkNet.Model.RequestParams;

namespace QompanyVKApp.Controllers
{
    [Route("api/[controller]")]
    public class MeetingsController : Controller
    {
        private readonly QompanyDbContext _context;

        public MeetingsController(QompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public IEnumerable<Meeting> GetMeetings()
        {
            return _context.Meetings;
        }

        [HttpGet("[action]/{groupId}")]
        public IEnumerable<Meeting> GetMeetings([FromBody]string groupId)
        {
            return _context.Meetings.Where(g => g.Group.VKId == groupId);
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetMeeting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meeting = await _context.Meetings.SingleOrDefaultAsync(m => m.Id == id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeeting([FromRoute] int id, [FromBody] Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meeting.Id)
            {
                return BadRequest();
            }

            _context.Entry(meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
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

        // POST: api/Meetings
        [HttpPost]
        public async Task<IActionResult> PostMeeting([FromBody] Meeting meeting, string groupId, bool notifyEmployees)
        {
            meeting.Group = _context.Groups.First(g => g.VKId == groupId);

            if (notifyEmployees)
            {
                var vk = new VkApi();
                var accessToken = _context.Groups.First(g => g.VKId == groupId).AccessToken;
                if (!String.IsNullOrEmpty(accessToken))
                {
                    
                    foreach (var user in meeting.Employees)
                    {
                        vk.Authorize(new ApiAuthParams
                        {
                            AccessToken = accessToken//"40099039dc84bbba636fff7051de15181a2da99a439ae66557e5646a64f36d75faebb023eb4cc7e2d86f6"
                        });
                        var send = vk.Messages.Send(new MessagesSendParams
                        {
                            Message = $"Здравствуйте {user.FullName}!" +
                                      $" Ваc пригласили на встречу c {meeting.StartTime} до" +
                                      $"{meeting.EndTime} в переговорке {meeting.MeetingRoom.Name}",
                            UserId = Convert.ToInt64(user.VKId),
                        });
                    }
                }
                
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeeting", new { id = meeting.Id }, meeting);
        }

        // DELETE: api/Meetings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meeting = await _context.Meetings.SingleOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();

            return Ok(meeting);
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}