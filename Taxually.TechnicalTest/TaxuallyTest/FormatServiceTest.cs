using System.Text;
using NUnit.Framework;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;

namespace VATRegisterTest.obj;

public class FormatServiceTests
{
    [Test]
    public void GenerateCsv()
    {
        var formatService = new FormatService();
        var request = new VATRegistrationRequest
        {
            CompanyName = "Test_Name",
            CompanyId = "Test_Id"
        };
        var result = formatService.GenerateCsv(request);

        var expectedCsv = "CompanyName,CompanyId\r\nTest_Name,Test_Id\r\n";
        var expectedBytes = Encoding.UTF8.GetBytes(expectedCsv);

        Assert.AreEqual(expectedBytes, result);
    }

    [Test]
    public void GenerateXm()
    {
        var formatService = new FormatService();
        var request = new VATRegistrationRequest
        {
            CompanyName = "Test_Name",
            CompanyId = "Test_Id"
        };
        var result = formatService.GenerateXml(request);

        var expectedXml =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<VATRegistrationRequest xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <CompanyName>Test_Name</CompanyName>\r\n  <CompanyId>Test_Id</CompanyId>\r\n</VATRegistrationRequest>";
        
        Assert.AreEqual(expectedXml, result);
    }
}