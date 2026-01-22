using Moq;
using OrderProcessing;

namespace OrderProcessing.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task ProcessOrderAsync_ValidId_CallsRepositoryAndNotification()
    {
        var mockRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger>();
        var mockValidator = new Mock<IOrderValidator>();
        var mockNotification = new Mock<INotificationService>();

        mockValidator.Setup(v => v.IsValid(It.IsAny<int>())).Returns(true);
        mockRepository.Setup(r => r.GetOrderAsync(1)).ReturnsAsync("Laptop");

        var service = new OrderService(
            mockRepository.Object,
            mockLogger.Object,
            mockValidator.Object,
            mockNotification.Object);

        await service.ProcessOrderAsync(1);

        mockValidator.Verify(v => v.IsValid(1), Times.Once);
        mockRepository.Verify(r => r.GetOrderAsync(1), Times.Once);
        mockNotification.Verify(n => n.SendAsync(It.IsAny<string>()), Times.Once);
        mockLogger.Verify(l => l.LogInfo(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ProcessOrderAsync_InvalidId_DoesNotCallRepository()
    {
        var mockRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger>();
        var mockValidator = new Mock<IOrderValidator>();
        var mockNotification = new Mock<INotificationService>();

        mockValidator.Setup(v => v.IsValid(It.IsAny<int>())).Returns(false);

        var service = new OrderService(
            mockRepository.Object,
            mockLogger.Object,
            mockValidator.Object,
            mockNotification.Object);

        await service.ProcessOrderAsync(-1);

        mockValidator.Verify(v => v.IsValid(-1), Times.Once);
        mockRepository.Verify(r => r.GetOrderAsync(It.IsAny<int>()), Times.Never);
        mockNotification.Verify(n => n.SendAsync(It.IsAny<string>()), Times.Never);
        mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task ProcessOrderAsync_OrderNotFound_CatchesExceptionAndLogs()
    {
        var mockRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger>();
        var mockValidator = new Mock<IOrderValidator>();
        var mockNotification = new Mock<INotificationService>();

        mockValidator.Setup(v => v.IsValid(It.IsAny<int>())).Returns(true);
        mockRepository.Setup(r => r.GetOrderAsync(999))
            .ThrowsAsync(new KeyNotFoundException("Order not found."));

        var service = new OrderService(
            mockRepository.Object,
            mockLogger.Object,
            mockValidator.Object,
            mockNotification.Object);

        await service.ProcessOrderAsync(999);

        mockRepository.Verify(r => r.GetOrderAsync(999), Times.Once);
        mockLogger.Verify(l => l.LogError(
            It.Is<string>(s => s.Contains("999")),
            It.Is<Exception>(e => e is KeyNotFoundException)), Times.Once);
        mockNotification.Verify(n => n.SendAsync(It.IsAny<string>()), Times.Never);
    }
}
