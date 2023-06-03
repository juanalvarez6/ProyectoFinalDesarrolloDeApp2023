using System;
using System.Collections.Generic;

namespace ProyectoFinal.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? Documento { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public double? Salario { get; set; }

    public int? IdDepartamento { get; set; }

    public virtual Departamento? DetDepartamentos { get; set; }
}
