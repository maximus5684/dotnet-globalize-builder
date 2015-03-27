using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace dotnet_globalize_builder
{
    /// <summary>
    /// A generic handler that produces a combined JavaScript file with all of the necessary CLDR data for the user's locale.
    /// Also loads all of the data into Globalize and initializes Globalize with the selected locale.
    /// </summary>
    public class GlobalizeBuilder : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //Change these to suit your environment.
            string mainPath = context.Server.MapPath("~/Scripts/json-cldr/main/");
            string suppPath = context.Server.MapPath("~/Scripts/json-cldr/supplemental/");

            context.Response.ContentType = "application/javascript";
            
            //Client-side cahcing help.
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));
            context.Response.Cache.SetMaxAge(new TimeSpan(0, 30, 0));
            context.Response.Cache.SetRevalidation(HttpCacheRevalidation.None);
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.AddHeader("Last-Modified", new FileInfo(Directory.GetCurrentDirectory() + "GlobalizeBuilder.ashx").LastWriteTime.ToLongDateString());

            //Attempts to select the user's preferred language/locale based on UA locale.
            //Defaults to US English if none is found.
            string lang = "en";

            if (HttpContext.Current.Request.UserLanguages.Length > 0)
            {
                string prefLocale = HttpContext.Current.Request.UserLanguages[0];

                try
                {
                    DirectoryInfo mainDI = new DirectoryInfo(mainPath);
                    IEnumerable<DirectoryInfo> langDirs = from dirs in mainDI.EnumerateDirectories() select dirs;

                    if (langDirs.Any(dir => dir.Name == prefLocale) == true)
                    {
                        //Their preferred language is suppored by the cldr dataset.
                        lang = prefLocale;
                    }
                    else if (prefLocale.Length > 2 && langDirs.Any(dir => dir.Name == prefLocale.Substring(0, 2)))
                    {
                        //Their preferred locale is supported by the cldr dataset.
                        lang = prefLocale.Substring(0, 2);
                    }
                }
                catch
                {
                    context.Response.Write("alert('An error was encountered while enumerating the list of supported languages.");
                }
            }

            context.Response.Write("var gCurrencies = " + File.ReadAllText(mainPath + lang + "/currencies.json"));
            context.Response.Write("var gCalendar = " + File.ReadAllText(mainPath + lang + "/ca-gregorian.json"));
            context.Response.Write("var gTimeZoneNames = " + File.ReadAllText(mainPath + lang + "/timeZoneNames.json"));
            context.Response.Write("var gNumbers = " + File.ReadAllText(mainPath + lang + "/numbers.json"));
            context.Response.Write("var gRelativeTimes = " + File.ReadAllText(mainPath + lang + "/dateFields.json"));
            context.Response.Write("var gCurrencyData = " + File.ReadAllText(suppPath + "currencyData.json"));
            context.Response.Write("var gLikelySubtags = " + File.ReadAllText(suppPath + "likelySubtags.json"));
            context.Response.Write("var gNumberingSystems = " + File.ReadAllText(suppPath + "numberingSystems.json"));
            context.Response.Write("var gTimeData = " + File.ReadAllText(suppPath + "timeData.json"));
            context.Response.Write("var gWeekData = " + File.ReadAllText(suppPath + "weekData.json"));
            context.Response.Write("var gPlurals = " + File.ReadAllText(suppPath + "plurals.json"));
            context.Response.Write("var gOrdinals = " + File.ReadAllText(suppPath + "ordinals.json"));
            context.Response.Write("Globalize.load(gCurrencies, gCalendar, gTimeZoneNames, gNumbers, gRelativeTimes, ");
            context.Response.Write("gCurrencyData, gLikelySubtags, gNumberingSystems, gTimeData, gWeekData, gPlurals, gOrdinals);");
            context.Response.Write("Globalize.locale(\"" + lang + "\");");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}