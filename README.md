	
   	PASOS DESARROLLO CRUD CON ARQUITECTURA DE N CAPAS Y MIGRATION DE .NET CORE

1- Creamos un nuevo proyecto CRUD.NCapas.BackEnd.Net.API con la solución CRUD.NCapas.BackEnd.Net: ASP.NET Core Web API en Visual Studio 2022, Framework 6.0.

2- Agregar el proyecto o biblioteca de clases a la solución: CRUD.NCapas.BackEnd.Net.Data

3- Agregar las carpetas Context, Mapping y Models al proyecto CRUD.NCapas.BackEnd.Net.Data y eliminar la clase Class1.cs

4- Agregar el proyecto o biblioteca de clases a la solución: CRUD.NCapas.BackEnd.Net.Domain.To

5- Agregar la carpeta ViewModels al proyecto CRUD.NCapas.BackEnd.Net.Domain.To y eliminar la clase Class1.cs

6- Agregar el proyecto o biblioteca de clases a la solución: CRUD.NCapas.BackEnd.Net.Service

7- Agregar la carpeta Implementation y Interface al proyecto CRUD.NCapas.BackEnd.Net.Service y eliminar la clase Class1.cs

8- Agregar dependencias de proyectos:
    CRUD.NCapas.BackEnd.Net.Data       Depende de CRUD.NCapas.BackEnd.Net.Domain.To
    CRUD.NCapas.BackEnd.Net.Service    Depende de CRUD.NCapas.BackEnd.Net.Domain.To
    CRUD.NCapas.BackEnd.Net.Service    Depende de CRUD.NCapas.BackEnd.Net.Data    
    CRUD.NCapas.BackEnd.Net.API        CRUD.NCapas.BackEnd.Net.Service

9- Agregar paquete de nuget Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools y AutoMapper.Extensions.Microsoft.DependencyInjection
   en proyecto CRUD.NCapas.BackEnd.Net.Data

10- Agregar paquete de nuget Microsoft.EntityFrameworkCore.Design en proyecto CRUD.NCapas.BackEnd.Net.API

11- Agregar clase Persona.cs en la carpeta Models del proyecto CRUD.NCapas.BackEnd.Net.Data:

using System.ComponentModel.DataAnnotations;

    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required]
        [StringLength(50)]
        public string NoDocumento { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }
    }

12- Agregar clase DataContext.cs en la carpeta Context del proyecto CRUD.NCapas.BackEnd.Net.Data:

using CRUD.NCapas.BackEnd.Net.Data.Models;
using Microsoft.EntityFrameworkCore;

    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        #region DataSets

        public DbSet<Persona> Persona { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(k => k.IdPersona);
                entity.Property(x => x.NoDocumento).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Nombres).IsRequired().HasMaxLength(100);
                entity.Property(x => x.Apellidos).IsRequired().HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

13- Agregamos ConnectionStrings en archivo appsettings.json del proyecto inicial, el nombre que coloquemos de base de datos, es con el que se va a crear
    automáticamente la base de datos: CRUD.NCapas.BackEnd.Net.API

,"ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\sqlexpress;Database=Persona;user id=diego.acevedoa;password=Medellin1*;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true"
  }

14- Agregar ConnectionStrings en archivo Program.cs del proyecto inicial CRUD.NCapas.BackEnd.Net.API después de builder:

using CRUD.NCapas.BackEnd.Net.Data.Context;
using Microsoft.EntityFrameworkCore;

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

15- Abrir la NuGet Package Manager (Consola del administrador de paquetes) por Herramientas --> Administrador de paquetes NuGet --> Consola del administrador de paquetes,
    seleccionamos el proyecto donde se van a ejecutar las migraciones (CRUD.NCapas.BackEnd.Net.Data) y Creamos la migración inicial:

    Add-Migration InitialCreate

16- Aplicamos migración para crear la base de datos y la tabla Persona: 
    
    Update-Database

17- Agregar Cors en archivo Program.cs después de builder:

