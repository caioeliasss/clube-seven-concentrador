; Inno Setup script — ClubeSevenBridge
; Compilar com: build-installer.bat  (chama publicar-instalador.bat + iscc)
; Requer Inno Setup 6 (https://jrsoftware.org/isdl.php).

#define AppName "ClubeSevenBridge"
#define AppPublisher "Clube Seven"
#define AppExe "SevenConcentradorBridge.exe"
#define AppVersion "0.5.0"
#define DefaultPort "5100"

[Setup]
AppId={{8C2F1B30-7E5A-4C2E-9B1D-A1B2C3D4E5F6}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\ClubeSevenBridge
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
PrivilegesRequired=admin

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "autostart"; Description: "Iniciar automaticamente quando o Windows ligar"; GroupDescription: "Inicialização:"
Name: "firewall"; Description: "Liberar a porta {#DefaultPort} no Firewall do Windows"; GroupDescription: "Rede:"
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
