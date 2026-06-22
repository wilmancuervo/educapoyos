using EduApoyos.Domain.Common;

namespace EduApoyos.Tests.Common;

public class ResultTests
{
    [Fact]
    public void ResultGenerico_Success_ValueAccesible()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ResultGenerico_Failure_AccederValueLanzaExcepcion()
    {
        var error = new Error("Test.Error", "Mensaje de error");
        var result = Result.Failure<int>(error);

        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }

    [Fact]
    public void Result_Success_IsSuccessTrue()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Result_Failure_IsFailureTrue()
    {
        var error = new Error("Test.Error", "Mensaje");
        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }
}
