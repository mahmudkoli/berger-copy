using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace BergerMsfaApi.Helpers
{
    public class UserRequest : IUserRequest
    {
        public IHttpContextAccessor _httpContext;
        public UserRequest(IServiceProvider serviceProvider)
        {
            _httpContext = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
        }

        public string GetUserIp
        {
            get
            {
                try
                {
                    string strIp = _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
                    if (strIp == "::1" || strIp == "localhost")
                    {
                        strIp = "127.0.0.1";
                    }
                    return strIp;
                }
                catch (Exception)
                {
                    return "127.0.0.1";
                }
            }
        }

        public string GetUserOS
        {
            get
            {
                var osList = new Dictionary<string, string>
            {
                {"Windows NT 10.0", "Windows 10"},
                {"Windows NT 6.3", "Windows 8.1"},
                {"Windows NT 6.2", "Windows 8"},
                {"Windows NT 6.1", "Windows 7"},
                {"Windows NT 6.0", "Windows Vista"},
                {"Windows NT 5.2", "Windows Server 2003"},
                {"Windows NT 5.1", "Windows XP"},
                {"Windows NT 5.0", "Windows 2000"}
            };

                var userAgentText = _httpContext.HttpContext.Request.Headers["User-Agent"].ToString();

                //if (userAgentText == null) return HttpContext.Current.Request.Browser.Platform;
                var startPoint = userAgentText.IndexOf('(') + 1;
                var endPoint = userAgentText.IndexOf(';');

                var osVersion = userAgentText.Substring(startPoint, (endPoint - startPoint));
                var friendlyOsName = osList[osVersion];
                return friendlyOsName ?? osVersion;
            }
        }
        private string GetUserPlatform(HttpRequest request)
        {
            var ua = request.Headers["User-Agent"].ToString();

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            var platform = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            //fallback to basic platform:
            return platform + (ua.Contains("Mobile") ? " Mobile " : "Web");
        }

        private string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

        public string GetUserAgentInfo
        {
            get
            {
                try
                {
                    if (_httpContext.HttpContext == null)
                    {
                        return "CL:en-US|IP:127.0.0.1";
                    }
                    // var userBrowser = HttpContext.Current.Request.Browser;
                    var platform = _httpContext.HttpContext.Request.Headers["User-Agent"].ToString();
                    var pf = platform != null && platform.Contains("Mobile") ? " Mobile " : "Web";

                    var cultureName = _httpContext.HttpContext.Features.Get<IRequestCultureFeature>();

                    //var requestCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>();

                    var result = "OS:" + GetUserPlatform(_httpContext.HttpContext.Request) +
                                 "|PF:" + pf +
                                 "|BN:" + platform +
                                 "|BV:" + platform +
                                 "|CL:" + cultureName +
                                 //"|CL:" + CultureHelper.GetImplementedCulture(cultureName.ToString()) +
                                 "|IP:" + GetUserIp;
                    return result;
                }
                catch (Exception)
                {
                    return "CL:en-US|IP:127.0.0.1";
                }
            }
        }

        public List<Tuple<string, string>> GetAgent(string agentId)
        {

            var agent = !string.IsNullOrWhiteSpace(agentId)
                ? agentId.Split('|').Select(s => Tuple.Create(s.Split(':')[0], s.Split(':')[1])
                ).ToList()
                : new List<Tuple<string, string>> { Tuple.Create("", "") };

            return agent;
        }

    }
}
