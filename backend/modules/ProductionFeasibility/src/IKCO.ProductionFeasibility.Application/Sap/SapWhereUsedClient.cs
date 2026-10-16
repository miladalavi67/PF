using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using IKCO.ProductionFeasibility.Exceptions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace IKCO.ProductionFeasibility.Sap;

/// <summary>
/// فراخوانی SOAP سرویس SAP (ZMAT_WHERE_USED). بدنه درخواست با XDocument ساخته می‌شود (مقادیر escape می‌شوند)،
/// اعتبار از تنظیمات خوانده می‌شود و خطاهای شبکه/پاسخ نامعتبر به خطای منطقی استاندارد تبدیل می‌شوند.
/// </summary>
public class SapWhereUsedClient : ISapWhereUsedClient, ITransientDependency
{
    private static readonly XNamespace Soap = "http://schemas.xmlsoap.org/soap/envelope/";
    private static readonly XNamespace Rfc = "urn:sap-com:document:sap:rfc:functions";

    private readonly HttpClient _http;
    private readonly SapOptions _options;

    public SapWhereUsedClient(HttpClient http, IOptions<SapOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<SapWhereUsedItem>> GetWhereUsedAsync(string partNo, string saloonCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.WhereUsedUrl) || string.IsNullOrWhiteSpace(_options.UserName))
            throw new BaseException(PfErrorCodes.ExternalServiceUnavailable,
                "ارتباط با SAP پیکربندی نشده است (ProductionFeasibility:Sap).");

        var envelope = new XDocument(
            new XElement(Soap + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", Soap.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "web", Rfc.NamespaceName),
                new XElement(Soap + "Header"),
                new XElement(Soap + "Body",
                    new XElement(Rfc + "ZMAT_WHERE_USED",
                        new XElement("P_IDNRK", partNo),
                        new XElement("P_SFCSALON", saloonCode),
                        new XElement("IT1")))));

        using var request = new HttpRequestMessage(HttpMethod.Post, _options.WhereUsedUrl)
        {
            Content = new StringContent(envelope.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml"),
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/soap+xml"));
        request.Headers.Add("SOAPAction", "urn:sap-com:document:sap:rfc:functions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.UserName}:{_options.Password}")));

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(Math.Max(5, _options.TimeoutSeconds)));

        string xml;
        try
        {
            using var response = await _http.SendAsync(request, timeout.Token);
            if (!response.IsSuccessStatusCode)
                throw new BaseException(PfErrorCodes.ExternalServiceUnavailable, $"سرویس SAP پاسخ ناموفق داد ({(int)response.StatusCode}).");
            xml = await response.Content.ReadAsStringAsync(timeout.Token);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new BaseException(PfErrorCodes.ExternalServiceUnavailable, "ارتباط با سرویس SAP برقرار نشد.");
        }

        try
        {
            var items = XDocument.Parse(xml).Descendants().Where(e => e.Name.LocalName == "item" && e.Parent?.Name.LocalName == "IT1");
            var result = new List<SapWhereUsedItem>();
            foreach (var item in items)
            {
                var matnr = item.Elements().FirstOrDefault(e => e.Name.LocalName == "MATNR")?.Value.Trim();
                if (string.IsNullOrEmpty(matnr))
                    continue;
                var code = matnr.Split(':')[0].Trim();
                decimal? quantity = decimal.TryParse(item.Elements().FirstOrDefault(e => e.Name.LocalName == "BMENG")?.Value.Trim(),
                    System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var q) ? q : null;
                result.Add(new SapWhereUsedItem(code, quantity));
            }

            return result;
        }
        catch (System.Xml.XmlException)
        {
            throw new BaseException(PfErrorCodes.ExternalServiceResponseInvalid, "پاسخ سرویس SAP قابل پردازش نیست.");
        }
    }
}
