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
    public class ItemsController : ControllerBase
    {

        private readonly MainDbContext _db;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ILogger<ItemsController> logger, MainDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Items(int id)
        {
            var response = new Response<List<Item>>();

            try
            {
                response.Data = await _db.Items.Include(x => x.Step).Where(x => x.StepId == id).ToListAsync();
                response.Data.Select(x => x.Step).ToList().ForEach(x => x!.Items = null);
                int index = 0;
                response.Data.ForEach(x => x.Index = ++index);
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(ItemsController)}>{nameof(Items)}"); return BadRequest(response); }

            return Ok(response);
        }


        [HttpGet("ById")]
        public async Task<IActionResult> Item(int id)
        {
            var response = new Response<Item>();

            try
            {
                response.Data = await _db.Items.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(ItemsController)}>{nameof(Item)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Item item)
        {
            var response = new Response<Item>();

            try
            {
                _db.Entry(item).State = EntityState.Added;
                await _db.SaveChangesAsync();
                response.Data = item;
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(ItemsController)}>{nameof(Create)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Item item)
        {
            var response = new Response<Item>();

            try
            {
                _db.Entry(item).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                response.Data = item;
            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(ItemsController)}>{nameof(Update)}"); return BadRequest(response); }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new Response<Item>();

            try
            {
                var item = await _db.Items.FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                {
                    response.AddError(ResponseErrorCode.NotFound);
                    return NotFound(response);
                }

                _db.Entry(item).State = EntityState.Deleted;
                await _db.SaveChangesAsync();

            }
            catch (Exception ex) { response.AddError(ex); _logger.LogError(ex, $"{nameof(ItemsController)}>{nameof(Delete)}"); return BadRequest(response); }

            return Ok(response);
        }


        [HttpGet("IsAdded")]
        public async Task<bool> IsAdded(string title, int id, int stepId) =>
            await _db.Items.AnyAsync(x => x.Title!.ToLower() == title!.ToLower() && x.StepId == stepId && x.Id != id);
    }
}