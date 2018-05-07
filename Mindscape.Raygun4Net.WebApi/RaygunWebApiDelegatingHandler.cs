﻿using System.Linq;
using System.Net.Http;
using System.Text;

namespace Mindscape.Raygun4Net.WebApi
{
  public class RaygunWebApiDelegatingHandler : DelegatingHandler
  {
    public const string RequestBodyKey = "Raygun.RequestBody";

    protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
      // ReadAsByteArrayAsync is always readable as it calls LoadIntoBufferAsync internally.
      var bytes = await request.Content.ReadAsByteArrayAsync();
      if (bytes.Length > 0)
      {
        // Only take first 4096 bytes
        var bytesToSend = bytes.Take(4096).ToArray();
        request.Properties[RequestBodyKey] = Encoding.UTF8.GetString(bytesToSend);
      }

      return await base.SendAsync(request, cancellationToken);
    }
  }
}