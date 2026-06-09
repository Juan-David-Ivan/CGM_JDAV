# CGM-JDAV — Aplicación de gestión de citas médicas
 
Aplicación de escritorio para la **administración y recepción de una clínica**: permite gestionar especialidades, personal médico, pacientes y citas, y generar informes en PDF (volante y justificante).
 
Proyecto de fin de ciclo del CFGS de Desarrollo de Aplicaciones Multiplataforma (DAM).
 
## Tecnologías
 
- **C# / .NET 10**
- **Avalonia UI 11** (patrón MVVM)
- **CommunityToolkit.Mvvm**
- **Supabase** (PostgreSQL) a través de su **API REST**
- **Newtonsoft.Json** para el manejo de JSON
## Funcionalidades
 
- Inicio de sesión de administrador.
- Gestión completa (alta, consulta, edición y baja) de **especialidades, personal, pacientes y citas**.
- Validaciones al crear citas: el doctor se filtra según la especialidad y no se permiten dos citas del mismo doctor a la misma fecha y hora.
- El botón de volante se desactiva en las citas ya pasadas.
- Buscador de citas en tiempo real por paciente o médico.
- Visualización de los informes en PDF (volante y justificante) dentro de la propia aplicación mediante un WebView.
## Requisitos
 
- SDK de **.NET 10** instalado.
- Un IDE como **JetBrains Rider** o **Visual Studio**.
- La **API de informes en Java** en marcha si se quieren ver los PDF (ver más abajo).
## Cómo ejecutar
 
1. Clonar el repositorio.
2. Abrir la solución en Rider o Visual Studio.
3. Restaurar los paquetes NuGet.
4. Ejecutar el proyecto.
## Informes en PDF
 
Los informes (volante y justificante) los genera una **API independiente en Java** (Spring Boot + JasperReports). Para que la aplicación pueda mostrarlos en el WebView, esa API debe estar arrancada y escuchando en `http://localhost:8080`.
 
## Autor
 
Iván Juan David
