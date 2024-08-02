using System.Text.Json.Serialization;

namespace Eventer.Models;

// VendorProductEnrollmentRequest myDeserializedClass = JsonSerializer.Deserialize<VendorProductEnrollmentRequest>(myJsonResponse);
public class VendorProductEnrollmentRequest
{
    [JsonPropertyName("product")]
    public string Product { get; set; }

    [JsonPropertyName("vendors")]
    public List<Vendor> Vendors { get; set; }
}

public class Address
{
    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }

    [JsonPropertyName("line1")]
    public string Line1 { get; set; }

    [JsonPropertyName("line2")]
    public string Line2 { get; set; }

    [JsonPropertyName("line3")]
    public string Line3 { get; set; }

    [JsonPropertyName("line4")]
    public string Line4 { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}

public class RemitToAddress
{
    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("countryCode")]
    public string CountryCode { get; set; }

    [JsonPropertyName("line1")]
    public string Line1 { get; set; }

    [JsonPropertyName("line2")]
    public string Line2 { get; set; }

    [JsonPropertyName("line3")]
    public string Line3 { get; set; }

    [JsonPropertyName("line4")]
    public string Line4 { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}

public class Vendor
{
    [JsonPropertyName("bamId")]
    public string BamId { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("doingBusinessAs")]
    public string DoingBusinessAs { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("remitToAddress")]
    public RemitToAddress RemitToAddress { get; set; }

    [JsonPropertyName("taxId")]
    public string TaxId { get; set; }

    [JsonPropertyName("accountingVendorId")]
    public string AccountingVendorId { get; set; }
}

