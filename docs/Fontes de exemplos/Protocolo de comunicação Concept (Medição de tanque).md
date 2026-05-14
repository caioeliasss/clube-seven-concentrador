

Protocolo de comunicação Concept



## INVENTÁRIO ATUAL
Código da Função: 201
Tipo da Função: Comando para reportar o Inventario do Tanque.
Formato do Comando: <SOH>i201TT
Resposta típica no Computador:
<SOH>i201TTYYMMDDHHmmTTpssssNNFFFFFFFF...TTpssssNNFFFFFFFF&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- TT – Numero do Tanque (Decimal, 00 = all)
- p – Código do Produto (um ASCII caractere [20h-7Eh])
- ssss - Tanque Status Bits: Bit 1 - (LSB) entrega em Progresso
Bit 2 – Teste de Estanqueidade em Progresso
Bit 3 - Invalid Fuel Height Alarm (MAG Probes Only)
## Bit 4-16 - Unused
- NN – Números de oito caracteres de dados a seguir (Hex)
- FFFFFFFF - ASCII Hex IEEE floats:
## 1. Volume
- TC Volume
## 3. Vazio
## 4. Altura
## 5. Agua
## 6. Temperatura
- Volume de agua
## 7. && - Data Terminação Flag
- CCCC – Checksum

## RELATÓRIO DE ENTREGAS NO TANQUE
Código da Função: 202
Tipo da Função: Comando para reportar o relatório de entregas no tanque.
Formato do Comando: <SOH>i202TT
Resposta típica no Computador:
<SOH>i202TTYYMMDDHHmmTTpddYYMMDDHHmmYYMMDDHHmmNNFFFFFFFF...
TTpddYYMMDDHHmmYYMMDDHHmmNNFFFFFFFF&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- TT – Numero do Tanque (Decimal, 00 = all)
- p – Código do Produto (um ASCII caractere [20h-7Eh])
- dd - Numero de entregas a seguir (Decimal, 00 se não haver data avaliada para o tanque)
- YYMMDDHHmm – Inicio da Data/Hora
- YYMMDDHHmm – Final da Data/Hora
- NN - Números de oito caracteres de dados a seguir (Hex)
- FFFFFFFF - ASCII Hex IEEE floats:
## 1. Volume Inicial
- TC Volume Inicial
## 3. Agua Inicial
## 4. Temperatura Inicial
## 5. Volume Final
- TC Volume Final
## 7. Agua Final
## 8. Temperatura Final
## 9. Altura Inicial
## 10. Altura Final
## 9. && - Data Terminação Flag
- CCCC – Checksum

## ENTREGA MAIS RECENTE DO TANQUE
Código da Função: 20C
Tipo da Função: Comando para reportar a entrega mais recente do tanque.
Formato do Comando: <SOH>i20CTT
Resposta típica no Computador:
<SOH>i20CTTYYMMDDHHmmTTpddYYMMDDHHmmYYMMDDHHmmNNFFFFFFFF...
TTpddYYMMDDHHmmYYMMDDHHmmNNFFFFFFFF&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- TT – Numero do Tanque (Decimal, 00 = all)
- p – Código do Produto (um ASCII caractere [20h-7Eh])
- dd - Numero de entregas a seguir (Decimal, 00 se não haver data avaliada para o tanque)
- YYMMDDHHmm – Inicio da Data/Hora
- YYMMDDHHmm – Final da Data/Hora
- NN - Números de oito caracteres de dados a seguir (Hex)
- FFFFFFFF - ASCII Hex IEEE floats:
## 1. Volume Inicial
- TC Volume Inicial
## 3. Agua Inicial
## 4. Temperatura Inicial
## 5. Volume Final
- TC Volume Final
## 7. Agua Final
## 8. Temperatura Final
## 9. Altura Inicial
## 10. Altura Final
## 18. && - Data Terminação Flag
- CCCC – Checksum







## ALARMES DA SONDA
Código da Função: 205
Tipo da Função: Comando para reportar Alarmes da Sonda.
Formato do Comando: <SOH>i205TT
Resposta típica no Computador:
<SOH>i205TTYYMMDDHHmmTTnnNN... TTnnNN&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- TT – Numero do Tanque (Decimal, 00 = all)
- p – Código do Produto (um ASCII caractere [20h-7Eh])
- nn - - Numero de alarmes no tanque (Decimal, 00 se não haver data avaliada para o tanque)
- NN – Numero do tipo de alarme:
Tabela de Equivalência:
## NN:
## 01 = Tanque Setup Dados Warning
## 02 = Tanque Leak Alarm
## 03 = Tanque High Water
## 04 = Tanque Overfill Alarm
## 05 = Tanque Baixo Produto
06 = Tanque Alarm perda súbita
07 = Tanque alta Produto
08 = Tanque inválido Nível de combustível Alarm
## 09 = Tanque Probe Out Alarm
## 10 = Tanque High Water Warning
11 = Tanque Abastecimento necessário Warning
12 = Tanque máxima do produto
## 13 = Tanque Gross Leak Test Falha Alarm
14 = Tanque periódica Teste de vazamento de alarme de falha de
## 15 = Anual Tanque Leak Test Falha Alarm
16 = Tanque periódica de teste necessária Warning
17 = tanque de teste anual necessário Warning
18 = Tanque periódica Teste de Alarme Necessário
## 27 = Tanque Fria Warning

