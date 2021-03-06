﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Datasilk
{
    public class Page: Request
    {

        public string title = "Datasilk";
        public string description = "";
        public string headCss = "";
        public string favicon = "/images/favicon.png";
        public string scripts = "";
        public bool useTapestry = true;
        public Dictionary<string, string> Form = new Dictionary<string, string>();
        public IFormFileCollection Files;

        public Page(Core DatasilkCore) : base(DatasilkCore){}

        public virtual string Render(string[] path, string body = "", object metadata = null)
        {
            //renders HTML layout
            var scaffold = new Scaffold("/layout.html", S.Server.Scaffold);
            scaffold.Data["title"] = title;
            scaffold.Data["description"] = description;
            scaffold.Data["head-css"] = headCss;
            scaffold.Data["favicon"] = favicon;
            scaffold.Data["body"] = body;

            //add initialization script
            scaffold.Data["scripts"] = scripts;

            return scaffold.Render();
        }

        public string AccessDenied(bool htmlOutput = true, Page login = null)
        {
            if (htmlOutput == true)
            {
                if (!CheckSecurity() && login != null)
                {
                    return login.Render(new string[] { });
                }
                var scaffold = new Scaffold("/access-denied.html", S.Server.Scaffold);
                return scaffold.Render();
            }
            return "Access Denied";
        }

        public string Redirect(string url)
        {
            return "<script language=\"javascript\">window.location.href = '" + url + "';</script>";
        }

        public void AddScript(string url, string id = "")
        {
            scripts += "<script language=\"javascript\"" + (id != "" ? " id=\"" + id + "\"" : "") + " src=\"" + url + "\"></script>";
        }

        public void AddCSS(string url, string id = "")
        {
            headCss += "<link rel=\"stylesheet\" type=\"text/css\"" + (id != "" ? " id=\"" + id + "\"" : "") + " href=\"" + url + "\"></link>";
        }

        public void LoadPartial(ref Page page)
        {
            page.Files = Files;
            page.Form = Form;
        }
    }
}
