using AutoMapper;
using CRUD.NCapas.BackEnd.Net.Data.Models;
using CRUD.NCapas.BackEnd.Net.Domain.To.ViewModels;
using CRUD.NCapas.BackEnd.Net.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUD.NCapas.BackEnd.Net.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;

        public PersonasController(IPersonaService personaService, IMapper mapper)
        {
            _personaService = personaService;
            _mapper = mapper;
        }

        // GET: api/<PersonasController>
        [HttpGet]
        public ActionResult<IEnumerable<PersonaViewModel>> GetAllPersona()
        {
            List<Persona> list = _personaService.GetAll();
            var mapped = _mapper.Map<List<Persona>, List<PersonaViewModel>>(list);

            return mapped;
        }

        // GET api/<PersonasController>/5
        [HttpGet("{id}")]
        public ActionResult<PersonaViewModel> GetByIdPersona(int id)
        {
            Persona? data = _personaService.GetById(id);
            var mapped = _mapper.Map<Persona?, PersonaViewModel>(data);

            return mapped;
        }

        // POST api/<PersonasController>
        [HttpPost]
        public ActionResult<PersonaViewModel> Post([FromBody] Persona model)
        {
            Persona data = _personaService.Add(model);
            var mapped = _mapper.Map<Persona, PersonaViewModel>(data);

            return CreatedAtAction("GetByIdPersona", new { id = mapped.IdPersona }, mapped);
        }

        // PUT api/<PersonasController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Persona model)
        {
            if (id != model.IdPersona)
            {
                return BadRequest();
            }

            _personaService.Update(model);

            return NoContent();
        }

        // DELETE api/<PersonasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personaService.Delete(id);

            return NoContent();
        }
    }
}
