using System;
namespace CoreSK.API.Services
{
    public interface IOpenAIService
    {
        IAsyncEnumerable<string> InvokePromptStreaming(string prompt);
    }

}

