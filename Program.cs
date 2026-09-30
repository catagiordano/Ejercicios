using AccesoDatos.Data;
using AccesoDatos.Models;
using AccesoDatos.Repositories;
using Microsoft.EntityFrameworkCore;

var vehiculoRepository = new GenericRepository<Vehiculo>();
var clientRepository = new GenericRepository<Client>();
var alquilerRepository = new GenericRepository<Alquiler>();
var alquilerVehiculoRepository = new GenericRepository<AlquilerVehiculo>();

bool continuar = true;

while (continuar)

{
    Console.Clear();

    Console.WriteLine("   RENT A CAR    ");
    Console.WriteLine();

    Console.WriteLine("1. Registrar vehículo");
    Console.WriteLine("2. Registrar cliente");
    Console.WriteLine("3. Registrar alquiler");
    Console.WriteLine();

    Console.WriteLine("4. Devolver alquiler");
    Console.WriteLine("5. Reporte de alquiler por vehículo");
    Console.WriteLine("6. Clientes con demora");
    Console.WriteLine("7. Vehículos más alquilados");
    Console.WriteLine("8. Cliente que más vehículos alquiló");
    Console.WriteLine("0. Salir");
    Console.WriteLine();

    Console.Write("Seleccione una opción: ");
    string opcion = Console.ReadLine();

    Console.Clear();

    switch (opcion)
    {
        case "1":
            RegistrarVehiculo();
            break;

        case "2":
            Registrarcliente();
            break;

        case "3":
            RegistrarAlquiler();
            break;

        case "4":
            DevolverAlquiler();
            break;

        case "5":
            ReporteAlquilerPorVehiculo();
            break;

        case "6":
            ReportarClientesConDemora();
            break;

        case "7":
            ReporteVehiculosMasAlquilados();
            break;

        case "8":
            ReporteClienteQueMasAlquilo();
            break;

        case "0":
            continuar = false;
            Console.WriteLine("Aplicación finalizada.");
            break;


        default:
            Console.WriteLine("Opción inválida.");
            PresioneParaContinuar();
            break;
    }
}

void RegistrarVehiculo()
{
    Console.Clear();
    Console.Write("   REGISTRAR VEHICULO    ");
    Vehiculo vehiculo = new Vehiculo();

    Console.WriteLine();
    Console.Write("Patente: ");
    vehiculo.Patente = Console.ReadLine();

    Console.WriteLine();
    Console.Write("Marca: ");
    vehiculo.Marca = Console.ReadLine();

    Console.WriteLine();
    Console.Write("Modelo: ");
    vehiculo.Modelo = Console.ReadLine();

    Console.WriteLine();
    Console.Write("Precio por dia: ");
    decimal.TryParse(Console.ReadLine(), out decimal precio);
    vehiculo.PrecioPorDia = precio;

    Console.WriteLine();
    Console.Write("Cantidad disponible: ");
    int.TryParse(Console.ReadLine(), out int cantidad);
    vehiculo.CantidadDisponible = cantidad;

    Console.WriteLine();
    Console.Write("Valor del vehiculo: ");
    decimal.TryParse(Console.ReadLine(), out decimal valor);
    vehiculo.ValorVehiculo = valor;

    vehiculoRepository.Agregar(vehiculo);
    Console.WriteLine("VEHICULO REGISTRADO EXITOSAMENTE.");

    PresioneParaContinuar();
}

void Registrarcliente()

{
    Console.Clear();
    Console.Write("   REGISTRAR CLIENTE    ");
    Client client = new Client();

    Console.Write("Nombre: ");
    client.Nombre = Console.ReadLine();

    Console.Write("Apellido: ");
    client.Apellido = Console.ReadLine();

    Console.Write("DNI: ");
    client.DNI = Console.ReadLine();

    Console.Write("Lincencia de Conducir: ");
    client.LicenciaConducir = Console.ReadLine();

    Console.Write("Teléfono: ");
    client.Telefono = Console.ReadLine();

    client.TieneDescuento = false;

    clientRepository.Agregar(client);
    Console.WriteLine("CLIENTE REGISTRADO EXITOSAMENTE.");

    PresioneParaContinuar();
}

