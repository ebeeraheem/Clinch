using ClinchApi.Models;
using ClinchApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AddressService _addressService;

        public ValuesController(AddressService addressService)
        {
            _addressService = addressService;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<Address?> GetOne(int id)
        {
            //return $"The value is: {id}";
            return await _addressService.GetAddressById(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Address>> Post([FromBody] Address address)
        {
            var c = await _addressService.Create(address);
            var maybe = GetOne(c.Id);
            return CreatedAtAction(nameof(GetOne), new { id = c.Id }, c);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
