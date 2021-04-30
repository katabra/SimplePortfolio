using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFBaseServer
{
    /// <summary>
    /// 미들웨어 등록 확장
    /// </summary>
    public static class WSExt
    {
        public static IApplicationBuilder MapWebSocketChatManager(this IApplicationBuilder app, PathString path, WSHandler handler)
        {
            return app.Map(path, x => x.UseMiddleware<WSManagerMiddle>(handler));
        }
    }
}
