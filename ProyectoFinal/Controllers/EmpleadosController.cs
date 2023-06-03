using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {

        public readonly WebApiFinalContext _dbcontex;

        public EmpleadosController(WebApiFinalContext contex)
        {
            _dbcontex = contex;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<Empleado> listEmpl = new List<Empleado>();

            try
            {
                listEmpl = _dbcontex.Empleados.ToList();
                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Consulta Exitosa",
                    Detail = listEmpl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Error al consultar",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("Detalle")]
        public IActionResult Detalle(int id)
        {
            Empleado Empl = new Empleado();

            try
            {
                //Empl = _dbcontex.Empleados.Find(id);
                Empl = _dbcontex.Empleados.Include(c => c.DetDepartamentos).Where(e => e.IdEmpleado == id).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Consulta Exitosa",
                    Detail = Empl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Error al consultar",
                    Detail = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Guardar([FromBody] Empleado ObjProd)
        {
            try
            {
                _dbcontex.Empleados.Add(ObjProd);
                var result = _dbcontex.SaveChanges();
                if (result > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, new
                    {
                        code = "OK",
                        message = "Datos Almacendos Exitosamente",
                        Detail = ""
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "Error",
                        message = "No es posible almacenar datos",
                        Detail = ""
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "No es posible Almacenar Datos",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Editar([FromBody] Empleado ObjProd)
        {
            Empleado Empl = _dbcontex.Empleados.Find(ObjProd.IdEmpleado);

            if (Empl == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    code = "HTTP 400",
                    message = "No es posible Editar Datos",
                    Detail = ""
                });
            }
            try
            {
                Empl.Nombres = ObjProd.Nombres;
                Empl.Apellidos = ObjProd.Apellidos;

                if (ObjProd.Salario != null)
                {
                    Empl.Salario = ObjProd.Salario.Value;
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "Error",
                        message = "No es posible almacenar datos",
                        Detail = ""
                    });
                }

                if (ObjProd.IdDepartamento != null)
                {
                    Empl.IdDepartamento = ObjProd.IdDepartamento.Value;
                }
                else
                {
                    return BadRequest(new
                    {
                        code = "Error",
                        message = "No es posible almacenar datos",
                        Detail = ""
                    });
                }

                _dbcontex.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new
                {
                    code = "OK",
                    message = "Datos Modificados Exitosamente",
                    Detail = ""
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Error al Modificar",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Eliminar(int id)
        {
            Empleado Empl = _dbcontex.Empleados.Find(id);

            if (Empl == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    code = "HTTP 400",
                    message = "Datos no encontrados",
                    Detail = ""
                });
            }

            try
            {
                _dbcontex.Empleados.Remove(Empl);
                _dbcontex.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Datos Eliminados Exitosamente",
                    Detail = ""
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    code = "OK",
                    message = "Error al Eliminar",
                    Detail = ex.Message
                });
            }
        }
    }
}
