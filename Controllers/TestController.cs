using CryptoTrading.Data;
using CryptoTrading.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrading.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly CryptoTradingDbContext _context;

        public TestController(CryptoTradingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestTable>>> GetAll()
        {
            var data = await _context.TestTables.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestTable>> GetById(int id)
        {
            var test = await _context.TestTables.FindAsync(id);
            if (test == null)
                return NotFound();
            return Ok(test);
        }

        [HttpPost]
        public async Task<ActionResult<TestTable>> Create(TestTable item)
        {
            _context.TestTables.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TestTable item)
        {
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.TestTables.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.TestTables.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}


