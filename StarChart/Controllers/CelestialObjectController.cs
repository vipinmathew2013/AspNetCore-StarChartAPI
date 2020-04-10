using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject output = new CelestialObject();
            List<CelestialObject> result = _context.CelestialObjects.Where(c => c.Id == id).ToList();
            if (result.Any())
                return NotFound();
            output.Satellites = result;
            return Ok(output);

        }

        [HttpGet("{name:string }")]
        [ActionName("GetByName")]
        public IActionResult GetByName(string name)
        {
            CelestialObject output = new CelestialObject();
            List<CelestialObject> result = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (result.Any())
                return NotFound();
            output.Satellites = result;
            return Ok(output);

        }

        [HttpGet("{name:string }")]
        [ActionName("GetAll")]
        public IActionResult GetAll()
        {
            List<CelestialObject> result = _context.CelestialObjects.ToList();
            if (result.Any())
                return NotFound();
            result.ForEach(c => c.Satellites =  result.Where(f=>f.OrbitedObjectId==f.Id).ToList());
            return Ok(result);

        }
    }
}
