using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSK.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Tokenizers;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Text;
using System.Numerics.Tensors;
using System.Text;
using Azure.AI.OpenAI;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSK.API.Controllers
{
    [Route("api/[controller]")]
    public class CopilotController : Controller
    {

        private IOpenAIService _openAIService;
        private Microsoft.SemanticKernel.Embeddings.ITextEmbeddingGenerationService _embeddingService;

        public CopilotController(IOpenAIService openAIService, Microsoft.SemanticKernel.Embeddings.ITextEmbeddingGenerationService embeddingService)
        {
            _openAIService = openAIService;
            _embeddingService = embeddingService;
        }
        // GET api/values/5
        //[HttpGet("{question}")]
        [HttpGet]
        public  IAsyncEnumerable<string> Get([FromQuery] string question)
        {
           //return GetResponseAsync(question);
            return _openAIService.InvokePromptStreaming(question);
           
        }

        [HttpGet("secundary")]
        public  IAsyncEnumerable<string> GetSecundary([FromQuery] string question)
        {
            string PathFile = "TensorPrimitives.netcore.txt";
            //String PathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            var code = System.IO.File.ReadAllLines(PathFile);
             var prompt = new StringBuilder("Por favor responda esta pregunta: ").AppendLine(question);
            prompt.AppendLine("*** Código a utilizar para responder la pregunta: ").AppendJoin("\n", code);
        
             return _openAIService.InvokePromptStreaming(prompt.ToString());
           
        }
        [HttpGet("secundarychunk")]
        public async Task<IAsyncEnumerable<string>> GetSecundarycChunk([FromQuery] string question)
        {
            string PathFile = "TensorPrimitives.netcore.txt";
            //String PathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            var code = System.IO.File.ReadAllLines(PathFile);

            var tokenizer = await Tiktoken.CreateByModelNameAsync("gpt-4");
            var chunks = TextChunker.SplitPlainTextParagraphs([..code], 500, 100, null, text => tokenizer.CountTokens(text));


            List<(string Content, ReadOnlyMemory<float> Vector)> dbVectorial = chunks.Zip(await _embeddingService.GenerateEmbeddingsAsync(chunks)).ToList();

            var qe = await _embeddingService.GenerateEmbeddingAsync(question);

            var prompt = new StringBuilder("Por favor responda esta pregunta: ")
                .AppendLine(question)
                .AppendLine("*** Codigo: ");

            int tokensRemaining = 2000;

            foreach (var c in dbVectorial.OrderByDescending(c => TensorPrimitives.CosineSimilarity<float>(qe.Span, c.Vector.Span)))
            {
                if ((tokensRemaining -= tokenizer.CountTokens(c.Content)) < 0) break;
                prompt.AppendLine(c.Content);

            }

            return  _openAIService.InvokePromptStreaming(prompt.ToString());

        }

  

            var tokenizer = await Tiktoken.CreateByModelNameAsync("gpt-4");
            var chunks = TextChunker.SplitPlainTextParagraphs([..code], 500, 100, null, text => tokenizer.CountTokens(text));


            List<(string Content, ReadOnlyMemory<float> Vector)> dbVectorial =
           chunks.Zip(await _embeddingService.GenerateEmbeddingsAsync(chunks)).ToList();

            var qe = await _embeddingService.GenerateEmbeddingAsync(question);

            var prompt = new StringBuilder("Por favor responda esta pregunta: ")
                .AppendLine(question)
                .AppendLine("*** Codigo: ");

            int tokensRemaining = 2000;

            foreach (var c in dbVectorial.OrderByDescending(c => TensorPrimitives.CosineSimilarity<float>(qe.Span, c.Vector.Span)))
            {
                if ((tokensRemaining -= tokenizer.CountTokens(c.Content)) < 0) break;
                prompt.AppendLine(c.Content);

            }

            return  _openAIService.InvokePromptStreaming(prompt.ToString());

        }

  

        static async IAsyncEnumerable<string> GetResponseAsync(string question, string code)
        {

            foreach (string word in question.Split(' '))

            {

                await Task.Delay(250);

                yield return word + " ";

            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string question)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

