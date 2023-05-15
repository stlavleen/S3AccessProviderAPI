using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S3AccessProviderAPI.Contracts;
using S3AccessProviderAPI.Models;
using File = S3AccessProviderAPI.Models.File;

namespace S3AccessProviderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3AccessController : ControllerBase
    {
        private readonly FileDbContext _context;
        private readonly IS3HandlerService service;

        public S3AccessController(FileDbContext context, IS3HandlerService service)
        {
            _context = context;
            this.service = service;
        }

        // GET: api/S3Access
        [HttpGet(Name = "GetObjectsListAsync")]
        public async Task<ActionResult<IEnumerable<File>>> Get()
        {
            if (_context.Files == null)
            {
                return NotFound();
            }
            //return await _context.Files.ToListAsync();
            return await service.GetObjectsListAsync();
        }

        // GET: api/S3Access/5
        [HttpGet("{id}", Name = "GetPermanentLinkToDownloadFile")]
        public async Task<ActionResult<string>> Get(string id)
        {
            if (_context.Files == null)
            {
                return NotFound();
            }
            var file = await _context.Files.FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            return service.GetPermanentLink(id);
        }

        //POST: api/S3Access
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Implemented according to the condition that the client is guaranteed to upload the file, 
        // otherwise need to wait for the temp link expiration,
        // check s3 storage and update table if needed
        [HttpPost(Name = "GetTemporaryLinkToUploadFile")]
        public async Task<ActionResult<File>> Post(File file)
        {
            if (_context.Files == null)
            {
                return Problem("Entity set 'FileDbContext.Files'  is null.");
            }

            string url = string.Empty;
            _context.Files.Add(file);

            try
            {
                await _context.SaveChangesAsync();
                url = service.GetTemporaryLinkToUploadFile(file.Key);
            }
            catch (DbUpdateException)
            {
                if (FileExists(file.Key))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(url);
        }

        private bool FileExists(string id)
        {
            return (_context.Files?.Any(e => e.Key == id)).GetValueOrDefault();
        }
    }
}