void RegistrarAlquiler()
{
    Console.Clear();
    Console.Write("   REGISTRAR ALQUILER    ");
    List<Client> clientes = clientRepository.ObtenerTodos();

    if (clientes.Count == 0)
    {
        Console.WriteLine("NO HAY CLIENTES REGISTRADOS");
        PresioneParaContinuar();
        return;

    }

    Console.WriteLine("CLIENTES");

    foreach (var client in clientes)
    {
        Console.WriteLine(
           $"{client.Id} -" +
           $" {client.Nombre}" +
           $" {client.Apellido}"
            );
    }

    Console.Write("INGRESE ID DEL CLIENTE: ");
    int.TryParse(Console.ReadLine(), out int clientId);

    Client clienteseleccionado = clientRepository.ObtenerPorId(clientId);

    if (clienteseleccionado == null)
    {
        Console.WriteLine("CLIENTE NO ENCONTRADO");
        PresioneParaContinuar();
        return;
    }

    List<Vehiculo> vehiculos =
         vehiculoRepository.ObtenerTodos()
        .Where(v => v.CantidadDisponible > 0)
        .ToList();

    if (vehiculos.Count == 0)
    {
        Console.WriteLine("NO HAY VEHICULOS DISPONIBLES");
        PresioneParaContinuar();
        return;
    }

    Console.WriteLine();
    Console.WriteLine("VEHICULOS DISPONIBLES");

    foreach (var vehiculo in vehiculos)
    {
        Console.WriteLine(
            $"{vehiculo.Id} -" +
            $" {vehiculo.Marca}" +
            $" {vehiculo.Modelo} - " +
            $"Precio por dia: {vehiculo.PrecioPorDia} - " +
            $"Cantidad disponible: {vehiculo.CantidadDisponible}"
            );
    }

    Console.Write("CUANTOS VEHICULOS DESEA ALQUILAR?: ");
    int.TryParse(Console.ReadLine(), out int cantidadVehiculos);

    if (cantidadVehiculos <= 0)
    {
        Console.WriteLine("CANTIDAD INVALIDA");
        PresioneParaContinuar();
        return;
    }

    List<Vehiculo> vehiculosSeleccionados =
        new List<Vehiculo>();

    decimal montoBase = 0;
    decimal montoSeguro = 0;
    decimal montoFinal = 0;
    decimal porcentajeSeguro = 0;
    int dias = 0;

    for (int i = 0; i < cantidadVehiculos; i++)
    {
        Console.Write($"INGRESE ID DEL VEHICULO {i + 1}: ");

        int.TryParse(Console.ReadLine(), out int vehiculoId);
        Vehiculo vehiculoSeleccionado =
            vehiculoRepository.ObtenerPorId(vehiculoId);
        if (vehiculoSeleccionado == null)
        {
            Console.WriteLine("VEHICULO NO DISPONIBLE");
            i--;
            continue;
        }
        vehiculosSeleccionados.Add(vehiculoSeleccionado);

        Console.Write("Cantidad de días: ");
        dias = int.TryParse(Console.ReadLine(), out int diasTmp) ? diasTmp : 0;

        if (dias <= 0)
        {
            Console.WriteLine("Cantidad de días incorrecta.");
            PresioneParaContinuar();
            return;
        }

        foreach (var vehiculo in vehiculosSeleccionados)
        {
            montoBase +=
                vehiculo.PrecioPorDia * dias;
        }

        Console.WriteLine();
        Console.WriteLine("SEGURO");
        Console.WriteLine("1. 5%");
        Console.WriteLine("2. 10%");
        Console.WriteLine("3. 15%");

        Console.Write("Seleccione seguro: ");
        int.TryParse(Console.ReadLine(), out int opcionSeguro);

        porcentajeSeguro = 0;

        if (opcionSeguro == 1)
        {
            porcentajeSeguro = 5;
        }
        else if (opcionSeguro == 2)
        {
            porcentajeSeguro = 10;
        }
        else if (opcionSeguro == 3)
        {
            porcentajeSeguro = 15;
        }

        montoSeguro = 0;

        foreach (var vehiculo in vehiculosSeleccionados)
        {
            montoSeguro +=
                vehiculo.ValorVehiculo *
                porcentajeSeguro / 100;
        }

        montoFinal = montoBase + montoSeguro;
    }
    if (clienteseleccionado.TieneDescuento)
    {
        Console.WriteLine();
        Console.WriteLine("El cliente tiene un descuento del 10%.");

        montoFinal = montoFinal * 0.90m;

        clienteseleccionado.TieneDescuento = false;

        clientRepository.Modificar(clienteseleccionado);
    }

    DateTime fechaAlquiler = DateTime.Now;

    DateTime fechaLimite =
        fechaAlquiler.AddDays(dias);

    Alquiler alquiler = new Alquiler();

    alquiler.ClienteId = clienteseleccionado.Id;
    alquiler.FechaAlquiler = fechaAlquiler;
    alquiler.FechaLimite = fechaLimite;
    alquiler.MontoBase = montoBase;
    alquiler.SeguroPorcentaje = porcentajeSeguro;
    alquiler.MontoSeguro = montoSeguro;
    alquiler.MontoFinal = montoFinal;
    alquiler.Devuelto = false;

    alquilerRepository.Agregar(alquiler);

    foreach (var vehiculo in vehiculosSeleccionados)
    {
        vehiculo.CantidadDisponible--;

        vehiculoRepository.Modificar(vehiculo);

        AlquilerVehiculo alquilerVehiculo =
            new AlquilerVehiculo();

        alquilerVehiculo.AlquilerId = alquiler.Id;
        alquilerVehiculo.VehiculoId = vehiculo.Id;

        alquilerVehiculoRepository.Agregar(
            alquilerVehiculo);
    }

    Console.WriteLine();
    Console.WriteLine("ALQUILER REGISTRADO");
    Console.WriteLine($"Monto base: ${montoBase}");
    Console.WriteLine($"Seguro: ${montoSeguro}");
    Console.WriteLine($"Monto final: ${montoFinal}");
    Console.WriteLine(
        $"Fecha límite: {fechaLimite:dd/MM/yyyy}");

    PresioneParaContinuar();
}

