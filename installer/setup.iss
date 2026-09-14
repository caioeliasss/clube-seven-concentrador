; Inno Setup script — ClubeSevenBridge
; Compilar com: build-installer.bat  (chama publicar-instalador.bat + iscc)
; Requer Inno Setup 6 (https://jrsoftware.org/isdl.php).

#define AppName "ClubeSevenBridge"
#define AppPublisher "Clube Seven"
#define AppExe "SevenConcentradorBridge.exe"
; Versão: build-installer.bat passa /DAppVersion (extraída do csproj). Default só p/ build manual.
#ifndef AppVersion
  #define AppVersion "0.0.0"
#endif
#define DefaultPort "5100"

[Setup]
; AppId NOVO (linha per-user): corta o vínculo com a instalação antiga em Admin/Program Files
; (AppId 8C2F1B30-...). Se mantivesse o AppId antigo, o Inno exigiria elevação para "atualizar"
; aquela instância — e UAC em conta de operador sem senha de admin travava o auto-update.
AppId={{5E8B2A47-C1D9-4F63-9B0E-7C3A4D5E6F81}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
; Instalação POR USUÁRIO, SEM admin (modelo Chrome/Discord): pasta gravável pelo próprio usuário
; é o que permite ao auto-update rodar o instalador silenciosamente sem prompt de UAC.
DefaultDirName={localappdata}\Programs\ClubeSevenBridge
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
OutputBaseFilename=ClubeSevenBridge-Setup-{#AppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
SetupIconFile=..\seven-logo.ico
UninstallDisplayIcon={app}\{#AppExe}
; App x86; permitir instalar em Windows 64-bit tambem.
ArchitecturesAllowed=x86 x64
PrivilegesRequired=lowest
; Upgrade com o app rodando (auto-start na bandeja): o AppMutex casa com o mutex nomeado
; criado pelo exe (Program.cs). CloseApplications fecha o bridge via Restart Manager antes de
; copiar os arquivos (libera o exe e a companytec.dll do worker); RestartApplications reinicia
; ao final — inclusive em modo silencioso (auto-update). Reinicia o EXE DO MESMO CAMINHO,
; que após o upgrade já é a versão nova (upgrade in-place da pasta per-user).
AppMutex=ClubeSevenBridgeSingleInstance
CloseApplications=yes
RestartApplications=yes

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Dirs]
; Pasta da config gravável (appsettings.json editado pelo painel). Fica fora do Program Files
; e é liberada para escrita de usuários comuns — assim o bridge NÃO precisa rodar como admin
; para salvar config. O app cria/semeia o arquivo na primeira execução (ver AppPaths.cs).
Name: "{commonappdata}\ClubeSevenBridge"; Permissions: users-modify

[Tasks]
Name: "autostart"; Description: "Iniciar automaticamente quando o Windows ligar"; GroupDescription: "Inicialização:"
; Regra de firewall exige admin — na instalação per-user (sem elevação) ela falha em silêncio.
; Desmarcada por padrão: painel e fila Socket.IO não precisam de entrada (loopback/saída).
; Marque apenas numa instalação manual feita por um admin que queira expor a porta na rede.
Name: "firewall"; Description: "Liberar a porta {#DefaultPort} no Firewall (requer admin)"; GroupDescription: "Rede:"; Flags: unchecked
Name: "desktopicon"; Description: "Criar atalho ""Abrir Painel"" na Área de Trabalho"; GroupDescription: "Atalhos:"

[Files]
; Conteúdo do publish self-contained (dist\). Inclui exe, companytec.dll, appsettings.json, wwwroot, runtime.
Source: "..\dist\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs ignoreversion
; appsettings.json: não sobrescrever em upgrade para preservar config do cliente.
Source: "..\dist\appsettings.json"; DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall

[INI]
; Atalho de internet que abre o painel no navegador padrão (com ícone da logo).
Filename: "{app}\Abrir Painel.url"; Section: "InternetShortcut"; Key: "URL"; String: "http://localhost:{#DefaultPort}/"
Filename: "{app}\Abrir Painel.url"; Section: "InternetShortcut"; Key: "IconFile"; String: "{app}\seven-logo.ico"
Filename: "{app}\Abrir Painel.url"; Section: "InternetShortcut"; Key: "IconIndex"; String: "0"

[Icons]
Name: "{group}\Abrir Painel"; Filename: "{app}\Abrir Painel.url"; IconFilename: "{app}\seven-logo.ico"
Name: "{group}\Iniciar Bridge"; Filename: "{app}\{#AppExe}"
Name: "{group}\Desinstalar {#AppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Abrir Painel ClubeSeven"; Filename: "{app}\Abrir Painel.url"; IconFilename: "{app}\seven-logo.ico"; Tasks: desktopicon

[Registry]
; Autostart no logon do usuário atual.
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; \
  ValueName: "ClubeSevenBridge"; ValueData: """{app}\{#AppExe}"""; \
  Flags: uninsdeletevalue; Tasks: autostart

[Run]
; Regra de firewall (opcional).
Filename: "{sys}\netsh.exe"; \
  Parameters: "advfirewall firewall add rule name=""ClubeSevenBridge"" dir=in action=allow protocol=TCP localport={#DefaultPort}"; \
  Flags: runhidden; Tasks: firewall
; Inicia agora e oferece abrir o painel.
Filename: "{app}\{#AppExe}"; Description: "Iniciar o bridge agora"; Flags: nowait postinstall skipifsilent
Filename: "{app}\Abrir Painel.url"; Description: "Abrir o painel no navegador"; Flags: shellexec postinstall skipifsilent nowait

[UninstallRun]
Filename: "{sys}\netsh.exe"; \
  Parameters: "advfirewall firewall delete rule name=""ClubeSevenBridge"""; \
  Flags: runhidden; RunOnceId: "DelFwRule"

[UninstallDelete]
Type: files; Name: "{app}\Abrir Painel.url"
