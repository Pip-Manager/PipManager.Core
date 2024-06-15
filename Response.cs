using PipManager.Core.Enums;

namespace PipManager.Core;

public readonly struct Response<T>(T message, ResponseMessageType type)
{
    public T Message { get; } = message;
    public ResponseMessageType Type { get; } = type;
}