using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    // This route is translated to - http:localhost:5000/api/values
    // Notice: [controller] is just a placeholder for 'values',
    // so it's replaced as such in the url. .Net gets 'values' from
    // our class name 'ValuesController' and knows to remove the 'Controller' part. 

    // SIDENOTE: All controllers inherit from ControllerBase, which gives us access to 
    // things like HTTP responses and Actions we can use inside our controllers. Angular
    // provides us View (MVC) support. The initial structure of our controller will use the 
    // attribute routing ([Route("api/[controller]")]) and [ApiController].
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext context;
        public ValuesController(DataContext context)
        {
            this.context = context;
        }

        // GET api/values
        // Returning 'IActionResult' allows us to return HTTP responses. 
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await this.context.Values.ToListAsync(); //context allows us access to EF commands

            return Ok(values); //HTTP 200 OK response
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task <ActionResult> GetValue(int id)
        {
            //x represents the value we're returning
            var value = await this.context.Values.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}