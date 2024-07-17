using System;
using Microsoft.SemanticKernel;

namespace CoreSK.API.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly Kernel _kernel;

        public OpenAIService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public IAsyncEnumerable<string> InvokePromptStreaming(string prompt)
        {
            return _kernel.InvokePromptStreamingAsync<string>(prompt);
        }

    }
}

