using System.IdentityModel.Tokens;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Chat.api.Controllers
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = actionContext.Request.Headers.Authorization.Parameter;
            var token = tokenHandler.ReadToken(tokenString) as JwtSecurityToken;
            var matches = SessionController.Context.Sessions.Join(token.Claims, s => s.Creator, c => c.Value, (s, c) => s);
            return matches.Any();
        }
    }
}