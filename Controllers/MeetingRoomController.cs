using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QompanyVKApp.Models;

namespace QompanyVKApp.Controllers
{
    [Route("api/[controller]")]
    public class MeetingRoomController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<MeetingRoom> GetRooms()
        {
            return null;
        }
    }
}