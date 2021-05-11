using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eazy.sms.Model;

namespace eazy.sms.Core.Helper
{
    public static class TemplateRenderer
    {
        public static async Task<string> RenderTemplateToStringAsync<TModel>(string templateName, TModel model)
        {
            templateName = templateName.Replace("/", "\\");
            var appDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\Template";

            HelperExtention.CreateDirectoryIfDoesNotExist(appDirectory);


            var filename = appDirectory + "\\" + templateName;

            HelperExtention.CreateFileIfDoesNotExist(filename);


            var content = await HelperExtention.LoadTextFile(filename);


            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));

            if (model == null) return content;

            var properties = model.GetType().GetProperties();

            content = properties.Aggregate(content,
                (current,
                    property) => current.Replace("{{" + property.Name + "}}",
                    property.GetValue(model)
                        ?.ToString()));

            content = content.Replace("\\n", Environment.NewLine);

            return content;
        }

        internal static async Task<Attachment> RenderAttachmentToAttachAsync(Attachment attachment)
        {
            var appDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\Template\\Voice";
            HelperExtention.CreateDirectoryIfDoesNotExist(appDirectory);
            var filename = appDirectory + "\\" + attachment.File;

            if (!HelperExtention.CheckIfExistFile(filename)) throw new ArgumentNullException(nameof(filename));

            return new Attachment {File = filename};
        }
    }
}