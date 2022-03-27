#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CherieAppBankSound.Models;
using CherieAppBankSound.Services;
using Microsoft.AspNetCore.SignalR;

namespace CherieAppBankSound.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MySoundsController : Controller
{
    private readonly MySoundContext _context;
    private readonly ILogger<MySoundsController> _logger;
    private readonly IHubContext<MyDiscussionHub> _hubContext;

    public MySoundsController(MySoundContext context,
        ILogger<MySoundsController> logger,
    IHubContext<MyDiscussionHub> hubContext)
    {
        _context = context;
        _logger = logger;
        _hubContext = hubContext;
    }

    // GET: MySounds
    [HttpGet("List")]
    public async Task<List<MySound>> ListOfSoundsAsync()
    {
        return await _context.MySounds.ToListAsync();
    }

    // GET: MySounds/Details/5
    [HttpGet("/MySoundsDetails/{id}")]
    public async Task<MySound> DetailsAsync(Guid? id)
    {
        if (id == null)
        {
            return new MySound();
        }

        var mySound = await _context.MySounds
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mySound == null)
        {
            return new MySound();
        }

        return mySound;
    }

    // POST: MySounds/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost("/Create")]
    [ValidateAntiForgeryToken]
    public async Task<MySound> CreateAsync([Bind("Id,Name,MyAudio")] MySound mySound)
    {
        if (ModelState.IsValid)
        {
            mySound.Id = Guid.NewGuid();
            _context.Add(mySound);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("broadcastMessage", "New Sound", mySound);
            return mySound;
        }
        return mySound;
    }

    // POST: MySounds/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost("/Edit")]
    [ValidateAntiForgeryToken]
    public async Task<MySound> EditAsync(Guid id, [Bind("Id,Name,MyAudio")] MySound mySound)
    {
        if (id != mySound.Id)
        {
            return new MySound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(mySound);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("broadcastMessage", "Update Sound", mySound);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MySoundExists(mySound.Id))
                {
                    return new MySound();
                }
                else
                {
                    throw;
                }
            }
            return mySound;
        }
        return mySound;
    }

    // POST: MySounds/Delete/5
    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<MySound> DeleteConfirmed(Guid id)
    {
        var mySound = await _context.MySounds.FindAsync(id);
        _context.MySounds.Remove(mySound);
        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("broadcastMessage", "Deleted Sound", mySound);
        return mySound;
    }

    private bool MySoundExists(Guid id)
    {
        return _context.MySounds.Any(e => e.Id == id);
    }
}
