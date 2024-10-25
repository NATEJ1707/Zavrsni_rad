// Controllers/MarkaController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


/// <summary>
/// API za upravljanje markama klima uređaja.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MarkaController : ControllerBase
{
    private readonly AppDbContext _context;

    public MarkaController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Dohvaća sve marke.
    /// </summary>
    /// <returns>Lista svih marki.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Marka>>> GetMarke()
    {
        return await _context.Marke.ToListAsync();
    }

    /// <summary>
    /// Dohvaća marku prema ID-u.
    /// </summary>
    /// <param name="id">ID marke.</param>
    /// <returns>Marka s navedenim ID-om.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Marka>> GetMarka(int id)
    {
        var marka = await _context.Marke.FindAsync(id);
        if (marka == null)
        {
            return NotFound();
        }
        return marka;
    }

    /// <summary>
    /// Kreira novu marku.
    /// </summary>
    /// <param name="marka">Marka koja se kreira.</param>
    /// <returns>Napravljena marka.</returns>
    [HttpPost]
    public async Task<ActionResult<Marka>> PostMarka(Marka marka)
    {
        _context.Marke.Add(marka);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMarka), new { id = marka.Id }, marka);
    }

    /// <summary>
    /// Ažurira postojeću marku.
    /// </summary>
    /// <param name="id">ID marke koja se ažurira.</param>
    /// <param name="marka">Ažurirani podaci marke.</param>
    /// <returns>NoContent ako je uspješno, ili odgovarajuća greška.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMarka(int id, Marka marka)
    {
        if (id != marka.Id)
        {
            return BadRequest();
        }

        _context.Entry(marka).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MarkaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary>
    /// Briše marku prema ID-u.
    /// </summary>
    /// <param name="id">ID marke koja se briše.</param>
    /// <returns>Marka koja je izbrisana.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Marka>> DeleteMarka(int id)
    {
        var marka = await _context.Marke.FindAsync(id);
        if (marka == null)
        {
            return NotFound();
        }

        _context.Marke.Remove(marka);
        await _context.SaveChangesAsync();

        return marka;
    }

    /// <summary>
    /// Provjerava postoji li marka s navedenim ID-om.
    /// </summary>
    /// <param name="id">ID marke.</param>
    /// <returns>True ako postoji, inače false.</returns>
    private bool MarkaExists(int id)
    {
        return _context.Marke.Any(e => e.Id == id);
    }
}
