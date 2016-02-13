using Nancy;

namespace Authorization.Api.REST
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            //            pipelines.RegisterCompressionCheck();
            pipelines.AfterRequest += (ctx) =>
                {
                    //                    if (ctx.Response.ContentType == "text/html")
                    //                    ctx.Response.ContentType = ctx.Response.ContentType + "; charset=utf-8";

                    //                    var response = ctx.Response;
                    //                    var contents = response.Contents;
                    //                    response.Contents = responseStream =>
                    //                        {
                    //                            using (var stream = new StreamWriter(responseStream, Encoding.UTF8))
                    //                            {
                    //                                contents(stream.BaseStream);
                    //                            }
                    //                            //                            var stream = new StreamReader(responseStream, Encoding.UTF8);
                    //                        };

                    //                    var jsonBytes = Encoding.UTF8.GetBytes();
                    //                    return new Response
                    //                    {
                    //                        ContentType = "application/json",
                    //                        Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length)
                    //                    };
                };

            //显示详细错误
            StaticConfiguration.DisableErrorTraces = false;
            //设置Json字符串最大长度
            Nancy.Json.JsonSettings.MaxJsonLength = int.MaxValue;
            base.ApplicationStartup(container, pipelines);
        }
    }
}