﻿using System;
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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialobject = _context.CelestialObjects.Find(id);
            if (celestialobject == null)
                return NotFound();
            celestialobject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
            return Ok(celestialobject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.OrbitedObjectId).ToList();
            }
            return Ok(celestialObjects);

        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            var celestialObjects = _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject co)
        {
            var celestialObjects = _context.CelestialObjects.Find(id);
            if (celestialObjects == null)
                return NotFound();
            celestialObjects.Name = co.Name;
            celestialObjects.OrbitalPeriod = co.OrbitalPeriod;
            celestialObjects.OrbitedObjectId = co.OrbitedObjectId;
            _context.CelestialObjects.Update(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObjects = _context.CelestialObjects.Find(id);
            if (celestialObjects == null)
                return NotFound();
            celestialObjects.Name = name;
            _context.CelestialObjects.Update(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();
            if (!celestialObjects.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
