using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eazy.sms.Core.Helper
{
    public class TemplateRenderer
    {
        public static async Task<string> RenderTemplateToStringAsync<TModel>(string templateName, TModel model)
        {
            templateName = templateName.Replace("/", "\\");
            var appDirectory = Directory.GetCurrentDirectory() + templateName;

            var properties = model.GetType().GetProperties();

            var content = await HelperExtention.LoadTextFile(appDirectory);
             
            content = properties.Aggregate(content,
                (current,
                    property) => current.Replace("[" + property.Name.ToLower() + "]",
                    property.GetValue(model)
                        ?.ToString()));

            content = content.Replace("\\n", Environment.NewLine);

            return content;
        }
    }
}
