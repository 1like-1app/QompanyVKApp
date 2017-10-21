using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QompanyVKApp;
using QompanyVKApp.Data;
using QompanyVKApp.Models;

namespace QompanyVKApp.Controllers
{

    [Route("api/[controller]")]
    public class MeetingRoomsController : Controller
    {
        private readonly QompanyDbContext _context;

        public MeetingRoomsController(QompanyDbContext context)
        {
            _context = context;
        }
        
        // GET: api/MeetingRooms
        [HttpGet("[action]")]
        public IEnumerable<MeetingRoom> GetMeetingRooms()
        {
            return _context.MeetingRooms;
        }

        // GET: api/MeetingRooms/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetMeetingRoom([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetingRoom = await _context.MeetingRooms.SingleOrDefaultAsync(m => m.Id == id);

            if (meetingRoom == null)
            {
                return NotFound();
            }

            return Ok(meetingRoom);
        }

        /// <example> 
        /// api/meetingRooms/GetSatisfyingRoom/2009-06-15T13:45:30/2012-04-09T11:59:32
        /// </example>
        [HttpGet("[action]/{startTime}/{endTime}")]
        public IEnumerable<MeetingRoom> GetSatisfyingRooms([FromRoute]DateTime startTime, DateTime endTime)
        {
            var meetingRooms = _context.MeetingRooms.Include(mr => mr.Meetings).ToList();
            var k =  meetingRooms.Where(mr => mr.Meetings.Any(x => x.EndTime < startTime || x.StartTime > endTime)).ToList();
            return k.Select(x => new MeetingRoom
            {
                Capacity = x.Capacity,
                Floor = x.Floor,
                Id = x.Id,
                Name = x.Name
            });
        }

        // PUT: api/MeetingRooms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingRoom([FromRoute] int id, [FromBody] MeetingRoom meetingRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meetingRoom.Id)
            {
                return BadRequest();
            }

            _context.Entry(meetingRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingRoomExists(id))
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

        // POST: api/MeetingRooms
        [HttpPost]
        public async Task<IActionResult> PostMeetingRoom([FromBody] MeetingRoom meetingRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MeetingRooms.Add(meetingRoom);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetingRoom", new { id = meetingRoom.Id }, meetingRoom);
        }

        // DELETE: api/MeetingRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingRoom([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meetingRoom = await _context.MeetingRooms.SingleOrDefaultAsync(m => m.Id == id);
            if (meetingRoom == null)
            {
                return NotFound();
            }

            _context.MeetingRooms.Remove(meetingRoom);
            await _context.SaveChangesAsync();

            return Ok(meetingRoom);
        }

        private bool MeetingRoomExists(int id)
        {
            return _context.MeetingRooms.Any(e => e.Id == id);
        }
    }
}