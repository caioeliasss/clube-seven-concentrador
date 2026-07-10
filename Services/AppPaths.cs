namespace SevenConcentradorBridge.Services;

// Resolve o caminho do appsettings.json GRAVÁVEL (o que o painel edita).
//
// Problema que isto resolve: o exe é instalado em Program Files (setup.iss → {autopf}),
// pasta protegida. Gravar config lá exigia rodar o bridge como administrador. Em produção,
// movemos o arquivo gravável para C:\ProgramData\ClubeSevenBridge\ (comum à máquina,
// liberado para escrita de usuários comuns pelo instalador), eliminando a exigência de admin.
//
// Em dev mantemos o appsettings.json do content root (pasta do projeto): gravar em outro
// lugar quebraria o fluxo dev, onde a config é lida do projeto (ver ConfigService).
public static class AppPaths
{
    // Retorna o caminho do appsettings.json gravável, garantindo que ele exista.
    // Em prod, cria C:\ProgramData\ClubeSevenBridge\ e, na primeira execução, semeia o
    // arquivo copiando o {ContentRoot}\appsettings.json. Como o instalador preserva o
    // appsettings.json antigo em upgrade (setup.iss, onlyifdoesntexist), essa cópia migra
    // a config real do cliente para o novo local sem perda.
    public static string EnsureWritableConfig(IHostEnvironment env)
    {
        if (env.IsDevelopment())
            return Path.Combine(env.ContentRootPath, "appsettings.json");

        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "ClubeSevenBridge");
        Directory.CreateDirectory(dir);

        var dest = Path.Combine(dir, "appsettings.json");
        if (!File.Exists(dest))
        {
            var seed = Path.Combine(env.ContentRootPath, "appsettings.json");
            if (File.Exists(seed))
            {
                try { File.Copy(seed, dest); } catch { /* segue com config default se a cópia falhar */ }
            }
        }
        return dest;
    }
}
