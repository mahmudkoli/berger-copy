using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BergerMsfaApi.Core;
using BergerMsfaApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace BergerMsfaApi.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }
        public static IEnumerable<Exception> GetAllExceptions(this Exception exception)
        {
            yield return exception;

            if (exception is AggregateException)
            {
                var aggrEx = exception as AggregateException;
                foreach (Exception innerEx in aggrEx.InnerExceptions.SelectMany(e => e.GetAllExceptions()))
                {
                    yield return innerEx;
                }
            }
            else if (exception.InnerException != null)
            {
                foreach (Exception innerEx in exception.InnerException.GetAllExceptions())
                {
                    yield return innerEx;
                }
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }
        public static string GetAllMessages(this Exception exception)
        {
            var messages = exception
                .FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message).ToList();
            return string.Join(Environment.NewLine, messages);
        }

        public static IEnumerable<ApplicationError> GetAll(this Exception exception)
        {
            var dateTime = DateTime.Now;
            var exceptions = exception.FromHierarchy(ex => ex.InnerException);

            var list = from ex in exceptions
                       let st = new StackTrace(ex, true)
                       let frame = st.GetFrame(st.FrameCount - 1)
                       let declaringType = frame != null ? frame.GetMethod() != null ? frame.GetMethod().DeclaringType : null : null
                       select new ApplicationError
                       {
                           Id = Guid.NewGuid().ToString().ToLower(),
                           ErrorTime = dateTime,
                           FileName = frame != null ? Path.GetFileName(frame.GetFileName()) : null,
                           MethodName = frame != null ? frame.GetMethod() != null ? frame.GetMethod().Name : null : null,
                           LineNumber = frame != null ? frame.GetFileLineNumber() : 0,
                           Message = ex.Message,
                           ColumnNumber = frame != null ? frame.GetFileColumnNumber() : 0,
                           EntityName = declaringType != null ? declaringType.Name : frame != null ? frame.GetFileName() : null,
                           EntityFullName = declaringType != null ? declaringType.FullName : frame != null ? frame.GetFileName() : null,
                           StackTrace = ex.StackTrace,
                       };

            return list;
        }
        public static void ToTextFileLog(this Exception ex)
        {
            var startupPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            ToTextFileLog(ex, startupPath);
        }

        public static void ToTextFileLog(this Exception ex, string startupPath, string folderName = "Logs", string fileName = "ErrorLog", string extention = ".txt")
        {
            string message = string.Empty;
            var toDay = DateTime.Now.ToString("yyyy-MM-dd");
            var filePath = startupPath + "\\" + folderName + "\\" + fileName + "_" + toDay + extention;
            var context = HttpHelper.HttpContext;
            var userId = AppIdentity.AppUser.UserId;
            var userName = AppIdentity.AppUser.UserName;

            foreach (var item in ex.GetAll())
            {

                var msg = "--------------------------------------------------" + Environment.NewLine;

                msg += "Date: " + toDay + Environment.NewLine
                        + "Time: " + DateTime.Now.ToString("HH:mm:ss") + Environment.NewLine
                        + "File Name: " + item.FileName + Environment.NewLine
                        + "Entity Name: " + item.EntityFullName + Environment.NewLine
                        + "Method Name: " + item.MethodName + Environment.NewLine
                        + "Line Number: " + item.LineNumber + Environment.NewLine
                        + "Column Number: " + item.ColumnNumber + Environment.NewLine
                        + "Message : " + item.Message + Environment.NewLine
                        + "Request------------------------- : " + Environment.NewLine +
                            $"User Id: {userId} {Environment.NewLine}" +
                            $"User Name: {userName} {Environment.NewLine}" +
                            $"Client IP: {context.Connection.RemoteIpAddress} {Environment.NewLine}" +
                            $"URL: {context.Request.Host} {context.Request.Path} {Environment.NewLine}" +
                            $"Path: {context.Request.Path} {Environment.NewLine}" +
                            $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                            $"Request Body: {FormatRequestBody(context)} {Environment.NewLine}"
                        + "Stack Trace : " + item.StackTrace + Environment.NewLine;
                msg += "--------------------------------------------------" + Environment.NewLine;
                msg += "--------------------------------------------------" + Environment.NewLine;
                message += msg;

            }

            File.AppendAllText(filePath, message);
        }
        public static void ToWriteMessageLog(this string message, string methodName = "", string folderName = "Log", string fileName = "MessageLog.txt")
        {
            var startupPath =
    new DirectoryInfo(
            Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent
        .FullName;


            var msg = "--------------------------------------------------" + Environment.NewLine;
            msg += "Date: " + DateTime.Now.ToString("yyyy-MM-dd") + Environment.NewLine
                 + "Time: " + DateTime.Now.ToString("hh:mm:ss") + Environment.NewLine;

            if (!string.IsNullOrWhiteSpace(methodName))
            {
                msg += "Function : " + methodName + Environment.NewLine;
            }
            msg += "Message : " + message + Environment.NewLine;
            msg += "--------------------------------------------------" + Environment.NewLine;

            var filePath = startupPath + "\\" + folderName + "\\" + fileName;
            File.AppendAllText(filePath, msg);
        }


        public static void ToJsonFileLog(this Exception model)
        {
            {
                try
                {
                    var startupPath =
                        new DirectoryInfo(
                                Path.GetDirectoryName(
                                    Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))).Parent
                            .FullName;

                    ToJsonFileLog(model, startupPath);
                }
                catch (Exception ex)
                {
                    ex.ToTextFileLog();
                }
            }
        }
        public static void ToJsonFileLog(this Exception model, string startupPath, string fileName = "ErrorLog.txt")
        {
            var strLogs = JsonConvert.SerializeObject(model.GetAll());
            File.AppendAllText(startupPath + "\\" + fileName, strLogs);
        }
        public static void ToWriteLog(this Exception ex)
        {
            ex.ToTextFileLog();
        }


        public static string FormatRequestBody(HttpContext context)
        {
            var httpContext = context;
            var request = httpContext.Request;
            string body = "";
            if (request.Form.Any())
            {
                var dictionary = request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                body += JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            }
            else
            {
                request.Body.Position = 0;
                StreamReader sr = new StreamReader(request.Body);
                AsyncHelper.RunSync<string>(() => sr.ReadToEndAsync());
                request.Body.Position = 0;
            }
            return body;
        }
    }

}
