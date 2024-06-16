using PipManager.Core.Enums;

namespace PipManager.Core;

public readonly struct Response<T>(T message, ResponseMessageType type, string exception = "")
{
    public T Message { get; } = message;
    public ResponseMessageType Type { get; } = type;
    public string Exception { get; init; } = exception;
}