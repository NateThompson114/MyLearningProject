using Eventer.Models;

namespace Eventer.Helpers;

public class RandomRequestHelper
{
    public VendorProductEnrollmentRequest Generate()
    {
        return new VendorProductEnrollmentRequest();
    }
    
    static VendorProductEnrollmentRequest GenerateRandomRequest()
        {
            var random = new Random();
            return new VendorProductEnrollmentRequest
            {
                Product = random.Next(0, 2) == 0 ? null : "Product " + random.Next(1, 1000),
                Vendors = new List<Vendor>
                {
                    new Vendor
                    {
                        BamId = random.Next(0, 2) == 0 ? null : Guid.NewGuid().ToString(),
                        Address = GenerateRandomAddress(),
                        DoingBusinessAs = random.Next(0, 2) == 0 ? null : "Business " + random.Next(1, 1000),
                        EmailAddress = random.Next(0, 2) == 0 ? null : "email@example.com",
                        ExternalId = random.Next(0, 2) == 0 ? null : "External " + random.Next(1, 1000),
                        Name = random.Next(0, 2) == 0 ? null : "Vendor " + random.Next(1, 1000),
                        PhoneNumber = random.Next(0, 2) == 0 ? null : "123-456-7890",
                        RemitToAddress = GenerateRandomRemitToAddress(),
                        TaxId = random.Next(0, 2) == 0 ? null : "TaxID " + random.Next(1, 1000),
                        AccountingVendorId = random.Next(0, 2) == 0 ? null : "Account " + random.Next(1, 1000)
                    }
                }
            };
        }

        static Address GenerateRandomAddress()
        {
            var random = new Random();
            return new Address
            {
                City = random.Next(0, 2) == 0 ? null : "City " + random.Next(1, 1000),
                CountryCode = random.Next(0, 2) == 0 ? null : "Country " + random.Next(1, 100),
                Line1 = random.Next(0, 2) == 0 ? null : "Line1 " + random.Next(1, 1000),
                Line2 = random.Next(0, 2) == 0 ? null : "Line2 " + random.Next(1, 1000),
                Line3 = random.Next(0, 2) == 0 ? null : "Line3 " + random.Next(1, 1000),
                Line4 = random.Next(0, 2) == 0 ? null : "Line4 " + random.Next(1, 1000),
                PostalCode = random.Next(0, 2) == 0 ? null : "Postal " + random.Next(1, 1000),
                State = random.Next(0, 2) == 0 ? null : "State " + random.Next(1, 100)
            };
        }

        static RemitToAddress GenerateRandomRemitToAddress()
        {
            var random = new Random();
            return new RemitToAddress
            {
                City = random.Next(0, 2) == 0 ? null : "City " + random.Next(1, 1000),
                CountryCode = random.Next(0, 2) == 0 ? null : "Country " + random.Next(1, 100),
                Line1 = random.Next(0, 2) == 0 ? null : "Line1 " + random.Next(1, 1000),
                Line2 = random.Next(0, 2) == 0 ? null : "Line2 " + random.Next(1, 1000),
                Line3 = random.Next(0, 2) == 0 ? null : "Line3 " + random.Next(1, 1000),
                Line4 = random.Next(0, 2) == 0 ? null : "Line4 " + random.Next(1, 1000),
                PostalCode = random.Next(0, 2) == 0 ? null : "Postal " + random.Next(1, 1000),
                State = random.Next(0, 2) == 0 ? null : "State " + random.Next(1, 100)
            };
        }
}