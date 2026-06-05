using Newtonsoft.Json;

namespace CGM_JDAV.Models;

public class AdministradorModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("usuario")]
    public string Usuario { get; set; } = "";

    [JsonProperty("contrasena")]
    public string Contrasena { get; set; } = "";
}