# ReadPriceLiterLevel0

Export nativo de `companytec.dll` — lê preço por litro do nível 0 de um bico.

## Assinatura

```pascal
Function ReadPriceLiterLevel0(nozzle: PChar): Integer; stdcall;
```

Variantes irmãs (mesma ABI):

```pascal
Function ReadPriceLiterLevel1(nozzle: PChar): Integer; stdcall;
Function ReadPriceLiterLevel2(nozzle: PChar): Integer; stdcall;
```

## ABI

| Item | Valor |
|---|---|
| DLL | `companytec.dll` |
| Calling convention | `stdcall` |
| Bitness | x86 (32-bit) |
| Charset | ANSI |
| EntryPoint | `ReadPriceLiterLevel0` (sem mangling, sem prefixo `_`) |

Processo chamador **precisa ser x86**. Não roda em processo x64.

## Parâmetro

- `nozzle` — `PChar` (ANSI, null-terminated). ID do bico em decimal padded a 2 chars: `"01"` .. `"32"`.
  - Valores hex (`"0A"`, `"FF"`, etc) podem crashar DLL — protocolo Companytec espera decimal.
  - Bico fora do range do concentrador retorna falha (< 0), não crash.

## Retorno

`Integer` (4 bytes, signed).

- `>= 0` — preço × 1000 (3 decimais). Exemplo: `5900` = R$ 5,900; `12345` = R$ 12,345.
- `< 0` — falha (sem preço, bico inexistente, ou erro de comunicação).

Conversão: `precoReais = raw / 1000m` (use decimal pra evitar float drift).

## Pré-condições

1. DLL carregada (`LoadLibrary("companytec.dll")` ou link estático).
2. Conexão ativa com concentrador via:
   - `C_OpenSocket2(ip, porta)` — TCP/Ethernet, **ou**
   - `C_OpenSerial(numeroPorta)` — RS-232.
   Função falha se chamada antes do `Open*`.
3. DLL **não é thread-safe** — serializar chamadas em uma única thread.

## Exemplo — C# (P/Invoke)

```csharp
using System.Runtime.InteropServices;
using System.Globalization;

public static class CompanytecDll
{
    private const string DllName = "companytec.dll";

    [DllImport(DllName, EntryPoint = "ReadPriceLiterLevel0",
        CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int ReadPriceLiterLevel0(
        [MarshalAs(UnmanagedType.LPStr)] string nozzle);

    [DllImport(DllName, EntryPoint = "ReadPriceLiterLevel1",
        CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int ReadPriceLiterLevel1(
        [MarshalAs(UnmanagedType.LPStr)] string nozzle);

    [DllImport(DllName, EntryPoint = "ReadPriceLiterLevel2",
        CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int ReadPriceLiterLevel2(
        [MarshalAs(UnmanagedType.LPStr)] string nozzle);
}

// Uso
int raw = CompanytecDll.ReadPriceLiterLevel0("04");
if (raw < 0)
{
    Console.WriteLine("Falha: bico não encontrado ou DLL erro");
}
else
{
    decimal preco = raw / 1000m;
    Console.WriteLine(preco.ToString("F3", CultureInfo.InvariantCulture)); // "5.900"
}
```

Projeto deve forçar x86:

```xml
<PropertyGroup>
  <PlatformTarget>x86</PlatformTarget>
  <RuntimeIdentifier>win-x86</RuntimeIdentifier>
</PropertyGroup>
```

## Exemplo — VB.NET

```vbnet
Imports System.Runtime.InteropServices

Module CompanytecDll
    <DllImport("companytec.dll", EntryPoint:="ReadPriceLiterLevel0",
        CallingConvention:=CallingConvention.StdCall,
        CharSet:=CharSet.Ansi)>
    Function ReadPriceLiterLevel0(<MarshalAs(UnmanagedType.LPStr)> nozzle As String) As Integer
    End Function
End Module

Dim raw As Integer = ReadPriceLiterLevel0("04")
If raw >= 0 Then
    Dim preco As Decimal = raw / 100D
End If
```

## Exemplo — Delphi

```pascal
function ReadPriceLiterLevel0(nozzle: PAnsiChar): Integer;
  stdcall; external 'companytec.dll';

var
  raw: Integer;
  preco: Currency;
begin
  raw := ReadPriceLiterLevel0(PAnsiChar('04'));
  if raw >= 0 then
    preco := raw / 100;
end;
```

## Isolamento de crash (recomendado)

DLL pode gerar Access Violation em casos extremos (bico inválido, estado inconsistente do concentrador, args malformados). AV no processo principal mata o host.

Padrão usado neste projeto: rodar DLL em processo worker out-of-process, comunicar via stdin/stdout JSON. Ver `Native/DllWorker.cs` e `Native/DllProxyClient.cs`. Crash do worker → restart automático, host sobrevive.

Mínimo: try/catch em `AccessViolationException` requer `[HandleProcessCorruptedStateExceptions]` + `legacyCorruptedStateExceptionsPolicy=true` no app.config. Não é confiável — preferir worker isolado.

## Checklist de implementação

- [ ] Projeto compilado x86 (PlatformTarget=x86)
- [ ] `companytec.dll` copiada ao output (e dependências, se houver)
- [ ] `C_OpenSocket2` ou `C_OpenSerial` chamado **antes** de `ReadPriceLiterLevel0`
- [ ] Bico passado como decimal 2-chars (`"01"`..`"32"`)
- [ ] Chamadas DLL serializadas em uma única thread
- [ ] Tratamento de retorno < 0 como falha
- [ ] Considerar worker out-of-process pra isolar crashes

## Referência neste repo

- Declaração P/Invoke: `Native/CompanytecDll.cs:123`
- Dispatch no worker: `Native/DllWorker.cs:158`
- Cliente do worker: `Native/DllProxyClient.cs:117`
- Uso no service: `Services/ConcentradorService.cs:228` (`LerPrecoDllPorBico`)
- Endpoint HTTP: `GET /api/concentrador/precos-dll/{bico}` — `Controllers/ConcentradorController.cs:117`
