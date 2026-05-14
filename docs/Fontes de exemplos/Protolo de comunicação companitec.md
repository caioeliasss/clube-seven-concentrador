



Protocolo de comunicação
## Companytec
Protocolo de
comunicaça o
## Companytec
## DT 435


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 2 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## 1 INTRODUÇÃO .............................................. 3
## 2 ESTRUTURA DO COMANDO ......................... 3
## 3 COMANDOS ................................................. 3
## 3.1 ABASTECIMENTO ........................................ 3
3.1.1 Leitura de Abastecimento (52) ...... 3
3.1.2 Leitura de abastecimento (34) ...... 4
3.1.3 Leitura de abastecimento
identificado .................................................... 5
3.1.4 Leitura    de    abastecimento    com
dupla identificação ......................................... 6
3.1.5 Leitura de abastecimento PAF1 .... 7
3.1.6 Leitura de abastecimento PAF2 .... 8
3.1.7 Leitura de abastecimento estendido
com    dados    MQTT/informações    adicionais
## TWC................................................................. 9
3.1.8 Leitura de ponteiro do
abastecimento ............................................. 11
3.1.9 Leitura de registro de
abastecimento ............................................. 11
3.1.10 Leitura  dos  ponteiros  da  memória
(escrita e leitura) .......................................... 13
## 3.1.11 Incremento .................................. 14
## 3.2 VISUALIZAÇÃO ......................................... 14
## 3.2.1 Visualização ................................ 14
3.2.2 Visualização identificada ............. 14
## 3.3 IDENTFID................................................. 15
3.3.1 Leitura de identificador ............... 15
3.3.2 Gravação de identificadores ....... 16
3.3.3 Exclusão de identificador ............ 17
3.3.4 Incremento de identificador ........ 18
3.3.5 Limpeza de memória de
identificadores ............................................. 18
3.3.6 Leitura de identificadores do
abastecimento ............................................. 19
3.3.7 Leitura  de  registro  de  identificador
na memória .................................................. 19
3.3.8 Comandos de lista negra ............. 20
## 3.4 STATUS ................................................... 21
## 3.4.1 Status .......................................... 21
## 3.5 GERENCIAMENTO DE BOMBA ...................... 23
3.5.1 Leitura de totalizadores .............. 23
3.5.2 Alteração de preço ...................... 25
3.5.3 Alteração de preço estendido...... 26
3.5.4 Leitura de preços ......................... 27
3.5.5 Predeterminação de valor
(Preset)...... ................................................... 28
3.5.6 Predeterminação identificada
(Preset identificado) ..................................... 29
3.5.7 Modo de operação ...................... 30
## 3.6 RELÓGIO ................................................. 31
3.6.1 Leitura de calendário................... 31
3.6.2 Leitura de relógio estendido ........ 31
3.6.3 Ajuste de calendário .................... 32
3.6.4 Ajuste de calendário estendido ... 33
## 3.7 CÓDIGOS DE VÍRGULA ................................ 34
## 3.8 CÓDIGOS DE STATUS ................................. 34
## 3.9 CÓDIGOS DE MODO................................... 35
## 3.10 CÓDIGOS DE CONTROLE PARA
## IDENTIFICADORES...... ........................................... 35
## 3.10.1 Permissões................................... 35
## 3.11 CÁLCULO DE CHECKSUM ............................ 36






Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 3 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## 1 Introdução
- Todos os comandos são iniciados com o caractere “(“, seguido do cabeçalho do comando
e posteriormente os parâmetros, sendo finalizado com o caractere “)”.
- Campos denominados  “Bomba”  são  representados  em  decimal  na  faixa  de  1  a  4,
representado em um caractere em qualquer comando/resposta;
- Campos “bico” são representados em hexadecimal;
2 Estrutura do comando
Todos  os  comandos  são  definidos  como  a  figura  abaixo,  eles  consistem  em limitadores,
cabeçalho, parâmetros e um checksum, respectivamente.
## (CCPP...KK)
- Limitador inicial = (
- Cabeçalho = CC
- Parâmetros = PP
- Checksum  =  KK  ( somatório dos  valores  ACSII  dos  caracteres  do  comando,  sendo
desprezado o byte mais significativo)
- Limitador final = )
## 3 Comandos
## 3.1 Abastecimento
3.1.1 Leitura de Abastecimento (52)
Este comando é utilizado para a leitura de abastecimento finalizado.
- Cabeçalho: &A
- Estrutura do comando:
## (&A)
➢ &A [02]: Cabeçalho do comando.....................................................(hexadecimal)
- Compatibilidade: CBC03 / 04 e 05 (dip switch 3 ligada), CBC06 e Horustech.
- Resposta: Este  comando  retornará  até  52  caracteres,  conforme demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
## (TTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNRRRREEEEEEEEEESSKK)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 4 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ N [02]: Mês do abastecimento................................................................(decimal)
➢ R [04]: Registro do abastecimento na memória......................................(decimal)
➢ E [10]: Totalizador final do abastecimento..............................................(decimal)
➢ S [02]: Status de integridade da memória (00=Ok)................................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&A)
## RX: (00038600386010003E001C0812164002068300000807890098)
3.1.2 Leitura de abastecimento (34)
Este comando é utilizado para a leitura de abastecimento finalizado.
- Cabeçalho: &A
- Estrutura do comando:
## (&A)
➢ &A [02]: Cabeçalho do comando.....................................................(hexadecimal)
- Compatibilidade: IMS01/ 02, CBC01 e CBC03 / 04 e 05 (dip switch 3 ligada).
- Resposta: Este  comando  retornará  até  34  caracteres,  conforme  demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
## (TTTTTTLLLLLLPPPPVVCCCCBBDDHHMMKK)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 5 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.1.3 Leitura de abastecimento identificado
Este comando é utilizado para a leitura de abastecimento com o código do identificador que
realizou a liberação do bico.
- Cabeçalho: &A
- Estrutura do comando:
## (&AKK)
➢ &A [02]: Cabeçalho do comando.....................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este  comando  retornará  até  75  caracteres,  conforme  demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
## (ATTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNRRRREEEEEEEEEESSIIIIIIIIIIIIIIII
## MMMMPPKK)
➢ A [01]: Cabeçalho do comando.............................................................(caractere)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ N [02]: Mês do abastecimento................................................................(decimal)
➢ R [04]: Registro do abastecimento na memória......................................(decimal)
➢ E [10]: Totalizador final do bico...............................................................(decimal)
➢ S [02]: Status de integridade da memória (00=Ok)................................(caractere)
➢ I [16]: Identificador..........................................................................(hexadecimal)
➢ M [04]: Número da leitura de identificação.............................................(decimal)
➢ P [02]: Status de integridade da memória de identificadores (00=Ok)..(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&A67)
## RX:(A00038600386010003E001C08121640020683000008078900B3CF6CCFFF1FD792
## 068300F3)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 6 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.1.4 Leitura de abastecimento com dupla identificação
Este comando é utilizado para a leitura de abastecimento com o código dos identificadores.
## • Cabeçalho: &@
- Estrutura do comando:
## (&@KK)
➢ &@ [02]: Cabeçalho do comando....................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este  comando  retornará  até  87  caracteres,  conforme  demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
(@TTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNRRRREEEEEEEEEEssIIIIIIIIIIIIIIIIiii
iiiiiiiiiiiiiSSKK)
➢ @ [01]: Cabeçalho do comando............................................................(caractere)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ N [02]: Mês do abastecimento................................................................(decimal)
➢ R [04]: Registro do abastecimento na memória......................................(decimal)
➢ E [10]: Totalizador final do bico...............................................................(decimal)
➢ S [02]: Status de integridade da memória (00=Ok)................................(caractere)
➢ I [16]: Identificador 1.......................................................................(hexadecimal)
➢ i [16]: Identificador 2.......................................................................(hexadecimal)
➢ P [02]: Status de integridade da memória de identificadores (00=Ok)..(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&@66)
## RX:(@00038600386010003E001C08121640020683000008078900B3CF6CCFFF1FD79
## 2FFFFFFFFFFFFFFFF0081)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 7 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.1.5 Leitura de abastecimento PAF1
Este comando é utilizado para a leitura de abastecimentos com a informação de frentista e
cliente.
- Cabeçalho: &A2
- Estrutura do comando:
## (&A2KK)
➢ &A2 [03]: Cabeçalho do comando...................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará até 123 caracteres, conforme demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
(a2$SSSSSSSSTTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNAARRRRRRffffffffffiiiiiiiiiibbcc
ttvvvvvvvvIIIIIIIIIIIIIIIIJJJJJJJJJJJJJJJJnnKK)
➢ a2 [02]: Cabeçalho do comando...........................................................(caractere)
➢ $ [01]: Caractere de habilitação............................................................(caractere)
➢ S [08]: Número de série da automação....................................................(decimal)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ N [02]: Mês do abastecimento................................................................(decimal)
➢ A [02]: Ano do abastecimento.................................................................(decimal)
➢ R [06]: Registro do abastecimento na memória......................................(decimal)
➢ f [10]: Totalizador final do bico...............................................................(decimal)
➢ i [10]: Totalizador inicial do bico..............................................................(decimal)
➢ b [02]: Número do bico na pista*.............................................................(decimal)
➢ c [02]: Tipo de combustível*....................................................................(decimal)
➢ t [02]: Número do tanque*......................................................................(decimal)
➢ v [08]: Volume do tanque**....................................................................(decimal)
➢ I [16]: Identificador de frentista.......................................................(hexadecimal)
➢ i [16]: Identificador de cliente..........................................................(hexadecimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 8 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ N [02]: Status de integridade da memória (00=Ok)...............................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&A299)
RX:(a2FFFFFFFFF00038600386010003E001C081216400220000683000008078900000
## 8040303000000000000B3CF6CCFFF1FD792FFFFFFFFFFFFFFFF003E)
## Observações:
- Campos que necessitam de configuração correta;
** Válido quando implementada a comunicação com medidor de tanque.
3.1.6 Leitura de abastecimento PAF2
Este comando é utilizado para a leitura de abastecimentos com a informação de frentista e
cliente com precisão de segundos e informação do dia da semana.
- Cabeçalho: &A3
- Estrutura do comando:
## (&A3KK)
➢ &A3 [03]: Cabeçalho do comando...................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 (versão maior que 5.3) e Horustech (versão maior que 6.7).
- Resposta: Este comando retornará até 127 caracteres, conforme demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
(a3$SSSSSSSSTTTTTTLLLLLLPPPPVVCCCCBBAAmmDDHHMMSSRRRRRRFFFFFFFFFFIIIIII
IIIIbbccttVVVVVVVVIIIIIIIIIIIIIIIIJJJJJJJJJJJJJJJJRRiiKK)
➢ a3 [02]: Cabeçalho do comando...........................................................(caractere)
➢ $ [01]: Caractere de habilitação............................................................(caractere)
➢ S [08]: Número de série da automação....................................................(decimal)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ A [02]: Ano do abastecimento.................................................................(decimal)
➢ M [02]: Mês do abastecimento...............................................................(decimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 9 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ S [02]: Segundo do abastecimento..........................................................(decimal)
➢ R [06]: Registro do abastecimento na memória......................................(decimal)
➢ F [10]: Totalizador final do bico...............................................................(decimal)
➢ I [10]: Totalizador inicial do bico..............................................................(decimal)
➢ b [02]: Número do bico na pista*.............................................................(decimal)
➢ c [02]: Tipo de combustível*....................................................................(decimal)
➢ t [02]: Número do tanque*......................................................................(decimal)
➢ V [08]: Volume do tanque**....................................................................(decimal)
➢ I [16]: Identificador de frentista.......................................................(hexadecimal)
➢ J [16]: Identificador de cliente.........................................................(hexadecimal)
➢ R [02]: Reservado....................................................................................(decimal)
➢ i [02]: Status de integridade da memória (00=Ok)...............................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&A39A)
RX:(a3FFFFFFFFF00038600386010003E001C082002121640100006830000080789000
## 008040303000000000000B3CF6CCFFF1FD792FFFFFFFFFFFFFFFF000000)
## Observações:
- Campos que necessitam de configuração correta;
** Válido quando implementada a comunicação com medidor de tanque.
3.1.7 Leitura  de  abastecimento  estendido  com  dados  MQTT/informações
adicionais TWC
Este  comando  é  utilizado para  a  leitura  de  abastecimentos  com  a  informação de  frentista,
cliente, dados MQTT com precisão de segundos e informação do dia da semana.
- Cabeçalho: &A4
- Estrutura do comando:
## (&A4KK)
➢ &A4 [03]: Cabeçalho do comando...................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: Horustech (versão maior que 8.0).
- Resposta: Este comando retornará até 149 caracteres, conforme demonstração abaixo
ou (0) se nenhum abastecimento na memória.
- Estrutura da resposta:
(a4$SSSSSSSSTTTTTTTTLLLLLLLLPPPPPPVVCCCCBBAAmmDDHHMMSSRRRRRRFFFFFFF
FFFIIIIIIIIIIbbccttVVVVVVVVIIIIIIIIIIIIIIIIJJJJJJJJJJJJJJJJaaaaarrrrrpppppRRiiKK)
➢ a4 [02]: Cabeçalho do comando...........................................................(caractere)
➢ $ [01]: Caractere de habilitação............................................................(caractere)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 10 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ S [08]: Número de série da automação....................................................(decimal)
➢ T [08]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [08]: Volume abastecido.......................................................................(decimal)
➢ P [06]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ A [02]: Ano do abastecimento.................................................................(decimal)
➢ M [02]: Mês do abastecimento...............................................................(decimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ S [02]: Segundo do abastecimento..........................................................(decimal)
➢ R [06]: Registro do abastecimento na memória......................................(decimal)
➢ F [10]: Totalizador final do bico...............................................................(decimal)
➢ I [10]: Totalizador inicial do bico..............................................................(decimal)
➢ b [02]: Número do bico na pista*.............................................................(decimal)
➢ c [02]: Tipo de combustível*....................................................................(decimal)
➢ t [02]: Número do tanque*......................................................................(decimal)
➢ V [08]: Volume do tanque**....................................................................(decimal)
➢ I [16]: Identificador de frentista.......................................................(hexadecimal)
➢ J [16]: Identificador de cliente.........................................................(hexadecimal)
➢ a [05]: Authorization ID/Info adc 1..........................................................(decimal)
➢ r [05]: Reserve ID/Info adc 2....................................................................(decimal)
➢ p [05]: Provider (brand)/Info adc 3..........................................................(decimal)
➢ R [02]: Nivel de preço.......................................................................(hexadecimal)
➢ i [02]: Status de integridade da memória (00=Ok)...............................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&A49B)
RX:(a440002224600000000000000000001102E00114D250623012739000327000000
## 2393020023937124000012345678B328000000000001B32800000000000212345321
## 56001017E0085)
## Observações:
- Campos que necessitam de configuração correta;
** Válido quando implementada a comunicação com medidor de tanque.
O campo de nível de preço se dará no bit 6 e 7 do código de vírgula, então:
▪ 2A/2E ou 3A/3E: nível 0


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 11 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

▪ 6A/6E ou 7A/7E: nível 1
▪ AA/AE ou BA/BE: nível 2
3.1.8 Leitura de ponteiro do abastecimento
Este  comando  é  utilizado  para  retornar  o  abastecimento,  passando  como  parâmetro,  seu
registro na memória.
- Cabeçalho: &L
- Estrutura do comando:
## (&LCXXXXKK)
➢ &L [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ C [01]: Modo da leitura........................................................................(caractere)
➢ X [04]: Número do registro na memória..................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC04 / 05 / 06 e Horustech.
- Resposta: A resposta deste comando vem no formato da leitura de abastecimento PAF1.
Quando não há abastecimento a ser lido nesta posição, a automação responde a string
com FF.
## • Exemplos:
▪ Resposta de uma automação CBC06
## TX: (&LC000378)
## RX:(0000073502457202993A0016041908340400030007380542000000000000000000
## 00030000000000000000B6)
▪ Resposta de uma automação Horustech
## TX: (&LC000378)
RX:(a240002224600150600031381363E001E051720140422000001005790041800579
## 0038702000000000000000000000000000000000000000000000052)
- Exemplo de posição sem abastecimento:
## TX: (&LC50007A)
RX:(a2FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00FFFFFFFFFFFFFFFFFFFFFFFFFF
## FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF013A)
3.1.9 Leitura de registro de abastecimento
Este comando  é  utilizado  para  ler  um  abastecimento  de  uma  determinada  posição  na
memória da automação.
- Cabeçalho: &L
- Estrutura do comando:
## (&LRXXXXKK)
➢ &L [02]: Cabeçalho do comando......................................................(hexadecimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 12 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ R [01]: Modo da leitura.........................................................................(caractere)
➢ X [04]: Posição a ser consultada...............................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC04 / 05 / 06 e Horustech.
- Resposta: Este  comando  retornará até  82  caracteres em  automações  Horustech  e  74
caracteres em automações CBC04/05/06, conforme demonstração abaixo. Quando não
há abastecimento a ser lido nesta posição, a automação responde a string com FF.
- Estrutura da resposta CBC04/05/06:
## (TTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNRRRREEEEEEEEEEFFIIIIIIIIIIIIIIIINNNNSSKK)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)
➢ N [02]: Mês do abastecimento................................................................(decimal)
➢ R [04]: Registro do abastecimento na memória......................................(decimal)
➢ E [10]: Totalizador final do bico...............................................................(decimal)
➢ F [02]: Campo fixo...................................................................................(decimal)
➢ I [16]: Identificador..........................................................................(hexadecimal)
➢ N [04]: Número do registro lido...............................................................(decimal)
➢ S [02]: Status de integridade da memória (00=Ok)...............................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Estrutura da resposta Horustech:
## (TTTTTTLLLLLLPPPPVVCCCCBBDDHHMMNNRRRREEEEEEEEEEFFIIIIIIIIIIIIIIIINNNNXXXX
## XXXXSSKK)
➢ T [06]: Total a pagar (bombas mecânicas retornam “000000”)...............(decimal)
➢ L [06]: Volume abastecido.......................................................................(decimal)
➢ P [04]: Preço unitário do abastecimento.................................................(decimal)
➢ V [02]: Código de vírgula (tabela ao final do documento)................(hexadecimal)
➢ C [04]: Tempo do abastecimento.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia do abastecimento..................................................................(decimal)
➢ H [02]: Hora do abastecimento...............................................................(decimal)
➢ M [02]: Minuto do abastecimento...........................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 13 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ N [02]: Mês do abastecimento................................................................(decimal)
➢ R [04]: Registro do abastecimento na memória......................................(decimal)
➢ E [10]: Totalizador final do bico...............................................................(decimal)
➢ F [02]: Campo fixo...................................................................................(decimal)
➢ I [16]: Identificador..........................................................................(hexadecimal)
➢ N [04]: Campo fixo...................................................................................(decimal)
➢ X [08]: Início do identificador de cliente...........................................(hexadecimal)
➢ S [02]: Status de integridade da memória (00=Ok)...............................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Exemplo de leitura em automações CBC06:
## TX: (&LR000185)
## RX:(23429602364099113A002C11261741070001000007525600B328090961C1FEE400
## 01004F)
- Exemplo de leitura em automações Horustech:
## TX: (&LR000185)
## RX:(00010000081012343E000204290941019140000002864100B32812345678000100
## 00B3281234007C)
- Exemplo de posição sem abastecimento:
## TX: (&LR100085)
## RX:(FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
## FFF0129)
3.1.10 Leitura dos ponteiros da memória (escrita e leitura)
Este comando é utilizado para ler o ponteiro de leitura e de escrita dos abastecimentos na
memória da automação.
- Cabeçalho: &T99
- Estrutura do comando:
## (&T99PKK)
➢ &T99 [04]: Cabeçalho do comando..................................................(hexadecimal)
➢ P [01]: Indicação de ponteiro................................................................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará a localização dos ponteiros de leitura e escrita dos
abastecimentos na memória da automação.
- Estrutura da resposta:
## (TP99XXXXYYYYKK)
➢ TP99 [04]: Cabeçalho do comando..................................................(hexadecimal)
➢ X [04]: Posição do ponteiro de escrita.....................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 14 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ Y [04]: Posição do ponteiro de leitura......................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&T99P3C)
## RX: (TP9907000683AE)
## 3.1.11 Incremento
Este comando é utilizado para passar o ponteiro de leitura para o próximo abastecimento.
- Cabeçalho: &I
- Estrutura do comando:
## (&I)
➢ &I [02]: Cabeçalho do comando.......................................................(hexadecimal)
- Compatibilidade: IMS01 / 02, CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando não retorna nenhum parâmetro.
## 3.2 Visualização
## 3.2.1 Visualização
Este comando é utilizado para leitura dos abastecimentos em andamento.
- Cabeçalho: &V
- Estrutura do comando:
## (&V)
➢ &V [02]: Cabeçalho do comando.....................................................(hexadecimal)
- Compatibilidade: IMS01 / 02, CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará o código do bico juntamente com o valor ou volume
do abastecimento que está sendo realizado. Para cada bico irá retornar a estrutura de
resposta informada. Quando não houver abastecimentos em andamento, a automação
retornará (0).
- Estrutura da resposta:
## (BBTTTTTT)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ T [06]: Valor ou volume do abastecimento..............................................(decimal)
## • Exemplo:
## TX: (&V)
## RX: (04000120)
3.2.2 Visualização identificada
Este comando é utilizado para verificar o identificador que liberou o bico do abastecimento
que está em andamento.


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 15 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- Cabeçalho: ?V
- Estrutura do comando:
## (?VKK)
➢ ?V [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará o código do bico juntamente com o identificador do
abastecimento  que  está  sendo  realizado.  Para  cada  bico  irá  retornar  a  estrutura  de
resposta informada. Quando não houver abastecimentos em andamento , a automação
retornará (0).
- Estrutura da resposta:
## (BBIIIIIIIIIIIIIIII)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ I [16]: Identificador..........................................................................(hexadecimal)
## • Exemplo:
## TX: (?V95)
## RX: (04B328000000000001)
## 3.3 Identfid
3.3.1 Leitura de identificador
Este comando é utilizado para realizar a leitura do identificador que não está cadastrado na
memória  da  automação.  Neste  caso  a  automação  irá  enviar  o  identificador  ao  computador e
este por sua vez, tomará a providência necessária.
- Cabeçalho: ?A
- Estrutura do comando:
## (?AKK)
➢ ?A [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará o código do identificador lido. Quando não houver
nenhum identificador a ser lido, a automação retornará (0).
- Estrutura da resposta:
## (ACCCCCCCCCCCCCCCCBBDDHHMMNNRRRRSSKK)
➢ A [01]: Cabeçalho do comando.............................................................(caractere)
➢ C [16]: Código do identificador........................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ D [02]: Dia................................................................................................(decimal)
➢ H [02]: Hora.............................................................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 16 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ M [02]: Minuto........................................................................................(decimal)
➢ N [02]: Mês..............................................................................................(decimal)
➢ R [04]: Número do registro da leitura......................................................(decimal)
➢ S [02]: Status de integridade da memória (00=Ok)................................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (?A80)
## RX: (AB3CF6CCFFF1F51E6051713450200000041)
3.3.2 Gravação de identificadores
Este comando é utilizado para gravar códigos de identificadores na memória da automação,
após este  procedimento, os cartões  reconhecidos pelo equipamento, podem liberar qualquer
bico para abastecimento sem intervenção do computador, sendo assim, as liberações passam a
ser controladas pela automação.
- Cabeçalho: ?F
- Estrutura do comando:
(?FCCGTTTTTTTTTTTTTTTTAAAAaaaaBBBBbbbbKK)
➢ ?F [02]: Cabeçalho do comando.......................................................(hexadecimal)
➢ C [02]: Controle (tabela ao final do documento)..............................(hexadecimal)
➢ G [01]: Parâmetro para gravação..........................................................(caractere)
➢ T [16]: Código do identificador........................................................(hexadecimal)
➢ A [04]: Turno inicial A (hhmm).................................................................(decimal)
➢ a [04]: Turno final A (hhmm)....................................................................(decimal)
➢ B [04]: Turno inicial B (hhmm).................................................................(decimal)
➢ b [04]: Turno final B (hhmm)....................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará o código do identificador gravado assim como suas
características.
- Estrutura da resposta:
(FGPPPPPPMMMMMMTTTTTTTTTTTTTTTTAAAAaaaaBBBBbbbbCCKK)
➢ F [01]: Cabeçalho do comando..............................................................(caractere)
➢ G [01]: Parâmetro de gravação.............................................................(caractere)
➢ P [06]: Posição onde foi gravado o identificador......................................(decimal)
➢ M [06]: Quantidade de identificadores na memória................................(decimal)
➢ T [16]: Código do identificador................................................................(decimal)
➢ A [04]: Turno inicial A (hhmm).................................................................(decimal)
➢ a [04]: Turno final A (hhmm)....................................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 17 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ B [04]: Turno inicial B (hhmm).................................................................(decimal)
➢ b [04]: Turno final B (hhmm)....................................................................(decimal)
➢ C [02]: Controle (tabela ao final do documento)..............................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (?F27GB32800000000012300000000000000005A)
## RX: (FG000000000001B3280000000001230000000000000000275C)
3.3.3 Exclusão de identificador
Este   comando   é   utilizado   para   apagar   um   registro   de   identificador   na  memória   da
automação.
- Cabeçalho: ?F
- Estrutura do comando:
## (?FCCATTTTTTTTTTTTTTTT00RRRRRRLLLLLLLLKK)
➢ ?F [02]: Cabeçalho do comando.......................................................(hexadecimal)
➢ C [02]: Controle (tabela ao final do documento)..............................(hexadecimal)
➢ A [01]: Parâmetro para exclusão...........................................................(caractere)
➢ T [16]: Código do identificador........................................................(hexadecimal)
➢ 0 [02]: Campo fixo.................................................................................(caractere)
➢ R [06]: Posição do registro (0: campo fixo)...............................................(decimal)
➢ L [08]: Campo fixo (preencher com “0”)..................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará as informações da exclusão do identificador. Quando
houver  falha  na  exclusão  a  automação  retornará  000000  no  campo  de  posição  do
identificador excluído.
- Estrutura da resposta:
(FAXXXXXXSSSSSSCCCCCCCCCCCCCCCCAAAAaaaaBBBBbbbbXXKK)
➢ F [01]: Cabeçalho do comando..............................................................(caractere)
➢ A [01]: Parâmetro de exclusão..............................................................(caractere)
➢ X [06]: Posição do identificador excluído.................................................(decimal)
o 00000X: Excluído com sucesso;
o 000000: Falha na exclusão.
➢ S [06]: Número de registro requerido......................................................(decimal)
➢ C [16]: Campo fixo...................................................................................(decimal)
➢ A [04]: Turno inicial A (hhmm).................................................................(decimal)
➢ a [04]: Turno final A (hhmm)....................................................................(decimal)
➢ B [04]: Turno inicial B (hhmm).................................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 18 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ b [04]: Turno final B (hhmm)....................................................................(decimal)
➢ C [02]: Controle (tabela ao final do documento)..............................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Exemplo de exclusão realizada com sucesso:
## TX: (?F27AB328000000000005000000050000000058)
## RX: (FA000005000005000000000000000000000000000000000031)
- Exemplo de erro na exclusão:
## TX: (?F27AB328000000000005000000050000000058)
## RX: (FA00000000000500000000000000000000000000000000002C)
3.3.4 Incremento de identificador
Este comando é utilizado para incrementar o ponteiro de leitura para o próximo identificador
lido.
- Cabeçalho: ?I
- Estrutura do comando:
## (?IKK)
➢ ?I [02]: Cabeçalho do comando.......................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando não retorna nenhum parâmetro.
3.3.5 Limpeza de memória de identificadores
Este comando é utilizado para apagar todos os identificadores da memória da automação. O
comando apresentado abaixo está correto para a exclusão, então, para enviá-lo não é necessário
alterar nenhum parâmetro.
- Cabeçalho: ?F
- Estrutura do comando:
## (?F00L00000000000000000000000100000000KK)
➢ ?F [02]: Cabeçalho do comando.......................................................(hexadecimal)
➢ 0 [02]: Parâmetro fixo.............................................................................(decimal)
➢ L [01]: Parâmetro para limpeza da memória.........................................(caractere)
➢ As próximas 32 posições são fixas...........................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando não retorna nenhum parâmetro.


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 19 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.3.6 Leitura de identificadores do abastecimento
Este   comando   é   utilizado   para   ler   qual   ou   quais   identificadores   responsáveis   pelo
abastecimento de um determinado bico.
- Cabeçalho: &T
- Estrutura do comando:
## (&TBBIKK)
➢ &T [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ I [01]: Caractere de identificador..........................................................(caractere)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 (acima da versão 2.3) e Horustech.
- Resposta: Este comando retornará os identificadores responsável pelo abastecimento.
- Estrutura da resposta:
## (TIBBFFFFFFFFFFFFFFFFCCCCCCCCCCCCCCCCKK)
➢ T [01]: Cabeçalho do comando.............................................................(caractere)
➢ I [01]: Caractere de identificador..........................................................(caractere)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ F [16]: Identificador do frentista......................................................(hexadecimal)
➢ C [16]: Identificador do cliente........................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
3.3.7 Leitura de registro de identificador na memória
Este   comando   é   utilizado   para   ler   os   identificadores   armazenados   na   memória   da
automação.
- Cabeçalho: ?LF
- Estrutura do comando:
## (?LFPPPPPPKK)
➢ ?LF [03]: Cabeçalho do comando.....................................................(hexadecimal)
➢ P [06]: Posição da memória.....................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará as informações da exclusão do identificador.
- Estrutura da resposta:
(LFPPPPPPLLLLLLTTTTTTTTTTTTTTTTAAAAaaaaBBBBbbbbCCKK)
➢ LF [02]: Cabeçalho do comando............................................................(caractere)
➢ P [06]: Posição lida do identificador........................................................(decimal)
➢ L [06]: Quantidade de identificadores na memória.................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 20 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ T [16]: Código do identificador........................................................(hexadecimal)
➢ A [04]: Turno inicial A (hhmm).................................................................(decimal)
➢ a [04]: Turno final A (hhmm)....................................................................(decimal)
➢ B [04]: Turno inicial B (hhmm).................................................................(decimal)
➢ b [04]: Turno final B (hhmm)....................................................................(decimal)
➢ C [02]: Controle (tabela ao final do documento)..............................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Exemplo de posição com identificador:
## TX: (?LF000002F3)
## RX: (LF000002000010B32800000000000227000000000000002768)
- Exemplo de posição sem identificador:
## TX: (?LF000002F3)
## RX: (LF000002000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF20)
3.3.8 Comandos de lista negra
Este comando é utilizado para colocar ou retirar cartões  identfid de uma lista que  faz com
que o console não os reconheça temporariamente, ou seja, quando um cartão identfid está na
lista  negra,  ele  não  pode  liberar  um  abastecimento,  assim  este  cartão  é  remetido  a  lista  de
cartões  pendentes  para  o  sistema  tomar  a  decisão  em  relação  ao  cartão.  Esta  lista  possuí  20
posições.
- Cabeçalho: &M99
- Estrutura do comando:
## (&M99BIIIIIIIIIIIIIIIIKK)
➢ &M99 [04]: Cabeçalho do comando................................................(hexadecimal)
➢ B [01]: Modo do comando...............................................................(hexadecimal)
o c:   limpa   lista,   neste   parâmetro   não   é   necessário   colocar   código   do
identificador;
o b: coloca identificador na lista negra;
o l: retira identificador da lista negra.
➢ I [16]: Código do identificador.........................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará o código de bico ou código de erro.
- Estrutura da resposta:
▪ Comando aceito:
## (MBB)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 21 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

▪ Lista de cartões cheia:
(M?f)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?f [02]: Código de erro..........................................................................(caractere)
## • Exemplos:
▪ Limpeza de lista negra:
TX: (&M99c48)
## RX: (M99)
▪ Colocar identificador na lista:
TX: (&M99bB32800000000000167)
## RX: (M99)
▪ Retirar identificador da lista:
TX: (&M99lB32800000000000171)
## RX: (M99)
## 3.4 Status
## 3.4.1 Status
Este  comando é  utilizado para  ler  as  informações  de  estado  de  cada  bico  no momento  da
requisição.
- Cabeçalho: &S
- Estrutura do comando:
## (&S)
➢ &S [02]: Cabeçalho do comando......................................................(hexadecimal)
- Compatibilidade: CBC03 / 04 / 05 / 06.
- Resposta: Este comando retornará as informações de cada endereço da automação.
- Estrutura da resposta:
## (SXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXFFDDCVVVVMMMMPTT)
➢ S [01]: Cabeçalho do comando..............................................................(caractere)
➢ X [32]: Status do endereço....................................................................(caractere)
➢ F [02]: Fixo (uso futuro).........................................................................(caractere)
➢ D [02]: Estado das dip swithcs (somente em modo virtual)...................(caractere)
➢ C [01]: Tipo de cbc...................................................................................(decimal)
o 4: CBC04;
o 5: CBC05;
o 6: CBC06.
➢ V [04]: Versão de firmware...................................................................(caractere)
➢ M [04]: Versão do software monitor.....................................................(caractere)
➢ P [01]: Status da rede AC.......................................................................(caractere)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 22 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

o G: rede normal;
o B: sem rede externa.
➢ T [02]: Tensão da bateria.................................................................(hexadecimal)
## • Exemplo:
## TX: (&S)
## RX: (SBBFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF6V8.4M1.0G84)
Atenção:  Caso a automação seja uma Horustech e o protocolo ativo seja o CBC, o
comando terá a resposta com os status de 9 canais da automação e será da seguinte forma.
- Estrutura da resposta:
## (SXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXCVVVVMMMMPTT)
➢ S [01]: Cabeçalho do comando..............................................................(caractere)
➢ X [36]: Status do endereço....................................................................(caractere)
➢ C [01]: Tipo de cbc (protocolo ativado, neste caso CBC06)......................(decimal)
➢ V [04]: Versão de firmware...................................................................(caractere)
➢ M [04]: Versão do software monitor.....................................................(caractere)
➢ P [01]: Status da rede AC.......................................................................(caractere)
o G: rede normal;
o B: sem rede externa.
➢ T [02]: Tensão da bateria.................................................................(hexadecimal)
## • Exemplo:
## TX: (&S)
## RX: (SBBFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF6V8.4M1.0G84)
Atenção: Caso  a  automação  seja  uma  Horustech  e  o  protocolo  ativo  seja  o
Companytec, o comando terá a resposta com os status dos 12 canais da automação e será da
seguinte forma:
- Estrutura da resposta:
## (SXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX)
➢ S [01]: Cabeçalho do comando..............................................................(caractere)
➢ X [48]: Status do endereço....................................................................(caractere)
## • Exemplo:
## TX: (&S)
## RX: (SBBFFABFFLLFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 23 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.5 Gerenciamento de bomba
3.5.1 Leitura de totalizadores
Este  comando  é  utilizado  para  leitura  dos  dados  totais  de  um  determinado  bico  ou  da
automação.
- Cabeçalho: &T
- Estrutura do comando:
## (&TBBMKK)
➢ &T [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ M [01]: Modo do comando...................................................................(caractere)
o $: totalizador em valor;
o L: totalizador em volume;
o l: totalizador em volume estendido;
o N: número de série;
o U: preço por litro;
o P: ponteiro da memória de abastecimento;
o C: configuração de bico.
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará à informação solicitada juntamente com o seu modo.
- Estrutura da resposta:
▪ Resposta de totalizador em valor:
## (TMBBVVVVVVVVKK)
➢ T [01]: Cabeçalho do comando........................................................(hexadecimal)
➢ M [01]: Modo do comando...................................................................(caractere)
➢ B [02]: Número de bico............................................................................(decimal)
➢ V [08]: Totalizador em dinheiro no momento da consulta......................(decimal)
## • Exemplo:
## TX: (&T08$06)
## RX: (T$080000405069)
▪ Resposta de totalizador em volume:
## (TMBBVVVVVVVVKK)
➢ T [01]: Cabeçalho do comando........................................................(hexadecimal)
➢ M [01]: Modo do comando...................................................................(caractere)
➢ B [02]: Número de bico............................................................................(decimal)
➢ V [08]: Totalizador em volume no momento da consulta........................(decimal)
## • Exemplo:


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 24 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## TX: (&T08L2E)
## RX: (TL080000405596)
▪ Resposta de totalizador em volume estendido (10 dígitos):
## (TMBBVVVVVVVVVVKK)
➢ T [01]: Cabeçalho do comando........................................................(hexadecimal)
➢ M [01]: Modo do comando...................................................................(caractere)
➢ B [02]: Número de bico............................................................................(decimal)
➢ V [10]: Totalizador em volume no momento da consulta........................(decimal)
## • Exemplo:
TX: (&T08l4E)
RX: (Tl08000000405516)
▪ Resposta da leitura de número de série:
## (TMBBNNNNNNNNKK)
➢ T [01]: Cabeçalho do comando........................................................(hexadecimal)
➢ M [01]: Permissão da automação.........................................................(caractere)
o G: gás;
o C: combustível.
➢ B [02]: Número de bico............................................................................(decimal)
➢ N [08]: Número de série da automação...................................................(decimal)
▪ Resposta da leitura de preço por litro:
## (TMBBAAAABBBBKK)
➢ T [01]: Cabeçalho do comando........................................................(hexadecimal)
➢ M [01]: Modo do comando...................................................................(caractere)
➢ B [02]: Número de bico............................................................................(decimal)
➢ A [04]: Preço por litro nível 1...................................................................(decimal)
➢ B [04]: Preço por litro nível 0...................................................................(decimal)
## • Exemplo:
## TX: (&T08U37)
## RX: (TU082000100094)
▪ Resposta da leitura do ponteiro da memória:
## (TMBBPPPPPPPPKK)
➢ T [01]: Cabeçalho do comando........................................................(hexadecimal)
➢ M [01]: Modo do comando...................................................................(caractere)
➢ B [02]: Número de bico............................................................................(decimal)
➢ P [08]: Número do ponteiro....................................................................(decimal)
## • Exemplo:
## TX: (&T19C27)
RX: (Ts19063E0202D3)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 25 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

▪ Resposta da leitura do ponteiro da memória:
(TsBBCCVVEEPPKK)
➢ Ts [02]: Cabeçalho do comando.......................................................(hexadecimal)
➢ B [02]: Número de bico............................................................................(decimal)
➢ C [02]: Número do canal..........................................................................(decimal)
➢ V [02]: Código de vírgula..................................................................(hexadecimal)
➢ E [02]: Endereço lido...............................................................................(decimal)
➢ P [02]: Número de posições configuradas no endereço..........................(decimal)
3.5.2 Alteração de preço
Este comando é utilizado para alterar o preço unitário do bico. O preço será atualizado no
display da bomba somente após o bico ser retirado do repouso.
- Cabeçalho: &U
- Estrutura do comando:
## (&UBBN0PPPPKK)
➢ &U [02]: Cabeçalho do comando.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ N [01]: Nível de preço...........................................................................(caractere)
o 0: à vista ou dinheiro;
o 1: a prazo.
➢ 0 [01]: Campo fixo...................................................................................(decimal)
➢ P [04]: Preço unitário...............................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará o código do bico ou um código de erro.
- Estrutura da resposta:
▪ Comando aceito:
## (MBB)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)
▪ Timeout da bomba:
(M?t)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?t [02]: Código de erro..........................................................................(caractere)
▪ Código de bico inválido:
(M?b)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?b [02]: Código de erro.........................................................................(caractere)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 26 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

▪ Erro de resposta da bomba:
(M?r)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?r [02]: Código de erro..........................................................................(caractere)
## • Exemplo:
## TX: (&U0400123409)
## RX: (U04)
3.5.3 Alteração de preço estendido
Este comando é utilizado para alterar o preço unitário do bico com 6 dígitos, assim como seu
nível de preço. O preço será atualizado no display da bomba somente após o bico ser retirado
do repouso.
- Cabeçalho: &U
- Estrutura do comando:
## (&UBBN0PPPPPPKK)
➢ &U [02]: Cabeçalho do comando.....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ N [01]: Nível de preço...........................................................................(caractere)
o 0: à vista ou dinheiro;
o 1: credito;
o 2: debito.
➢ 0 [01]: Campo fixo.................................................................................(caractere)
➢ P [06]: Preço unitário...............................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará o código do bico ou um código de erro.
- Estrutura da resposta:
▪ Comando aceito:
## (MBB)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)
▪ Timeout da bomba:
(M?t)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?t [02]: Código de erro..........................................................................(caractere)
▪ Código de bico inválido:
(M?b)
➢ M [01]: Cabeçalho do comando............................................................(caractere)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 27 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ ?b [02]: Código de erro.........................................................................(caractere)
▪ Erro de resposta da bomba:
(M?r)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?r [02]: Código de erro..........................................................................(caractere)
## • Exemplo:
## TX: (&U08000012346D)
## RX: (U08)
3.5.4 Leitura de preços
Este comando é utilizado para realizar a consulta dos preços configurados em cada bico.
- Cabeçalho: &T
- Estrutura do comando:
## (&TBBMKK)
➢ &T [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ M [01]: Modo de consulta.....................................................................(caractere)
o U: 2 níveis de preço;
o u: 3 níveis de preço.
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este  comando  retornará o  código  do  bico  juntamente  com  os  preços
configurados.
- Estrutura da resposta:
▪ Leitura de 2 níveis de preços:
## (TUBBXXXXYYYYKK)
➢ TU [02]: Cabeçalho do comando...........................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)
➢ X [04]: Preço nível 1.................................................................................(decimal)
➢ Y [04]: Preço nível 0.................................................................................(decimal)
▪ Leitura de 3 níveis de preços:
## (TUBBXXXXYYYYZZZZKK)
➢ TU [02]: Cabeçalho do comando...........................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)
➢ X [04]: Preço nível 1.................................................................................(decimal)
➢ Y [04]: Preço nível 0.................................................................................(decimal)
➢ Z [04]: Preço nível 2.................................................................................(decimal)
## • Exemplo:


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 28 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## TX: (&T08U37)
## RX: (TU08222211119D)

TX: (&T08u57)
## RX: (TU0822221111333369)
3.5.5 Predeterminação de valor (Preset)
Este comando é utilizado para autorizar um abastecimento com valor máximo definido.
- Cabeçalho: &P
- Estrutura do comando:
## (&PBBVVVVVVKK)
➢ &P [02]: Cabeçalho do comando......................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ V [06]: Valor do preset.............................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará o código do bico ou um código de erro.
- Estrutura da resposta:
▪ Comando aceito:
## (PBB)
➢ P [01]: Cabeçalho do comando.............................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)

▪ Timeout da bomba:
(P?t)
➢ P [01]: Cabeçalho do comando.............................................................(caractere)
➢ ?t [02]: Código de erro..........................................................................(caractere)
▪ Código de bico inválido:
(P?b)
➢ P [01]: Cabeçalho do comando.............................................................(caractere)
➢ ?b [02]: Código de erro.........................................................................(caractere)
▪ Erro de resposta da bomba:
(P?r)
➢ P [01]: Cabeçalho do comando.............................................................(caractere)
➢ ?r [02]: Código de erro..........................................................................(caractere)
## • Exemplo:
## TX: (&P08001000FF)
## RX: (P08)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 29 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.5.6 Predeterminação identificada (Preset identificado)
Este comando é  utilizado para autorizar o bico e  determinar o código identfid responsável
pelo próximo abastecimento.
- Cabeçalho: ?F
- Estrutura do comando:
## (?FBBPTTTTTTTTTTTTTTTTCAPPPPPPNNHRRRRRKK)
➢ ?F [02]: Cabeçalho do comando.......................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ P [01]: Parâmetro do comando.............................................................(caractere)
➢ T [16]: Identificador.........................................................................(hexadecimal)
➢ C [01]: Tipo do identificador..................................................................(caractere)
o 0: frentista;
o 1: cliente;
o 2: hodômetro.
➢ A [01]: Autorização de bomba..............................................................(caractere)
o S: autoriza;
o N: não autoriza.
➢ P [06]: Valor do preset.............................................................................(decimal)
➢ N [02]: Tempo até retirada do bico..........................................................(decimal)
➢ H [01]: Tipo de preset............................................................................(caractere)
o $: Total a pagar (libera endereço inteiro);
o V: Volume (somente somente bico desejado).
➢ R [05]: Reservado (preencher com “0”)...................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 e Horustech.
- Resposta: Este comando retornará o código do bico ou um código de erro.
- Estrutura da resposta:
▪ Comando aceito:
## (FBB)
➢ F [01]: Cabeçalho do comando.............................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)
▪ Bomba abastecendo:
(F?t)
➢ F [01]: Cabeçalho do comando..............................................................(caractere)
➢ ?t [02]: Código de erro..........................................................................(caractere)
▪ Código de bico inválido:
(F?b)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 30 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ F [01]: Cabeçalho do comando..............................................................(caractere)
➢ ?b [02]: Código de erro.........................................................................(caractere)
## • Exemplo:
## TX: (?F08PB3280000000000010S00100030$0000078)
## RX: (F08)
3.5.7 Modo de operação
Este  comando  é  utilizado  para  gerenciar  o  modo  de  funcionamento  da  bomba,  podendo
bloquear, liberar ou autorizar o abastecimento somente uma vez e logo após retornar para seu
status anterior.
- Cabeçalho: &M
- Estrutura do comando:
(&MBBmKK)
➢ &M [02]: Cabeçalho do comando....................................................(hexadecimal)
➢ B [02]: Código de bico......................................................................(hexadecimal)
➢ m [01]: Modo (tabela ao final do manual)........................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará o código do bico ou um código de erro.
- Estrutura da resposta:
▪ Comando aceito:
## (MBB)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ B [02]: Código de bico...........................................................................(caractere)
▪ Timeout da bomba:
(M?t)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?t [02]: Código de erro..........................................................................(caractere)
▪ Código de bico inválido:
(M?b)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?b [02]: Código de erro.........................................................................(caractere)
▪ Caractere de modo inválido:
(M?m)
➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?m [02]: Código de erro........................................................................(caractere)
▪ Erro de resposta da bomba:
(M?r)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 31 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ M [01]: Cabeçalho do comando............................................................(caractere)
➢ ?r [02]: Código de erro..........................................................................(caractere)
## • Exemplo:
## TX: (&M04A18)
## RX: (M04)
## 3.6 Relógio
3.6.1 Leitura de calendário
Este comando é utilizado para realizar a leitura do horário e data da automação
- Cabeçalho: &R
- Estrutura do comando:
## (&R)
➢ &R [02]: Cabeçalho do comando.....................................................(hexadecimal)
- Compatibilidade: IMS01 / 02, CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará a horário e a data da automação.
- Estrutura da resposta:
(REL HH:mm:SS DD/MM/AA)
➢ REL [03]: Cabeçalho do comando.........................................................(caractere)
➢ H [02]: Hora.............................................................................................(decimal)
➢ H [02]: Hora.............................................................................................(decimal)
➢ m [02]: Minuto........................................................................................(decimal)
➢ S [02]: Segundo.......................................................................................(decimal)
➢ D [02]: Dia...............................................................................................(decimal)
➢ M [02]: Mês.............................................................................................(decimal)
➢ A [02]: Ano..............................................................................................(decimal)
## • Exemplo:
## TX: (&R)
## RX: (REL 09:49:06 18/02/20)
3.6.2 Leitura de relógio estendido
Este comando é utilizado para leitura do horário e data da automação, além do dia da semana
em que se encontra.
- Cabeçalho: &KR1
- Estrutura do comando:
## (&KR1KK)
➢ &KR1 [04]: Cabeçalho do comando.................................................(hexadecimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 32 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- Compatibilidade: CBC04 / 05 / 06 e Horustech.
- Resposta: Este  comando  retornará a  horário  e  a  data  da  automação,  além  do  dia  da
semana.
- Estrutura da resposta:
(KR1AAMMDDWWHHmmSSKK)
➢ KR1 [03]: Cabeçalho do comando.........................................................(caractere)
➢ A [02]: Ano..............................................................................................(decimal)
➢ M [02]: Mês.............................................................................................(decimal)
➢ D [02]: Dia...............................................................................................(decimal)
➢ W [02]: Dia da semana............................................................................(decimal)
o 01: domingo;
o 02: segunda-feira;
o 03: terça-feira;
o 04: quarta-feira;
o 05: quinta-feira;
o 06: sexta-feira;
o 07: sábado.
➢ H [02]: Hora.............................................................................................(decimal)
➢ m [02]: Minuto........................................................................................(decimal)
➢ S [02]: Segundo.......................................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&KR1F4)
## RX: (KR12002180309570194)
3.6.3 Ajuste de calendário
Este comando é utilizado para ajustar as definições de data e hora do equipamento.
- Cabeçalho: &H
- Estrutura do comando:
## (&HDDHHMM)
➢ &H [02]: Cabeçalho do comando.....................................................(hexadecimal)
➢ D [02]: Dia................................................................................................(decimal)
➢ H [02]: Hora.............................................................................................(decimal)
➢ M [02]: Minuto........................................................................................(decimal)
- Compatibilidade: IMS01 / 02, CBC01 / 03 / 04 / 05 / 06 e Horustech.
- Resposta: Este comando retornará um código de confirmação.
- Estrutura da resposta:
## (&H)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 33 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

➢ &H [02]: Cabeçalho do comando..........................................................(caractere)
## • Exemplo:
## TX: (&H150956)
## RX: (&H)
3.6.4 Ajuste de calendário estendido
Este comando é  utilizado para  ajustar  as  definições  de  data  e  hora  do  equipamento assim
como o dia da semana.
- Cabeçalho: &KW1
- Estrutura do comando:
(&KW1AAMMDDWWHHmmSSKK)
➢ &KW1 [04]: Cabeçalho do comando................................................(hexadecimal)
➢ A [02]: Ano..............................................................................................(decimal)
➢ M [02]: Mês.............................................................................................(decimal)
➢ D [02]: Dia...............................................................................................(decimal)
➢ W [02]: Dia da semana............................................................................(decimal)
o 01: domingo;
o 02: segunda-feira;
o 03: terça-feira;
o 04: quarta-feira;
o 05: quinta-feira;
o 06: sexta-feira;
o 07: sábado.
➢ H [02]: Hora.............................................................................................(decimal)
➢ m [02]: Minuto........................................................................................(decimal)
➢ S [02]: Segundo.......................................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
- Compatibilidade: CBC06 (versões acima da 5.3) e Horustech (versões acima da 6.7).
- Resposta: Este  comando  retornará  os  mesmos  parâmetros  enviados  caso  tenha
aceitado o comando ou o calendário que está na automação caso tenha ocorrido erro
no comando.
- Estrutura da resposta:
(KW1AAMMDDWWHHmmSSKK)
➢ KW1 [03]: Cabeçalho do comando........................................................(caractere)
➢ A [02]: Ano..............................................................................................(decimal)
➢ M [02]: Mês.............................................................................................(decimal)
➢ D [02]: Dia...............................................................................................(decimal)
➢ W [02]: Dia da semana............................................................................(decimal)


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 34 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

o 01: domingo;
o 02: segunda-feira;
o 03: terça-feira;
o 04: quarta-feira;
o 05: quinta-feira;
o 06: sexta-feira;
o 07: sábado.
➢ H [02]: Hora.............................................................................................(decimal)
➢ m [02]: Minuto........................................................................................(decimal)
➢ S [02]: Segundo.......................................................................................(decimal)
➢ K [02]: Checksum.............................................................................(hexadecimal)
## • Exemplo:
## TX: (&KW120021301090000AB)
## RX: (KW12002130109000085)
3.7 Códigos de vírgula
O campo de código de vírgula é utilizado para a verificação do número de casas decimais nos
campos de total a pagar, volume e preço por litro.
Este campo possuí dois caracteres e está representado em valores hexadecimais.
## Divisão:
- Bit0 e bit1: total a pagar;
- Bit2 e bit3: volume;
- Bit4 e bit5: preço por litro.
## Exemplo:
Código  Preço por litro Volume Total a pagar
## Bit7 Bit6 Bit5 Bit4 Bit3 Bit2 Bit1 Bit0
## 3A 0 0 1 1 1 0 1 0
## 3E 0 0 1 1 1 1 1 0
3.8 Códigos de status
- L: Bomba se encontra livre para abastecer;
- B: Bomba bloqueada para realizar abastecimentos;


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 35 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- C: Bomba concluiu o abastecimento;
- A: Bomba está abastecendo;
- E: Bomba está aguardando a liberação da automação para iniciar o abastecimento;
- F: Bomba não presente ou falha de comunicação;
- P: Bomba está pronta para abastecer.
3.9 Códigos de modo
- L: Libera bomba para abastecimentos;
- B: Bloqueia bomba para abastecimentos;
- S: Parar abastecimento (não compatível com todas as bombas);
- A: Autoriza bomba para realizar somente um abastecimento, após finalizado o mesmo,
bico volta a seu estado inicial;
- P: Pausar o abastecimento, deixando a bomba em status de espera;
- H: Habilita sensor Identfid;
- I: Desabilita sensor Identfid;
3.10 Códigos de controle para identificadores
O caractere X nas relações abaixo é apresentado como as permissões que podem ser dadas
aos tipos de cartões.
- X1: Tag veículo;
- X2: Tag máquina de lavar;
## • X3: Reservado;
- X4: Cliente nível 1;
- X5: Cliente nível 2;
- X6: Cliente nível 3;
- X7: Funcionário nível 1;
- X8: Funcionário nível 2;
- X9: Funcionário nível 3;
- XA: Funcionário nível 4;
- XB: Funcionário nível 5;
- XC: Funcionário nível 6;
- XD: Gerente nível 1;
- XE: Gerente nível 2;
- XF: Controle total.
## 3.10.1 Permissões
Estas permissões representam o caractere “X” dos códigos de controle para identificadores.


Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Página 36 de 37
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## ➢ 1: Reservado;
➢ 2: Libera bomba;
➢ 4: Respeita turno;
➢ 6: Libera bomba / respeita turno;
➢ 8: Libera máquina de lavar;
➢ A: Libera bombas / libera máquina de lavar.
3.11 Cálculo de Checksum
Somatório em hexadecimais dispostos em dois caracteres adicionados às linhas de comando
e  resposta  do equipamento, esse  somatório garante que  os dados estejam íntegros ao serem
recebidos nas extremidades de comunicação (PC ⇔ Automação).
O cálculo do checksum é realizado com um acumulador, onde são acrescidos os valores ASCII
de  cada  caractere  presente  após  o  caractere  de  início  de  bloco  nas  linhas  de  comando  e
resposta.
Para manter um tamanho fixo de dois caracteres, as casas mais significativas são removidas
do resultado do somatório, até que seu tamanho preestabelecido seja respeitado.




Protocolo de Comunicação
## Companytec
## DT 435
## Revisão: 10
## 25/06/2025


Companytec Automação e Controle Ltda.
Av. Ferreira Viana, 1421 - Areal - 96080-000 - Pelotas - RS
www.companytec.com.br
## Fone: (53) 3284-8129
desenvolvimento@companytec.com.br