void DevolverAlquiler()
{
    Console.Clear();
    Console.Write("   DEVOLVER ALQUILER    ");

    List<Alquiler> alquileres =
        alquilerRepository.ObtenerTodos()
        .Where(a => !a.Devuelto)
        .ToList();

    if (alquileres.Count == 0)
    {
        Console.WriteLine("NO HAY ALQUILERES PENDIENTES");
        PresioneParaContinuar();
        return;
    }

    Console.WriteLine("ALQUILERES PENDIENTES");

    foreach (var alquiler in alquileres)
    {
        Console.WriteLine(
            $"{alquiler.Id} - " +
            $"Fecha límite: {alquiler.FechaLimite:dd/MM/yyyy}");
    }

    Console.Write("INGRESE ID DEL ALQUILER: ");
    int.TryParse(Console.ReadLine(), out int alquilerId);

    Alquiler alquilerSeleccionado =
        alquilerRepository.ObtenerPorId(alquilerId);

    if (alquilerSeleccionado == null || alquilerSeleccionado.Devuelto)
    {
        Console.WriteLine("ALQUILER NO ENCONTRADO");
        PresioneParaContinuar();
        return;
    }

    alquilerSeleccionado.Devuelto = true;
    alquilerSeleccionado.FechaDevolucion = DateTime.Now;

    alquilerRepository.Modificar(alquilerSeleccionado);

    List<AlquilerVehiculo> vehiculosDelAlquiler =
        alquilerVehiculoRepository.ObtenerTodos()
        .Where(av => av.AlquilerId == alquilerSeleccionado.Id)
        .ToList();

    foreach (var av in vehiculosDelAlquiler)
    {
        Vehiculo vehiculo = vehiculoRepository.ObtenerPorId(av.VehiculoId);

        if (vehiculo != null)
        {
            vehiculo.CantidadDisponible++;
            vehiculoRepository.Modificar(vehiculo);
        }
    }

    Console.WriteLine("ALQUILER DEVUELTO EXITOSAMENTE.");
    PresioneParaContinuar();
}

void ReporteAlquilerPorVehiculo()
{
    Console.Clear();
    Console.Write("   REPORTE DE ALQUILER POR VEHICULO    ");
    List<Vehiculo> vehiculos = vehiculoRepository.ObtenerTodos();
    if (vehiculos.Count == 0)
    {
        Console.WriteLine("NO HAY VEHICULOS REGISTRADOS");
        PresioneParaContinuar();
        return;
    }
    foreach (var vehiculo in vehiculos)
    {
        int cantidadAlquileres =
            alquilerVehiculoRepository
            .ObtenerTodos()
            .Count(av => av.VehiculoId == vehiculo.Id);
        Console.WriteLine(
            $"{vehiculo.Marca} {vehiculo.Modelo} - " +
            $"Cantidad de alquileres: {cantidadAlquileres}");
    }
    PresioneParaContinuar();
}

void ReportarClientesConDemora()
{
    Console.Clear();
    Console.Write("   REPORTE DE CLIENTES CON DEMORA    ");
    List<Alquiler> alquileres =
        alquilerRepository.ObtenerTodos()
        .Where(a => !a.Devuelto && a.FechaLimite < DateTime.Now)
        .ToList();
    if (alquileres.Count == 0)
    {
        Console.WriteLine("NO HAY CLIENTES CON DEMORA");
        PresioneParaContinuar();
        return;
    }
    foreach (var alquiler in alquileres)
    {
        Client client = clientRepository.ObtenerPorId(alquiler.ClienteId);
        Console.WriteLine(
            $"{client.Nombre} {client.Apellido} - " +
            $"Fecha límite: {alquiler.FechaLimite:dd/MM/yyyy}");
    }
    PresioneParaContinuar();
}

