library companytec_shim;

{
  Shim DLL para expor LePPLNivel da companytec.dll com ABI C-friendly.
  Motivo: LePPLNivel original recebe Delphi AnsiString (managed) e retorna
  record PPLNivel por valor — ABI incompatível com P/Invoke de C# usando LPStr.
  Este shim aceita PAnsiChar + 3 out doubles e converte internamente.

  Compilar com Free Pascal (modo Delphi, target Win32):
    fpc -Mdelphi -Twin32 -Pi386 companytec_shim.dpr
  Ou Delphi IDE: abrir .dpr, target Win32, Build.

  Bitness: deve casar com companytec.dll (x86) e processo .NET (x86).
}

{$MODE DELPHI}
{$LONGSTRINGS ON}

uses
  Windows,
  SysUtils;

type
  PPLNivel = record
    nivel0: Double;
    nivel1: Double;
    nivel2: Double;
  end;

  TLePPLNivel = function(bico: AnsiString; niveis: Integer): PPLNivel; stdcall;

var
  hCompanytec: HMODULE = 0;
  _LePPLNivel: TLePPLNivel = nil;

function C_LePPLNivel(bico: PAnsiChar; niveis: Integer;
  out n0, n1, n2: Double): Integer; stdcall;
var
  s: AnsiString;
  res: PPLNivel;
begin
  Result := 0;
  n0 := -1; n1 := -1; n2 := -1;

  if not Assigned(_LePPLNivel) then Exit;
  if bico = nil then Exit;

  try
    s := AnsiString(bico);
    res := _LePPLNivel(s, niveis);
    n0 := res.nivel0;
    n1 := res.nivel1;
    n2 := res.nivel2;
    if res.nivel0 = -1.0 then Exit;
    Result := 1;
  except
    Result := 0;
  end;
end;

procedure ShimAttach;
begin
  hCompanytec := LoadLibraryA('companytec.dll');
  if hCompanytec <> 0 then
    @_LePPLNivel := GetProcAddress(hCompanytec, 'LePPLNivel');
end;

procedure ShimDetach;
begin
  if hCompanytec <> 0 then
  begin
    FreeLibrary(hCompanytec);
    hCompanytec := 0;
    _LePPLNivel := nil;
  end;
end;

procedure DllMain(reason: DWORD);
begin
  case reason of
    DLL_PROCESS_ATTACH: ShimAttach;
    DLL_PROCESS_DETACH: ShimDetach;
  end;
end;

exports
  C_LePPLNivel;

begin
  DllProc := @DllMain;
  DllMain(DLL_PROCESS_ATTACH);
end.
