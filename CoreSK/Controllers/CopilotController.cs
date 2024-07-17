using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreSK.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSK.API.Controllers
{
    [Route("api/[controller]")]
    public class CopilotController : Controller
    {

        private IOpenAIService _openAIService;
        public CopilotController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
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

        static async IAsyncEnumerable<string> GetResponseAsync(string question)
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

