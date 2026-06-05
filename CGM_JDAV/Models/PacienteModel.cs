using Newtonsoft.Json;

namespace CGM_JDAV.Models;

public class PacienteModel
{
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public int? Id { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; } = "";

    [JsonProperty("apellido")]
    public string Apellido { get; set; } = "";

    [JsonProperty("dni")]
    public string Dni { get; set; } = "";

    [JsonProperty("identificador")]
    public string Identificador { get; set; } = "";

    [JsonProperty("fechanacimiento")]
    public string FechaNacimiento { get; set; } = "";

    [JsonProperty("correoelectronico")]
    public string CorreoElectronico { get; set; } = "";

    public override string ToString() => $"{Nombre} {Apellido}";
}