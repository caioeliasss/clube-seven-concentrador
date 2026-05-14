# companytec_shim

DLL wrapper Delphi/FPC pra expor `LePPLNivel` da `companytec.dll` com ABI C-friendly.

## Problema

`LePPLNivel` original (seção 2.6.6 do manual DT433) é Delphi-nativa:

```pascal
Function LePPLNivel(bico: ansistring; niveis: integer): PPLNivel; stdcall;
```

- `ansistring` = tipo managed Delphi com header escondido (refcount + length em offsets negativos). NÃO compatível com PChar de C#.
- Retorno é record por valor → Delphi insere `var Result` hidden no stdcall.

Chamar direto via P/Invoke `[MarshalAs(UnmanagedType.LPStr)] string` → Access Violation no worker.

## Solução

Shim Delphi que:
1. Carrega `companytec.dll` em runtime
2. Converte `PAnsiChar` → `AnsiString` (com header correto, alocado pelo runtime Delphi/FPC)
3. Chama `LePPLNivel` nativo
4. Retorna 3 doubles via `out` params + int de status

ABI exportada:

```pascal
function C_LePPLNivel(bico: PAnsiChar; niveis: Integer;
  out n0, n1, n2: Double): Integer; stdcall;
```

Retorno: 1 = sucesso, 0 = falha.

## Build

Requer [Free Pascal Compiler](https://www.freepascal.org/download.html) (grátis) ou Delphi IDE.

```cmd
build.bat
```

Gera `companytec_shim.dll` (x86) e copia pra raiz do projeto.

## Bitness

`companytec.dll` é provavelmente x86. Shim deve ser x86. Processo .NET deve ser x86 (`<PlatformTarget>x86</PlatformTarget>` no csproj).