builder.Services.AddCors(options =>
{
    options.AddPolicy("Personas.CORS", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


app.UseCors("Personas.CORS");

18- Agregar clase PersonaViewModel.cs en la carpeta ViewModels del proyecto CRUD.NCapas.BackEnd.Net.Domain.To:

using System.ComponentModel.DataAnnotations;

    public class PersonaViewModel
    {
        public int IdPersona { get; set; }


        [Display(Name = "NoDocumento")]
        [StringLength(50)]
        public string NoDocumento { get; set; }

        [Display(Name = "Nombres")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(100)]
        public string Apellidos { get; set; }
    }

19- Agregar clase Mapeos.cs en la carpeta Mapping del proyecto CRUD.NCapas.BackEnd.Net.Data:

using AutoMapper;
using CRUD.NCapas.BackEnd.Net.Data.Models;
using CRUD.NCapas.BackEnd.Net.Domain.To.ViewModels;

    public class Mapeos : Profile
    {
        public Mapeos()
        {
            #region Persona 

            CreateMap<Persona, PersonaViewModel>()
                 .ForMember(m => m.IdPersona, map => map.MapFrom(vm => vm.IdPersona))
                 .ForMember(m => m.Nombres, map => map.MapFrom(vm => vm.Nombres))
                 .ForMember(m => m.Apellidos, map => map.MapFrom(vm => vm.Apellidos))
                 .ForMember(m => m.NoDocumento, map => map.MapFrom(vm => vm.NoDocumento))
                 .ReverseMap();

            #endregion
        }
    }


20- Agregar clase IPersonaService.cs en la carpeta Interface del proyecto CRUD.NCapas.BackEnd.Net.Service:

using CRUD.NCapas.BackEnd.Net.Data.Models;

    public interface IPersonaService
    {
        List<Persona> GetAll();

        Persona? GetById(int idPersona);

        Persona Add(Persona persona);

        bool Update(Persona persona);

        bool Delete(int idPersona);
    }

21- Agregar clase PersonaService.cs en la carpeta Implementation del proyecto CRUD.NCapas.BackEnd.Net.Service:

using CRUD.NCapas.BackEnd.Net.Data.Context;
using CRUD.NCapas.BackEnd.Net.Data.Models;
using CRUD.NCapas.BackEnd.Net.Service.Interface;
using Microsoft.EntityFrameworkCore;

    public class PersonaService : IPersonaService
    {
        private readonly DataContext _dataContext;

        public PersonaService(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public List<Persona> GetAll()
        {
            return _dataContext.Persona.ToList();
        }

        public Persona? GetById(int idPersona)
        {
            return _dataContext.Persona.Find(idPersona);
        }

        public Persona Add(Persona persona)
        {
            _dataContext.Persona.Add(persona);
            _dataContext.SaveChanges();

            return persona;
        }

        public bool Update(Persona persona)
        {
            _dataContext.Entry(persona).State = EntityState.Modified;
            _dataContext.SaveChanges();

            return true;
        }

        public bool Delete(int idPersona)
        {
            var persona = _dataContext.Persona.Find(idPersona);

            if (persona == null)
            {
                return false;
            }

            _dataContext.Persona.Remove(persona);
            _dataContext.SaveChanges();

            return true;
        }
    }

22- Agregar "Controlador de API con acciones de lectura y escritura": PersonasController.cs

using AutoMapper;
using CRUD.NCapas.BackEnd.Net.Data.Models;
using CRUD.NCapas.BackEnd.Net.Domain.To.ViewModels;
using CRUD.NCapas.BackEnd.Net.Service.Interface;
using Microsoft.AspNetCore.Mvc;

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


23- Realizamos inyección de dependencias en archivo Program.cs después de builder:

using CRUD.NCapas.BackEnd.Net.Data.Context;
using CRUD.NCapas.BackEnd.Net.Data.Mapping;
using CRUD.NCapas.BackEnd.Net.Service.Implementation;
using CRUD.NCapas.BackEnd.Net.Service.Interface;
using Microsoft.EntityFrameworkCore;

builder.Services.AddTransient<IPersonaService, PersonaService>();
builder.Services.AddAutoMapper(typeof(Mapeos));


24- Ejecutar y probar