## 6. && - Data Terminação Flag
- CCCC – Checksum

## RELATÓRIO DO HISTÓRICO DE ALARMES DA SONDA
Código da Função: 206
Tipo da Função: Comando para reportar histórico de alarmes.
Formato do Comando: <SOH>i206TT
Resposta típica no Computador:
<SOH>i206TTYYMMDDHHmmTTnnYYMMDDHHmmaaaa...
TTnnYYMMDDHHmmaaaa&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- TT – Numero do Tanque (Decimal, 00 = all)
- nn - Number of alarms in history for tank (Decimal, 00=none)
- YYMMDDHHmm - Date and time alarm occurred
- aaaa - Código do alarme
0002 - Alarme de Vazamento
0003 - Nível alto de água
0004 - Alarme de transbordo
0005 - Nível baixo de produto
0006 - Retirada Indevida de Produto
0007 - Nível alto de produto
0009 - Falha na sonda
000A - Alerta de água no tanque
000B - Entrega de produto requerida
000E - Teste de vazamento periódico
0014 - Teste de vazamento periódico Falhou
## 6. && - Data Terminação Flag
- CCCC – Checksum

## STATUS ATUAL DO SENSOR
Código da Função: 301
Tipo da Função: Comando para reportar o Status do sensor.
Formato do Comando: <SOH>i301TT
Resposta típica no Computador:
<SOH>i301SSYYMMDDHHmmSSssss...
SSssss&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- SS – Numero do Sensor (Decimal, 00=all)
- ssss – Valor do Status do Sensor:
## 0000 = Sensor Normal
0001=Configuração Incompleta do Sensor
0008= Alarme de Liquido no Sensor
## 4. && - Data Termination Flag
- CCCC – Checksum

## HISTÓRICO DE ALARMES NO SENSOR
Código da Função: 302
Tipo da Função: Comando para reportar o histórico de alarmes no sensor.
Formato do Comando: <SOH>i302TT
Resposta típica no Computador:
<SOH>i302SSYYMMDDHHmmSSNNYYMMDDHHmmaaaa...
SSNNYYMMDDHHmmaaaa&&CCCC<ETX>
Nota: 1. YYMMDDHHmm - Data e Hora Atuais
- SS – Numero do Sensor (Decimal, 00=all)
- NN – Número de alarmes a seguir
- YYMMDDHHmm - Data e Hora do Alarme
- aaaa – Código do alarme no sensor:
## 0000 = Sensor Normal
0001=Configuração Incompleta do Sensor
0008= Alarme de Liquido no Sensor
## 6. && - Data Terminação Flag
- CCCC - Checksum

Ponto flutuante padrão IEEE 754

- Rotina de conversão em linguagem C:

## #include <math.h>
unsigned int sinal, expoente, temp, i, binario;
float result,mantissa;
unsigned char buf[8]; // armazena os 8 dígitos hex recebidos
binario = 0;
for (i = 0; i < 8; i++) {
if((buf[i] >= '0') && (buf[i] <= '9')) temp = 0x30;
eles temp = 0x37;
binario = binario + ((buf[i] - temp) << (4*(7-i)));
## }
sinal = (binario >> 31) & 0x00000001;
expoente = ((binario >> 23) & 0x000000FF) - 127;
mantissa = float(binario & 0x007FFFFF) / 8.388608 / 1000000.0 + 1.0;
result = mantissa * pow(2.0,expoente);

- Rotina de conversão em linguagem Delphi:

procedure TForm1.Button2Click(Sender: TObject);
var temp,a:string;
S,E,M,man1,exp,res,man:double;
begin
a:=dectobin(strtoint('$' + edit1.text));
memo1.Lines.Add('A: ' + a);
## S:=strtoint(a[1]);
memo1.Lines.Add('S: ' + floattostr(S));
## E:=bintodec(copy(a,2,8));
memo1.Lines.Add('E: ' + floattostr(E));
## M:=bintodec(copy(a,10,23));
memo1.Lines.Add('M: ' + floattostr(M));
exp:=power(2,(E-127));
memo1.Lines.Add('Exp: ' + floattostr(exp));
man:=(M / 8.388608)/1000000;
man:=man + 1;
memo1.Lines.Add('Man: ' + floattostr(man));
memo1.Lines.Add('Man1: ' + floattostr(man1));
res:=exp*man;
if S=1 then res:=res*-1;
memo1.Lines.Add('Res: ' + floattostr(res));
end;