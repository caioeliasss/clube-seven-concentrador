# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ASP.NET Core (.NET 10) HTTP bridge that fronts the Companytec concentrador gas-pump controller (`companytec.dll`). Single-file self-contained `win-x86` exe, REST API on port 5100, sits between a backend webhook receiver and the physical concentrador (serial or TCP).

The DLL is x86-only and Delphi-native — entire process MUST run x86.

## Common Commands

```bat
run-locally.bat        REM dev server, framework-dependent x86, http://localhost:5100
publicar.bat           REM publish framework-dependent x86 to clube-seven-concentrador-v0.0\
iniciar.bat            REM run published exe (sits next to it)
```

Raw:

```bat
dotnet run --project SevenConcentradorBridge.csproj -r win-x86 --self-contained false --urls "http://localhost:5100"
dotnet publish -c Release -r win-x86 --self-contained false -o <out>
```

No unit tests. `tests/*.bat` are curl harnesses against a running server — point them at a live exe. Each script sets `X-Api-Key: TROCAR_POR_CHAVE_SEGURA` (matches default `appsettings.json`).

## Architecture

Three layers, glued by a single-threaded queue and an out-of-process worker:

**1. HTTP layer** — `Program.cs` + `Controllers/ConcentradorController.cs`
- API key middleware (`X-Api-Key` header), `/api/concentrador/health` bypasses auth.
- Endpoints: `preset`, `status`, `abastecimento`, `visualizacao`, `liberar/bloquear/autorizar`, `precos`, `precos/{bico}`, `precos-dll`, `precos-dll/{bico}`, `native`, `health`.

**2. Service layer** — `Services/`
- `ConcentradorService` — serializes every DLL call through one `BlockingCollection<Action>` consumed by a dedicated thread (`DLL-Concentrador`). The native DLL is **not thread-safe**; never bypass `Executar(...)`.
- `PollingService` (`BackgroundService`) — connects on startup with retry, then every `Polling:IntervaloMs` polls `C_GetSale`, matches `canal` against bicos registered by `MonitorarBico` (called after a successful preset), POSTs `WebhookPayload` to `Backend:WebhookUrl`, then calls `C_NextSale`.

**3. Native isolation** — `Native/`
- `CompanytecDll.cs` — `[DllImport]` declarations for `companytec.dll`. Read the comments before adding entries — several exports (`C_SendReceiveText` returns Delphi `ShortString`, `LePPLNivel` takes managed `ansistring`) crash under naive P/Invoke.
- `DllWorker.cs` — when launched with `--worker`, becomes a stdin/stdout JSON-RPC server that dispatches to `CompanytecDll`. Logs to `%TEMP%\seven-dll-worker.log`.
- `DllProxyClient.cs` — spawns the same exe with `--worker` and pipes requests to it. **Reason for the split: any AV in the DLL kills the worker, not the API.** On crash, `Restart()` respawns and fires `OnWorkerRestarted` so `ConcentradorService` flips `_connected = false` and the next call reconnects.
- `Program.cs` checks `args.Contains("--worker")` before building the web host — the same binary serves both roles.

## Companytec protocol notes

- Bico IDs are decimal strings padded to 2 digits (`"01"`–`"32"`). Hex values crash the DLL (see `LerPrecosTodos`).
- `EnviarComando` builds the `>?` framed Companytec command with checksum; `EnviarNativo` sends a raw `(...)` Companytec command via the `SendReceiveText` export (the `ref IntPtr st` form — see `CompanytecDll.cs:147` and worker handler).
- Three preço paths exist: framed protocol (`LerPrecosTodos`), DLL helper `ReadPriceLiterLevel0` (returns price×100), and the raw `SendReceiveText` for ad-hoc commands. Don't unify them without reading `Services/ConcentradorService.cs` first — they each work around a different DLL quirk.
- Connection type is config-driven (`Concentrador:TipoConexao` = `ethernet` | `serial`).

## native-shim/

Optional Delphi/FPC shim (`companytec_shim.dpr`) that re-exports `LePPLNivel` with a C-friendly ABI. Build with FPC via `native-shim/build.bat`. Output `companytec_shim.dll` is copied to repo root and packaged by the csproj only if present. Not currently required — `ReadPriceLiterLevel0` covers the live preço-por-litro path.

## docs/

`docs/Fontes de exemplos/` contains the Companytec manual and protocol references (Markdown) plus original Delphi/VB.NET sample sources. Excluded from build (`csproj` removes `docs/**`). Consult before touching protocol framing or new DLL exports.
