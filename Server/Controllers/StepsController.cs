using AutoHub.Server.Data;
using AutoHub.Shared;
using AutoHub.Shared.Entities;
using AutoHub.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Server.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("API/[controller]")]
    public class StepsController : ControllerBase
    {

        private readonly MainDbContext _db;
        private readonly ILogger<StepsController> _logger;

        public StepsController(ILogger<StepsController> logger, MainDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Steps()
        {
            var response = new Response<List<Step>>();

            try
            {
                response.Data = await _db.Steps.Include(x => x.Items).ToListAsync();
                response.Data.SelectMany(x => x.Items!).ToList().ForEach(x => x.Step = null);
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(StepsController)}>{nameof(Steps)}"); return BadRequest(response); }

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Step(int id)
        {
            var response = new Response<Step>();

            try
            {
                response.Data = await _db.Steps.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(StepsController)}>{nameof(Step)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Step step)
        {
            var response = new Response<Step>();

            try
            {
                _db.Entry(step).State = EntityState.Added;
                await _db.SaveChangesAsync();
                response.Data = step;
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(StepsController)}>{nameof(Create)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Step step)
        {
            var response = new Response<Step>();

            try
            {
                _db.Entry(step).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                response.Data = step;
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(StepsController)}>{nameof(Update)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new Response<Step>();

            try
            {
                var step = await _db.Steps.FirstOrDefaultAsync(x => x.Id == id);
                if (step == null)
                {
                    response.AddError(ResponseErrorCode.NotFound);
                    return NotFound(response);
                }

                _db.Entry(step).State = EntityState.Deleted;
                await _db.SaveChangesAsync();

            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(StepsController)}>{nameof(Delete)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpGet("IsAdded")]
        public async Task<bool> IsAdded(string title, int id) =>
            await _db.Steps.AnyAsync(x => x.Title!.ToLower() == title!.ToLower() && x.Id != id);
    }
}