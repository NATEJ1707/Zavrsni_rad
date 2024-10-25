// Controllers/KlimaController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


/// <summary>
/// API za upravljanje klima uređajima.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class KlimaController : ControllerBase
{
    private readonly AppDbContext _context;

    public KlimaController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Dohvaća sve klima uređaje.
    /// </summary>
    /// <returns>Lista svih klima uređaja.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Klima>>> GetKlime()
    {
        return await _context.Klime.Include(k => k.Marka).ToListAsync();
    }

    /// <summary>
    /// Dohvaća klima uređaj prema ID-u.
    /// </summary>
    /// <param name="id">ID klima uređaja.</param>
    /// <returns>Klima uređaj s navedenim ID-om.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Klima>> GetKlima(int id)
    {
        var klima = await _context.Klime.Include(k => k.Marka).FirstOrDefaultAsync(k => k.Id == id);
        if (klima == null)
        {
            return NotFound();
        }
        return klima;
    }

    /// <summary>
    /// Kreira novi klima uređaj.
    /// </summary>
    /// <param name="klima">Klima uređaj koji se kreira.</param>
    /// <returns>Napravljeni klima uređaj.</returns>
    [HttpPost]
    public async Task<ActionResult<Klima>> PostKlima(Klima klima)
    {
        // Provjera postoji li Marka s navedenim MarkaId
        var marka = await _context.Marke.FindAsync(klima.MarkaId);
        if (marka == null)
        {
            return BadRequest("Marka s navedenim ID-om ne postoji.");
        }

        _context.Klime.Add(klima);
        await _context.SaveChangesAsync();

        // Učitaj Marka navigacijsku svojstvo
        _context.Entry(klima).Reference(k => k.Marka).Load();

        return CreatedAtAction(nameof(GetKlima), new { id = klima.Id }, klima);
    }

    /// <summary>
    /// Ažurira postojeći klima uređaj.
    /// </summary>
    /// <param name="id">ID klima uređaja koji se ažurira.</param>
    /// <param name="klima">Ažurirani podaci klima uređaja.</param>
    /// <returns>NoContent ako je uspješno, ili odgovarajuća greška.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutKlima(int id, Klima klima)
    {
        if (id != klima.Id)
        {
            return BadRequest();
        }

        // Provjera postoji li Marka s navedenim MarkaId
        var marka = await _context.Marke.FindAsync(klima.MarkaId);
        if (marka == null)
        {
            return BadRequest("Marka s navedenim ID-om ne postoji.");
        }

        _context.Entry(klima).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!KlimaExists(id))
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
    /// Briše klima uređaj prema ID-u.
    /// </summary>
    /// <param name="id">ID klima uređaja koji se briše.</param>
    /// <returns>Klima uređaj koji je izbrisan.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Klima>> DeleteKlima(int id)
    {
        var klima = await _context.Klime.FindAsync(id);
        if (klima == null)
        {
            return NotFound();
        }

        _context.Klime.Remove(klima);
        await _context.SaveChangesAsync();

        return klima;
    }

    /// <summary>
    /// Provjerava postoji li klima uređaj s navedenim ID-om.
    /// </summary>
    /// <param name="id">ID klima uređaja.</param>
    /// <returns>True ako postoji, inače false.</returns>
    private bool KlimaExists(int id)
    {
        return _context.Klime.Any(e => e.Id == id);
    }
}
