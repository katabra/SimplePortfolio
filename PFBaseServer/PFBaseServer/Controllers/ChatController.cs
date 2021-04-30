using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PFBaseServer.Controllers
{
    /// <summary>
    /// 간단한 GET, POST 처리 콘트롤러
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        WSChatQueue _chatQueue;

        public ChatController(WSChatQueue chatQueue)
        {
            _chatQueue = chatQueue;
        }

        [HttpGet("last")]
        public JsonResult GetLastChat()
        {
            return new JsonResult(_chatQueue.GetLastMsgs());
        }

        [HttpPost("last")]
        public JsonResult PostLastChat()
        {
            return new JsonResult(_chatQueue.GetLastMsgs());
        }
    }
}
