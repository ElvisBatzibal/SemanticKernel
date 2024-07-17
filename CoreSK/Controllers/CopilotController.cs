using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSK.API.Controllers
{
    [Route("api/[controller]")]
    public class CopilotController : Controller
    {


        // GET api/values/5
        //[HttpGet("{question}")]
        [HttpGet]
        public IAsyncEnumerable<string> Get([FromQuery] string question)
        {
            return GetResponseAsync(question);
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

