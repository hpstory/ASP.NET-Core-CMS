using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebApplication.Utils;

namespace WebApplication.Entities.OperatorManage
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }
        private string LoginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public async Task<OperatorInfos> Current(string apiToken = "")
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            OperatorInfos user = null;
            string token = string.Empty;
            switch (LoginProvider)
            {
                case "Cookie":
                    if (hca.HttpContext != null)
                    {
                        token = new CookieHelper().GetCookie(TokenName);
                    }
                    break;

                case "Session":
                    if (hca.HttpContext != null)
                    {
                        token = new SessionHelper().GetSession(TokenName);
                    }
                    break;

                case "WebApi":
                    token = apiToken;
                    break;
            }
            if (string.IsNullOrEmpty(token))
            {
                return user;
            }
            token = token.Trim('"');
            user = CacheFactory.Cache.GetCache<OperatorInfo>(token);
            if (user == null)
            {
                user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
            }
            return user;
        }
    }
}
