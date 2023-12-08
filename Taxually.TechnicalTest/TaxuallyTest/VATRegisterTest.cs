using NUnit.Framework;
using Moq;
using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;

namespace VATRegisterTest;

public class VATRegisterTest
{
    private Mock<TaxuallyHttpClient> httpClientMock;
    private Mock<TaxuallyQueueClient> queueClientMock;
    private Mock<IFormatService> formatServiceMock;
    private VATRegisterService service;

    [SetUp]
    public void SetUp()
    {
        httpClientMock = new Mock<TaxuallyHttpClient>();
        queueClientMock = new Mock<TaxuallyQueueClient>();
        formatServiceMock = new Mock<IFormatService>();

        service = new VATRegisterService(httpClientMock.Object, queueClientMock.Object, formatServiceMock.Object);
    }

    [Test]
    public async Task RegisterForVAT_GB()
    {
        var request = new VATRegistrationRequest { CompanyName = "Test_Name", CompanyId = "Test_Id", Country = "GB" };

        await service.RegisterForVAT(request);

        httpClientMock.Verify(x => x.PostAsync(It.IsAny<string>(), It.IsAny<VATRegistrationRequest>()), Times.Once);
    }

    [Test]
    public async Task RegisterForVAT_FR_CSV()
    {
        var request = new VATRegistrationRequest { CompanyName = "Test_Name", CompanyId = "Test_Id", Country = "FR" };

        await service.RegisterForVAT(request);

        formatServiceMock.Verify(x => x.GenerateCsv(It.IsAny<VATRegistrationRequest>()), Times.Once);
        queueClientMock.Setup(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
    }

    [Test]
    public async Task RegisterForVAT_DE_XML()
    {
        var request = new VATRegistrationRequest { CompanyName = "Test_Name", CompanyId = "Test_Id", Country = "DE" };

        await service.RegisterForVAT(request);

        formatServiceMock.Verify(x => x.GenerateXml(It.IsAny<VATRegistrationRequest>()), Times.Once);
        queueClientMock.Verify(x => x.EnqueueAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void RegisterForVAT_Invalid_Country()
    {
        var request = new VATRegistrationRequest
            { CompanyName = "Test_Name", CompanyId = "Test_Id", Country = "Invalid_Country_Name" };

        Assert.ThrowsAsync<ArgumentException>(() => service.RegisterForVAT(request));
    }
}