void ReporteVehiculosMasAlquilados()
{
    Console.Clear();
    Console.Write("   REPORTE DE VEHICULOS MAS ALQUILADOS    ");
    List<Vehiculo> vehiculos = vehiculoRepository.ObtenerTodos();
    if (vehiculos.Count == 0)
    {
        Console.WriteLine("NO HAY VEHICULOS REGISTRADOS");
        PresioneParaContinuar();
        return;
    }
    var vehiculosMasAlquilados = vehiculos
        .Select(v => new
        {
            Vehiculo = v,
            CantidadAlquileres = alquilerVehiculoRepository
                .ObtenerTodos()
                .Count(av => av.VehiculoId == v.Id)
        })
        .OrderByDescending(v => v.CantidadAlquileres)
        .ToList();
    foreach (var item in vehiculosMasAlquilados)
    {
        Console.WriteLine(
            $"{item.Vehiculo.Marca} {item.Vehiculo.Modelo} - " +
            $"Cantidad de alquileres: {item.CantidadAlquileres}");
    }
    PresioneParaContinuar();
}

void ReporteClienteQueMasAlquilo()
{
    Console.Clear();
    Console.Write("   REPORTE DE CLIENTE QUE MAS ALQUILO    ");
    List<Client> clientes = clientRepository.ObtenerTodos();
    if (clientes.Count == 0)
    {
        Console.WriteLine("NO HAY CLIENTES REGISTRADOS");
        PresioneParaContinuar();
        return;
    }
    var clienteMasAlquileres = clientes
        .Select(c => new
        {
            Cliente = c,
            CantidadAlquileres = alquilerRepository
                .ObtenerTodos()
                .Count(a => a.ClienteId == c.Id)
        })
        .OrderByDescending(c => c.CantidadAlquileres)
        .FirstOrDefault();
    if (clienteMasAlquileres == null || clienteMasAlquileres.CantidadAlquileres == 0)
    {
        Console.WriteLine("NINGUN CLIENTE HA REALIZADO ALQUILERES");
        PresioneParaContinuar();
        return;
    }
    Console.WriteLine(
        $"{clienteMasAlquileres.Cliente.Nombre} {clienteMasAlquileres.Cliente.Apellido} - " +
        $"Cantidad de alquileres: {clienteMasAlquileres.CantidadAlquileres}");
    PresioneParaContinuar();
}

void MostrarVehiculos()
{
    var vehiculos =
                vehiculoRepository.ObtenerTodos();

    foreach (var vehiculo in vehiculos)
    {
        Console.WriteLine();
        Console.WriteLine($"ID: {vehiculo.Id}");
        Console.WriteLine(
            $"Patente: {vehiculo.Patente}");
        Console.WriteLine(
            $"Marca: {vehiculo.Marca}");
        Console.WriteLine(
            $"Modelo: {vehiculo.Modelo}");
        Console.WriteLine(
            $"Precio por día: ${vehiculo.PrecioPorDia}");
        Console.WriteLine(
            $"Disponibles: {vehiculo.CantidadDisponible}");
        Console.WriteLine(
            $"Valor: ${vehiculo.ValorVehiculo}");

        PresioneParaContinuar();
    }
}
void MostrarClientes()
{
    Console.Clear();

    Console.WriteLine("=== CLIENTES ===");

    var clientes =
        clientRepository.ObtenerTodos();

    foreach (var cliente in clientes)
    {
        Console.WriteLine();
        Console.WriteLine($"ID: {cliente.Id}");
        Console.WriteLine(
            $"Nombre: {cliente.Nombre} {cliente.Apellido}");
        Console.WriteLine(
            $"DNI: {cliente.DNI}");
        Console.WriteLine(
            $"Licencia: {cliente.LicenciaConducir}");
        Console.WriteLine(
            $"Teléfono: {cliente.Telefono}");
        Console.WriteLine(
            $"Tiene descuento: " +
            $"{(cliente.TieneDescuento ? "Sí" : "No")}");
    }
    PresioneParaContinuar();
}
void PresioneParaContinuar()
{
    Console.WriteLine();
    Console.WriteLine("Presione una tecla para continuar...");
    Console.ReadKey();
    Console.Clear();
}