using System.Net.WebSockets;
using System.Text;
using HttpTriggerWorld.wit.imports.wasi.http.v0_2_0;

namespace HttpTriggerWorld.wit.exports.wasi.http.v0_2_0;

public class IncomingHandlerImpl : HttpTriggerWorld.wit.exports.wasi.http.v0_2_0.IIncomingHandler
{
    public static void Handle(ITypes.IncomingRequest request, ITypes.ResponseOutparam responseOut)
    {
        var kv = HttpTriggerWorld.wit.imports.fermyon.spin.v2_0_0.IKeyValue.Store.Open("default");
        var history = kv.Get("history") switch {
            null => String.Empty,
            byte[] by => Encoding.UTF8.GetString(by),
        };
        var newHistory = String.Format("{0}\n{1}", history, request.PathWithQuery());
        kv.Set("history", Encoding.UTF8.GetBytes(newHistory));

        var headers = new ITypes.Fields();
        headers.Append("Content-Type", Encoding.UTF8.GetBytes("text/plain"));
        var resp = new ITypes.OutgoingResponse(headers);
        var ogbod = resp.Body();

        ITypes.ResponseOutparam.Set(responseOut, Result<ITypes.OutgoingResponse, ITypes.ErrorCode>.Ok(resp));

        var os = ogbod.Write();
        os.BlockingWriteAndFlush(Encoding.UTF8.GetBytes("Hello from C#\n"));
        os.BlockingWriteAndFlush(Encoding.UTF8.GetBytes("Previous visitors:\n"));
        os.BlockingWriteAndFlush(Encoding.UTF8.GetBytes(history));
        os.BlockingWriteAndFlush(Encoding.UTF8.GetBytes("\n"));
        os.Dispose();

        ITypes.OutgoingBody.Finish(ogbod, null); // Exception: "resource has children"
    }
}
