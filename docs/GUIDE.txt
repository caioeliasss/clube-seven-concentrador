1 INTRODUÇÃO .............................................. 4 
2 FUNÇÕES GERAIS ......................................... 4 
2.1 COMUNICAÇÃO .......................................... 5 
2.1.1 Abrir comunicação serial ............... 5 
2.1.2 Fechar comunicação serial ............ 5 
2.1.3 Abrir comunicação ethernet (IP) ... 5 
2.1.4 Abrir comunicação ethernet (IP: 
porta)…….. ...................................................... 6 
2.1.5 Fechar a comunicação ethernet .... 6 
2.2 ABASTECIMENTO ........................................ 6 
2.2.1 Ler abastecimento ......................... 6 
2.2.2 Ler abastecimento PAF1 ................ 7 
2.2.3 Ler abastecimento por registro ..... 7 
2.2.4 Incrementar ponteiro .................... 7 
2.2.5 Leitura dos ponteiros de 
abastecimento ............................................... 8 
2.3 VISUALIZAÇÃO ........................................... 8 
2.3.1 Ler volume ou valor do 
abastecimento em andamento ...................... 8 
2.3.2 Ler volume ou valor do 
abastecimento em andamento com 
identificação ................................................... 8 
2.4 IDENTFID................................................... 9 
2.4.1 Ler identificador ............................ 9 
2.4.2 Incrementar identificador ............. 9 
2.4.3 Gravar identificador ...................... 9 
2.4.4 Excluir identificador ..................... 10 
2.4.5 Limpar lista de identificadores .... 10 
2.4.6 Ler identificadores na memória .. 10 
2.4.7 Colocar cartão na lista negra ...... 10 
2.4.8 Remover cartão da lista negra .... 11 
2.4.9 Limpar lista negra ....................... 11 
2.5 STATUS ................................................... 11 
2.5.1 Ler status dos bicos ..................... 11 
2.6 GERENCIAMENTO DE BOMBAS ..................... 12 
2.6.1 Liberar bico .................................. 12 
2.6.2 Autorizar bico .............................. 12 
2.6.3 Bloquear bico .............................. 12 
2.6.4 Parar abastecimento ................... 13 
2.6.5 Alterar preço por litro .................. 13 
2.6.6 Ler preço por litro ........................ 13 
2.6.7 Predeterminar/ Presetar ............. 14 
2.6.8 Predeterminar/ Presetar com 
identificação ................................................. 14 
2.6.9 Ler totalizador em volume .......... 15 
2.6.10 Ler totalizador em valor .............. 15 
2.7 RELÓGIO ................................................. 15 
2.7.1 Consultar calendário ................... 15 
2.7.2 Ajustar calendário 
automaticamente ........................................ 15 
2.7.3 Ajustar calendário para um horário 
específico… ................................................... 16 
2.7.4 Ajustar calendário de forma 
estendida… ................................................... 16 
3 FUNÇÕES COMPATÍVEIS COM VB6 .............16 
3.1 COMUNICAÇÃO ........................................ 16 
3.1.1 Abrir comunicação serial ............. 16 
3.1.2 Fechar comunicação serial .......... 17 
3.1.3 Abrir comunicação ethernet (IP) . 17 
3.1.4 Fechar comunicação ethernet ..... 17 
3.2 ABASTECIMENTO ...................................... 18 
3.2.1 Ler abastecimento ....................... 18 
3.2.2 Ler abastecimento PAF1 .............. 18 
3.2.3 Incrementar ponteiro .................. 18 
3.3 STATUS ................................................... 19 
3.3.1 Ler status dos bicos ..................... 19 
3.4 GERENCIAMENTO DE BOMBAS ..................... 19 
3.4.1 Liberar bico .................................. 19 
3.4.2 Autorizar bico .............................. 19 
3.4.3 Bloquear bico .............................. 20 
3.4.4 Parar abastecimento ................... 20 
3.4.5 Alterar preço por litro .................. 20 
3.4.6 Predeterminar/ Presetar ............. 20 
3.4.7 Ler totalizador em volume .......... 21 
3.4.8 Ler totalizador em valor .............. 21 
3.5 ENVIO DE COMANDO ................................. 21 
3.5.1 Enviar comando nativo ............... 21 
4 FUNÇÕES COMPATÍVEIS COM 
C/C++/C#/VB.NET ...............................................22 
4.1 COMUNICAÇÃO ........................................ 22 
4.1.1 Abrir comunicação serial ............. 22 
 
Manual de funções 
DLL - Protocolo Companytec 
DT 433 
Revisão: 04 
08/03/2021 
 
 
Página 3 de 35 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
4.1.2 Fechar comunicação serial .......... 23 
4.1.3 Abrir comunicação ethernet (IP) . 23 
4.1.4 Abrir comunicação ethernet (IP: 
porta)…….. .................................................... 23 
4.1.5 Fechar a comunicação ethernet .. 24 
4.2 ABASTECIMENTO ...................................... 24 
4.2.1 Ler abastecimento ....................... 24 
4.2.2 Ler abastecimento PAF1 .............. 24 
4.2.3 Incrementar ponteiro .................. 24 
4.2.4 Ler abastecimento por registro ... 25 
4.3 VISUALIZAÇÃO ......................................... 25 
4.3.1 Ler volume ou valor do 
abastecimento em andamento .................... 25 
4.4 IDENTFID................................................. 25 
4.4.1 Ler de identificador ..................... 25 
4.4.2 Incrementar identificador ........... 26 
4.4.3 Gravar identificador .................... 26 
4.4.4 Excluir identificador ..................... 26 
4.4.5 Limpar lista de identificadores .... 26 
4.4.6 Colocar cartão na lista negra ...... 27 
4.4.7 Remover cartão da lista negra .... 27 
4.4.8 Ler identificadores da memória .. 27 
4.5 STATUS ................................................... 28 
4.5.1 Ler status dos bicos ..................... 28 
4.6 GERENCIAMENTO DE BOMBAS ..................... 28 
4.6.1 Liberar bico .................................. 28 
4.6.2 Autorizar bico .............................. 28 
4.6.3 Bloquear bico .............................. 28 
4.6.4 Parar abastecimento ................... 29 
4.6.5 Alterar preço por litro .................. 29 
4.6.6 Predeterminar/ Presetar ............. 29 
4.6.7 Ler totalizador em volume .......... 30 
4.6.8 Ler totalizador em valor .............. 30 
4.7 RELÓGIO ................................................. 30 
4.7.1 Consultar calendário ................... 30 
4.7.2 Ajustar calendário 
automaticamente ........................................ 30 
4.8 ENVIO DE COMANDO ................................. 31 
4.8.1 Enviar comando nativo ............... 31 
5 CÓDIGOS DE CONTROLE PARA 
IDENTIFICADORES .............................................. 31 
5.1 NÍVEIS DE USUÁRIOS ................................. 31 
5.2 NÍVEIS DE PERMISSÃO ................................ 32 
6 ESTRUTURA DAS RESPOSTAS .....................32 
 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
1 Introdução 
08/03/2021 
Caro desenvolvedor.  Essa biblioteca foi desenvolvida para facilitar a implementação dos 
equipamentos Companytec em seu software. Disponibilizando todas as funções necessárias 
para a integração, nossa DLL já está presente em muitos sistemas compatíveis com nossa 
solução. 
A fim de atender as linguagens mais utilizadas na atualidade, nossa DLL possui funções que 
se adaptam à sua linguagem, para que isso seja possível, existem várias funções que executam 
o mesmo trabalho, diferenciando-se apenas na sua maneira de passagem de parâmetros e 
retornos. Entre as linguagens que já utilizam a DLL podemos citar:  
• Delphi; 
• Visual Basic 6; 
• Visual Basic .NET; 
• C#; 
• C/C++; 
• Fox Pro; 
• COBOL for Windows. 
2 Funções gerais 
O conjunto de funções apresentadas neste capitulo representam as primeiras e principais 
funções criadas nesta DLL, caso o desenvolvedor tenha problemas em tratar o parâmetros 
passados pelas funções e problemas de tipagem de dados, como a dificuldade em criar as 
estruturas de dados definidas na DLL, o desenvolvedor poderá optar por outras soluções 
apresentadas nos capítulos abaixo. 
As funções definidas na DLL não são especificas por linguagem, logo o desenvolvedor pode 
utilizar algumas funções deste capitulo, e quaisquer outra função que ele tiver dificuldade, 
poderá optar por escolher a função de outros capítulos. 
As funções definidas nos próximos capítulos, por exemplo VB6, podem ser utilizados por 
outras linguagens de programação sem qualquer problema, a criação destes conjuntos de 
funções especificas se deu em função de restrições impostas pela linguagem alvo. 
Abaixo seguem as funções padrões da DLL Companytec: 
Página 4 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
2.1 Comunicação 
2.1.1 Abrir comunicação serial 
08/03/2021 
Função que abre a porta de comunicação serial para envio e recebimento de comandos, ela 
precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function InicializaSerial(np:byte): Boolean; 
• Parâmetros de entrada: 
➢ Np: Número da porta serial. 
• Retorno da função: 
➢ True: conexão aberta com sucesso; 
➢ False: não foi possível se conectar. 
2.1.2 Fechar comunicação serial 
Função chamada para finalizar a conexão serial. 
• Compatibilidade: Função compatível a partir da versão 5.0.0 
• Estrutura: Function FechaSerial: DWORD; 
• Parâmetros de entrada: nenhum. 
• Retorno da função:  
➢ 1: conexão fechada com sucesso; 
➢ 0: não foi possível fechar a comunicação. 
2.1.3 Abrir comunicação ethernet (IP) 
Função que abre a comunicação ethernet para o envio e recebimento de comandos, ela 
precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function InicializaSocket ( ip:pchar):  Boolean; 
• Parâmetros de entrada: 
➢ Ip: Endereço na rede em que será feita a conexão. 
• Retorno da função: 
➢ True: conexão aberta com sucesso; 
➢ False: não foi possível se conectar. 
Página 5 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
08/03/2021 
Atenção: Quando utilizada esta função, a porta que irá se conectar à automação é a 
1771. Caso seja necessária alguma manutenção utilizando o software HRS Console, é 
aconselhável fechar o software gerencial. 
2.1.4 Abrir comunicação ethernet (IP: porta) 
Função que abre a comunicação ethernet para o envio e recebimento de comandos, ela 
precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function InicializaSocket2(ip:pchar;porta:integer):Boolean; 
• Parâmetros de entrada: 
➢ Ip: Endereço na rede em que será feita a conexão. 
➢ Porta: Porta em que será aberta a comunicação, por padrão, a porta 2001 da 
automação é destinada ao sistema gerencial. 
• Retorno da função: 
➢ True: conexão aberta com sucesso; 
➢ False: não foi possível se conectar. 
2.1.5 Fechar a comunicação ethernet 
Função chamada para finalizar uma conexão do tipo ethernet. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function FechaSocket: boolean; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ True: conexão fechada com sucesso; 
➢ False: não foi possível fechar a comunicação. 
2.2 Abastecimento 
2.2.1 Ler abastecimento 
Função que lê o abastecimento atual da memória da automação e incrementa o ponteiro de 
leitura de abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0. 
• Estrutura: Function LeAbastecimento:abast; 
• Parâmetros de entrada: nenhum. 
Página 6 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Retorno da função: 
08/03/2021 
➢ Abast: retorna os dados do abastecimento como, preço total, volume 
abastecido, número do bico e etc, em uma estrutura definida na DLL (veja a 
definição do tipo “abast” no final deste documento). 
2.2.2 Ler abastecimento PAF1 
Função utilizada para ler os abastecimentos com identificação de frentista e cliente. Este 
comando, quando não passado nenhum parâmetro de entrada, executa automaticamente 
o comando de incremento. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: 
Function LeAbastecimentoPAF1(autoIncrement:boolean=true):AbastPAF1; 
• Parâmetros de entrada: nenhum ou false para não enviar o auto incremento. 
• Retorno da função: 
➢ AbastPAF1: Retorna os dados do abastecimento como, preço total, volume, 
número do bico, informações de identfid e etc, em uma estrutura definida 
na DLL (veja a definição do tipo “abastPAF1” no final deste documento). 
2.2.3 Ler abastecimento por registro 
Função que tem por finalidade ler um registro (local na memória) específico. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function LeStRegistroFid(NumReg:integer):ShortString; 
• Parâmetros de entrada:  
➢ NumReg: número do registro desejado. 
• Retorno da função:  
➢ Sucesso: String do abastecimento de acordo com o protocolo Companytec; 
➢ Falha: falha na execução do comando. 
2.2.4 Incrementar ponteiro 
Função que tem por finalidade informar ao concentrador que o abastecimento atual já foi 
lido e armazenado, podendo então assim colocar o ponteiro de leitura no próximo registro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Procedure Incrementa; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: nenhum. 
Página 7 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
2.2.5 Leitura dos ponteiros de abastecimento 
08/03/2021 
Função utilizada para coletar a posição dos ponteiros de escrita e leitura dos abastecimentos 
na memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.4.0. 
• Estrutura: function GetMemoryPointers:MemoryPointers; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ MemoryPointers: retorna a posição dos ponteiros de escrita e leitura em 
uma estrutura definida na DLL (veja a definição do tipo “MemoryPointers” 
no final deste documento). 
2.3 Visualização 
2.3.1 Ler volume ou valor do abastecimento em andamento 
Função que tem por finalidade ler os abastecimentos que estão em andamento. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function LeVisualizacao( ):OnLine; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ Online: estrutura composta por 48 posições contendo código de bico e o 
volume que o mesmo está abastecendo. As posições não utilizadas 
retornarão no campo “bico”, o valor “00” (veja definição do tipo “online” no 
final deste documento). 
2.3.2 Ler volume ou valor do abastecimento em andamento com identificação 
Função utilizada para ler o andamento de um abastecimento com o respectivo código 
identfid que o autorizou. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function GetVisualizacaoId:shortstring; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ Shortstring: código de bico junto com o código do identificador que o 
autorizou. 
Página 8 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
2.4 Identfid 
2.4.1 Ler identificador 
08/03/2021 
Função utilizada para coletar o identificador que foi lido pela automação, mas não está 
gravado em sua memória, neste caso a automação envia o código do identificador ao 
computador quando este solicitar. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function FidIdent:IFid; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ Ifid: armazena as informações do cartão retornado pelo concentrador. 
2.4.2 Incrementar identificador 
Função utilizada para incrementar o ponteiro de identificador e passar para o próximo lido 
que não esteja registrado no concentrador. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Procedure FidIncrementa; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: nenhum. 
2.4.3 Gravar identificador 
Função utilizada para gravar um identificador na memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: 
Function saveTagFid (controle1, controle2:char; tag,t1in,t1out,t2in,t2out:string): 
integer; 
• Parâmetros de entrada: 
➢ Controle 1: nível de usuário (verificar tabela de permissões no final deste 
documento); 
➢ Controle 2:nível de permissão (verificar tabela de permissões no final deste 
documento); 
➢ Tag: código do identificador; 
➢ T1in: início do turno A (hh:mm); 
➢ T1out: término do turno A (hh:mm); 
➢ T2in: início do turno B (hh:mm); 
➢ T2out: término do turno B (hh:mm); 
Página 9 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Retorno da função: 
➢ INTEGER: posição onde foi armazenado o identificador; 
➢  0: falha na execução do comando. 
2.4.4 Excluir identificador 
Função para deletar um identificador da memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
08/03/2021 
• Estrutura: Function deleteTagFid (posicao:integer;tag:string): integer; 
• Parâmetros de entrada:  
➢ Posição: posição na memória onde se encontra o identificador; 
➢ Tag: código de identificação do cartão a ser excluído. 
• Retorno da função:  
➢ Integer: posição deletada; 
➢ 0: falha na execução do comando. 
2.4.5 Limpar lista de identificadores 
Função utilizada para limpar a lista de cartões identificadores da automação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: procedure clearTagFid; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: nenhum. 
2.4.6 Ler identificadores na memória 
Função para ler os identificadores da memória da automação através de seu registro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function FidLeRegistro (nro:integer): ShortString; 
• Parâmetros de entrada:  
➢ Nro: posição de memória que deseja ser lida. 
• Retorno da função:  
➢ Shorstring: retorna a string completa conforme protocolo de comunicação 
Companytec. 
2.4.7 Colocar cartão na lista negra 
Esta função tem por finalidade colocar identificadores na lista negra, que seria uma lista onde 
o concentrador não reconhece os identificadores temporariamente, ou seja, um cartão que está 
na lista negra não pode desbloquear um bico. Esta lista possuí 20 posições. 
Página 10 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function PushBlackList(tag:pchar):boolean; 
• Parâmetros de entrada: 
➢ TAG: código de identificador para ser colocado na lista negra. 
• Retorno da função:  
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
2.4.8 Remover cartão da lista negra 
Esta função tem por finalidade de retirar um cartão da lista negra. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function PopBlackList(tag:pchar):boolean; 
• Parâmetros de entrada: 
➢ TAG: código de identificador a ser removido da lista negra. 
• Retorno da função:  
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
2.4.9 Limpar lista negra 
08/03/2021 
Esta função tem por finalidade excluir todos os cartões da lista negra, ou seja, limpá-la. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function ClearBlackList():boolean; 
• Parâmetros de entrada: nenhum; 
• Retorno da função:  
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
2.5 Status 
2.5.1 Ler status dos bicos 
Função utilizada para ler a situação de cada bomba conectada ao equipamento. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function LeStatus( ):multistatus; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
Página 11 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
08/03/2021 
➢ Multistatus: estrutura de dados de 48 posições que contém o atual estado 
das bombas conectadas ao concentrador (veja a definição do tipo 
“multistatus” no final deste documento). 
2.6 Gerenciamento de bombas 
2.6.1 Liberar bico 
Esta função libera a bomba para abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function AutoLibera ( bico:string): Error; 
• Parâmetros de entrada: 
➢ Bico: código do bico. 
• Retorno da função: 
➢ Error: tipo de dados definido pelo usuário; 
➢ None: comando executado com sucesso (veja definição do tipo ERROR no 
final deste documento). 
2.6.2 Autorizar bico 
Esta função autoriza a bomba a realizar apenas um abastecimento (funcional quando a 
bomba foi colocada anteriormente em modo de bloqueio, após o término do abastecimento, a 
mesma voltará para o status de bloqueada). 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function AutorizaAbast ( bico:string): Error; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ Error: tipo de dados definido pelo usuário; 
➢ None: comando executado com sucesso (veja definição do tipo ERROR no 
final deste documento). 
2.6.3 Bloquear bico 
Função utilizada para bloquear a bomba para não realizar abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function BloqueiaBico ( bico:string): Error; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ Error: tipo de dados definido pelo usuário; 
Página 12 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
08/03/2021 
➢ None: comando executado com sucesso (veja definição do tipo ERROR no 
final deste documento). 
2.6.4 Parar abastecimento 
Função utilizada para parar o abastecimento, está funcionalidade não está disponível em 
todas as bombas. Para verificar esta informação, favor consultar o manual da bomba desejada. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function ParaBomba ( bico:string): Error; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ Error: tipo de dados definido pelo usuário; 
➢ None: comando executado com sucesso (veja definição do tipo ERROR no 
final deste documento). 
2.6.5 Alterar preço por litro 
Função utilizada para alterar o preço da bomba pela automação (o valor será alterado no 
display somente quando for iniciado um novo abastecimento). 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura:  
Function AlteraPrecoNivel(bico:string;preco:double;decimais:byte;nivel:integer):error; 
• Parâmetros de entrada:  
➢ Bico: código do bico; 
➢ Preço: preço a ser alterado; 
➢ Decimais: quantidade de casas decimais; 
➢ Nível: nível de preço a ser alterado (0, 1 ou 2) 
• Retorno da função: 
➢ Error: tipo de dados definido pelo usuário; 
➢ None: comando executado com sucesso (veja definição do tipo ERROR no 
final deste documento). 
2.6.6 Ler preço por litro 
Função utilizada para a leitura de preços e seus níveis. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function LePPLNivel(bico:string; niveis: integer):PPLNivel; 
• Parâmetros de entrada:  
➢ Bico: código do bico; 
Página 13 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
08/03/2021 
➢ Níveis: quantidade dos níveis que deseja ler, poder ler somente o nível 0, 1 
ou 2. 
• Retorno da função: 
➢ Valor dos preços solicitados em uma estrutura PPLNível (veja informação 
no final deste manual); 
➢ -1: falha na execução do comando. 
2.6.7 Predeterminar/ Presetar 
Função utilizada para predeterminar o valor do abastecimento para um referido bico. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function Preset(bico:byte;valor:PChar):ShortString; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
➢ Valor: total a ser pré-determinado. 
• Retorno da função: 
➢ PXX: onde XX é o número do bico; 
➢ FALHA: falha na execução do comando. 
2.6.8 Predeterminar/ Presetar com identificação 
Função que executa o comando de predeterminação para liberar o bico com código de 
identificador. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura:  
function presetIdf(bico: Byte; tag:AnsiString; frentista, autoriza, dinheiro: Boolean; 
valor: PChar; tempo: Integer):ShortString; 
• Parâmetros de entrada:  
➢ Bico: código do bico; 
➢ Tag: código do identificador; 
➢ Frentista: flag para configurar o preset como cliente ou frentista; 
➢ Autoriza: flag para liberar a bomba para abastecimento ou não; 
➢ Dinheiro: flag para escolher o tipo de preset como dinheiro ou volume; 
➢ Valor: valor a ser presetado; 
➢ Tempo: tempo até começar o abastecimento ou que o frentista aproxime o 
cartão do sensor identfid. 
• Retorno da função:  
➢ FXX: onde XX é o número do bico; 
➢ FALHA: falha na execução do comando. 
Página 14 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
2.6.9 Ler totalizador em volume 
Função utilizada para coletar o totalizador em volume do bico desejado. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function GetEncerranteVolume(bicoInt:byte):ShortString; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ Totalizador: comando executado com sucesso; 
➢ FALHA: falha na execução do comando. 
2.6.10 Ler totalizador em valor 
Função utilizada para coletar o totalizador em valor do bico desejado. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function GetEncerranteValor(bicoInt:byte):ShortString; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ Totalizador: comando executado com sucesso; 
➢ FALHA: falha na execução do comando. 
2.7 Relógio 
2.7.1 Consultar calendário 
Esta função retorna o valor que está no relógio do concentrador. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function getclock:string; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ Data no formato DD/MM/AA HH:MM. 
2.7.2 Ajustar calendário automaticamente 
Função para ajustar o relógio do concentrador utilizando a data e hora do PC. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function SetClock(par:string):boolean; 
• Parâmetros de entrada:  
08/03/2021 
➢ Par: ‘AUTO’ para o concentrador sincronizar com o relógio do computador. 
• Retorno da função: 
Página 15 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
2.7.3 Ajustar calendário para um horário específico 
08/03/2021 
Função utilizada para sincronizar o calendário para um horário escolhido pelo usuário. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function SetIntClock(dia,hora,minuto:byte):Boolean 
• Parâmetros de entrada:  
➢ Dia, hora e minuto. 
• Retorno da função: 
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
2.7.4 Ajustar calendário de forma estendida 
Função utilizada para sincronizar o calendário para um data e horário escolhido pelo usuário. 
• Compatibilidade: Função compatível a partir da versão 5.1.0. 
• Estrutura:function 
setExtendedWatch(ano,mes,dia,diaSemana,hora,minuto,segundo: string):Boolean 
• Parâmetros de entrada:  
➢ Ano, mês, dia, dia da semana, hora, minuto e segundo. 
• Retorno da função: 
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
3 Funções compatíveis com VB6 
As funções compatíveis com VB6 tem o formate VB_nomedafunção. Estas funções podem 
ser utilizadas pelos usuários de C# passando os parâmetros como referência. 
Sistemas que antes foram implementados com VB6 e que agora migraram para o VB NET 
perceberão que algumas funções não vão funcionar como o desejado, para solucionar este 
problema, deve-se utilizar as funções compatíveis com .NET. 
3.1 Comunicação 
3.1.1 Abrir comunicação serial 
Esta função abre a porta de comunicações serial para envio e recebimento de comandos, ela 
precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
Página 16 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_OpenSerial( np: integer ):integer; 
• Parâmetros de entrada: 
➢ Np: Número da porta serial. 
• Retorno da função: 
➢ 1: conexão aberta com sucesso; 
➢ 0: não foi possível se conectar. 
3.1.2 Fechar comunicação serial 
Função chamada para finalizar a conexão serial. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_CloseSerial:integer; 
• Parâmetros de entrada: nenhum. 
• Retorno da função:  
08/03/2021 
➢ 1: conexão fechada com sucesso; 
➢ 0: não foi possível fechar a comunicação. 
3.1.3 Abrir comunicação ethernet (IP) 
Função que abre a porta de comunicação ethernet para o envio e recebimento de comandos, 
ela precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_OpenSocket( ip: string ):integer; 
• Parâmetros de entrada: 
➢ Ip: Endereço na rede em que será feita a conexão. 
• Retorno da função: 
➢ 1: conexão aberta com sucesso; 
➢ 0: não foi possível se conectar. 
Atenção: Quando utilizada esta função, a porta que irá se conectar à automação é a 
1771. Caso seja necessária alguma manutenção utilizando o software HRS Console, é 
aconselhável fechar o software gerencial. 
3.1.4 Fechar comunicação ethernet 
Função chamada para finalizar uma conexão do tipo ethernet. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
Página 17 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Estrutura: Function VB_CloseSerial:integer; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ 1: conexão fechada com sucesso; 
➢ 0: não foi possível fechar a comunicação. 
3.2 Abastecimento 
3.2.1 Ler abastecimento 
08/03/2021 
Função que lê o abastecimento atual da memória da automação e incrementa o ponteiro de 
leitura de abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_ReadSale(var st:string):integer; 
• Parâmetros de entrada:  
➢ St: string que por referência é passada vazia para retornar o abastecimento. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.2.2 Ler abastecimento PAF1 
Função utilizada para ler os abastecimentos com identificação de frentista e cliente. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_ReadSalePAF(var st:string):integer; 
• Parâmetros de entrada:  
➢ St: string que por referência é passada vazia para retornar o abastecimento. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.2.3 Incrementar ponteiro 
Função que tem por finalidade informar ao concentrador que o abastecimento atual já foi 
lido e armazenado, podendo então assim colocar o ponteiro de leitura no próximo registro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: function VB_NextSale:integer; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: retorna um valor inteiro que pode ser desprezado. 
Página 18 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
3.3 Status 
3.3.1 Ler status dos bicos 
08/03/2021 
Função utilizada para ler a situação de cada bomba conectada ao equipamento. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: function VB_ReadState(var st:string):integer; 
• Parâmetros de entrada: 
➢ St: string passada por referência que retorna o estado de cada lado da 
bomba no formato do protocolo de comunicação Companytec. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.4 Gerenciamento de bombas 
3.4.1 Liberar bico 
Esta função libera a bomba para abastecimentos, mantendo a mesma em status de livre. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_FreePump(bico:string):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.4.2 Autorizar bico 
Esta função autoriza a bomba a realizar apenas um abastecimento (funcional quando a 
bomba foi colocada anteriormente em modo de bloqueio, após o término do abastecimento, a 
mesma voltará para o status de bloqueada). 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_AutPump(bico:string):integer; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
Página 19 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
3.4.3 Bloquear bico 
Função utilizada para bloquear a bomba para não realizar abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_BlockPump(bico:string):integer; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.4.4 Parar abastecimento 
08/03/2021 
Função utilizada para parar o abastecimento. Esta funcionalidade não está disponível em 
todas as bombas. Para verificar esta informação, favor consultar o manual da bomba desejada. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_StopPump(bico: String):integer; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.4.5 Alterar preço por litro 
Função utilizada para alterar o preço da bomba pela automação (o valor será alterado no 
display somente quando iniciado um novo abastecimento). 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: function VB_SetPriceLevel(bico,price,level:pchar):integer; 
• Parâmetros de entrada:  
➢ Bico: código do bico; 
➢ Preço: o preço a ser alterado; 
➢ Level: nível de preço desejado. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.4.6 Predeterminar/ Presetar 
Função utilizada para predeterminar o valor do abastecimento para um referido bico. 
Página 20 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_PresetPump(bico,cash:string):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
➢ Cash: total a ser pré-determinado com 6 caracteres. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
3.4.7 Ler totalizador em volume 
08/03/2021 
Função que retorna o totalizador em volume do bico que foi passado como parâmetro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_ReadTotalsVolume(bico:string):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
• Retorno da função: 
➢ Totalizador: comando executado com sucesso; 
➢ -1: falha na execução do comando. 
3.4.8 Ler totalizador em valor 
Função que retorna o totalizador em valor do bico que foi passado como parâmetro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_ReadTotalsCash(bico:pchar):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
• Retorno da função: 
➢ Totalizador: comando executado com sucesso; 
➢ -1: falha na execução do comando. 
3.5 Envio de comando 
3.5.1 Enviar comando nativo 
Função para realizar a comunicação com o concentrador utilizando comandos do protocolo. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function VB_SendReceiveText(var st:string;timeout:integer):integer; 
Página 21 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Parâmetros de entrada: 
08/03/2021 
➢ St: comando no formato do protocolo Companytec, por referência; 
➢ Timeout: tempo para esperar a resposta do concentrador. 
• Retorno da função: 
➢ St: retorna a string com a resposta do comando em caso de sucesso; 
➢ 0: falha na execução do comando. 
4 Funções compatíveis com C/C++/C#/VB.NET 
As funções compatíveis com C/C++, C# e VB.NET são utilizadas no exemplo de comunicação 
disponível no kit de desenvolvimento. 
O tipo string retornado na DLL é composto de uma sequência de caracteres com o caractere 
‘\0’ no final, indicando o final da string, este tipo é compatível com o tipo char. 
Quando utilizar as funções deste capítulo para C# e VB.Net, siga o método abaixo: 
//declaração na DLL 
Function C_GetSalePAF():pchar; stdcall;  
//declaração 
[DllImport("companytec.dll")] 
public static extern System.IntPtr C_GetSalePAF() ; 
//chamada no código 
IntPtr data = C_GetSalePAF(); 
string str  = Marshal.PtrToStringAnsi(data); 
4.1 Comunicação 
4.1.1 Abrir comunicação serial 
Esta função abre a porta de comunicações serial para envio e recebimento de comandos, ela 
precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_OpenSerial(np:integer):integer; 
• Parâmetros de entrada: 
Página 22 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
➢ Np: Número da porta serial. 
• Retorno da função: 
➢ 1: conexão aberta com sucesso; 
➢ 0: não foi possível se conectar. 
4.1.2 Fechar comunicação serial 
Função chamada para finalizar a conexão serial. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_CloseSerial:integer; 
• Parâmetros de entrada: nenhum. 
• Retorno da função:  
08/03/2021 
➢ 1: conexão fechada com sucesso; 
➢ 0: não foi possível fechar a comunicação. 
4.1.3 Abrir comunicação ethernet (IP) 
Função que abre a porta de comunicação ethernet para o envio e recebimento de comandos, 
ela precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function  C_OpenSocket(ip:pchar):integer; 
• Parâmetros de entrada: 
➢ Ip: Endereço na rede em que será feita a conexão. 
• Retorno da função: 
➢ 1: conexão aberta com sucesso; 
➢ 0: não foi possível se conectar. 
Atenção: Quando utilizada esta função, a porta que irá se conectar à automação é a 
1771. Caso seja necessária alguma manutenção utilizando o software HRS Console, é 
aconselhável fechar o software gerencial. 
4.1.4 Abrir comunicação ethernet (IP: porta) 
Função que abre a porta de comunicação ethernet para o envio e recebimento de comandos, 
ela precisa ser chamada somente uma vez, no início da aplicação, somente para dar o start da 
comunicação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: function C_OpenSocket2(ip:pchar; port:integer ):integer; 
• Parâmetros de entrada: 
Página 23 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
➢ Ip: Endereço na rede em que será feita a conexão; 
➢ Porta: porta que deseja abrir a comunicação. 
• Retorno da função: 
➢ 1: conexão aberta com sucesso; 
➢ 0: não foi possível se conectar. 
4.1.5 Fechar a comunicação ethernet 
Função chamada para finalizar uma conexão do tipo ethernet. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_CloseSocket:integer; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ 1: conexão fechada com sucesso; 
➢ 0: não foi possível fechar a comunicação. 
4.2 Abastecimento 
4.2.1 Ler abastecimento 
Função que lê o abastecimento atual da memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_GetSale():pchar; 
• Parâmetros de entrada: nenhum. 
08/03/2021 
• Retorno da função: retorna os dados do abastecimento como preço total, volume 
abastecido, número do bico e etc, em uma estrutura igual ao do protocolo 
Companytec. 
4.2.2 Ler abastecimento PAF1 
Função utilizada para ler os abastecimentos com identificação de frentista e cliente.  
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_GetSalePAF():pchar; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: retorna os dados do abastecimento como preço total, volume 
abastecido, número do bico e etc, em uma estrutura igual ao do protocolo 
Companytec. 
4.2.3 Incrementar ponteiro 
Função que tem por finalidade informar ao concentrador que o abastecimento atual já foi 
lido e armazenado, podendo então assim colocar o ponteiro de leitura no próximo registro. 
Página 24 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Procedure C_NextSale; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: nenhum. 
4.2.4 Ler abastecimento por registro 
Função que tem por finalidade ler um registro (local na memória) específico. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: function C_ReadRegister(reg:pchar):pchar; 
• Parâmetros de entrada:  
➢ reg: número do registro a ser lido. 
08/03/2021 
• Retorno da função: resposta de acordo com o comando de leitura de registro de 
abastecimento do protocolo Companytec. 
4.3 Visualização 
4.3.1 Ler volume ou valor do abastecimento em andamento 
Função que tem por finalidade ler os abastecimentos que estão em andamento, retornando 
código de bico e o valor ou volume em tempo real. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_Visualize:pchar; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: Para cada abastecimento retorna código de bico e valor ou 
volume atual.  
4.4 Identfid 
4.4.1 Ler de identificador 
Função utilizada para coletar o identificador que foi lido pela automação, mas não está 
gravado em sua memória, neste caso a automação envia o código do identificador ao 
computador quando este solicitar. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: function C_ReadIdf():pChar; 
• Parâmetros de entrada: nenhum 
• Retorno da função: retornar a string completa com informações do identificador 
passado conforme protocolo de comunicação Companytec. 
Página 25 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
4.4.2 Incrementar identificador 
08/03/2021 
Função utilizada para incrementar o ponteiro de identificador e passar para o próximo lido 
que não esteja registrado no concentrador. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: Procedure C_IncrementIdf; 
• Parâmetros de entrada: nenhum 
• Retorno da função: nenhum 
4.4.3 Gravar identificador 
Função utilizada para gravar um identificador na memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: Function C_SaveTagIdf(control1,control2:integer;tag:pchar):integer; 
• Parâmetros de entrada:  
➢ Control 1: nível de usuário (verificar tabela de permissões no final deste 
documento); 
➢ Control 2: nível de permissão (verificar tabela de permissões no final deste 
documento); 
➢ Tag: código do identificador. 
• Retorno da função:  
➢ Número do registro: sucesso na exclusão; 
➢ 0: falha na exclusão 
4.4.4 Excluir identificador 
Função utilizada para deletar um identificador da memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: Function C_DeleteTagIdf(control,position,tag:pchar):integer; 
• Parâmetros de entrada:  
➢ Control: nível de funcionário e permissão que o identificador foi cadastrado; 
➢ Position: posição do identificador na memória da automação; 
➢ Tag: código do identificador. 
• Retorno da função:  
➢ Número do registro: sucesso na exclusão; 
➢ 0: falha na exclusão. 
4.4.5 Limpar lista de identificadores 
Função utilizada para limpar a lista de cartões identificadores da automação. 
Página 26 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: procedure C_ClearMemoryIdf; 
• Parâmetros de entrada: nenhum 
• Retorno da função: nenhum 
4.4.6 Colocar cartão na lista negra 
08/03/2021 
Esta função tem por finalidade colocar identificadores na lista negra, que seria uma lista onde 
o concentrador não reconhece os identificadores temporariamente, ou seja, um cartão que está 
na lista negra não pode desbloquear um bico. Esta lista possuí 20 posições. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: Function C_PushIdfBlackList(tag:pchar):integer; 
• Parâmetros de entrada:  
➢ Tag: código de identificador a ser colocado na lista negra. 
• Retorno da função:  
➢ 1: sucesso na execução do comando; 
➢ 0: falha na execução do comando. 
4.4.7 Remover cartão da lista negra 
Esta função tem por finalidade de retirar um cartão da lista negra. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: Function C_RemoveIdfBlackList(tag:pchar):integer; 
• Parâmetros de entrada:  
➢ Tag: código de identificador a ser removido da lista negra. 
• Retorno da função:  
➢ 1: sucesso na execução do comando; 
➢ 0: falha na execução do comando. 
4.4.8 Ler identificadores da memória 
Função para ler os identificadores da memória da automação. 
• Compatibilidade: Função compatível a partir da versão 5.3.0. 
• Estrutura: Function C_ReadRegisterIdf(nro:integer):pchar; 
• Parâmetros de entrada:  
➢ nro: posição de memória que deseja ser lida. 
• Retorno da função: retornar a string completa conforme protocolo de comunicação 
Companytec. 
Página 27 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
4.5 Status 
4.5.1 Ler status dos bicos 
08/03/2021 
Função utilizada para ler a situação de cada bomba conectada ao equipamento. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_readState ():pchar; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: retorno no mesmo formato da leitura de status presente no 
protocolo Companytec. 
4.6 Gerenciamento de bombas 
4.6.1 Liberar bico 
Esta função libera a bomba para abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_FreePump(bico:pchar):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
4.6.2 Autorizar bico 
Função autoriza a bomba a realizar apenas um abastecimento (funcional quando a bomba 
foi colocada anteriormente em modo de bloqueio, após o término do abastecimento, a mesma 
voltará para o status de bloqueada). 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_AutoPump(bico:pchar):integer; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: em caso de sucesso no comando; 
➢ 0: em caso de falha no comando. 
4.6.3 Bloquear bico 
Função utilizada para bloquear a bomba para não realizar abastecimentos. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_BlockPump(bico:pchar):integer; 
Página 28 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
4.6.4 Parar abastecimento 
08/03/2021 
Função utilizada para parar o abastecimento, essa funcionalidade não está disponível em 
todas as bombas. Para verificar esta informação, favor consultar o manual da bomba desejada. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_StopPump(bico:pchar):pchar; 
• Parâmetros de entrada:  
➢ Bico: código do bico. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
4.6.5 Alterar preço por litro 
Função utilizada para alterar o preço da bomba pela automação (o valor será alterado no 
display somente quando iniciado um novo abastecimento). 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_SetPrice(bico,preco:pchar):integer; 
• Parâmetros de entrada:  
➢ Bico: código do bico; 
➢ Preço: preço a ser alterado. 
• Retorno da função: 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
4.6.6 Predeterminar/ Presetar 
Função utilizada para predeterminar o valor do abastecimento para um referido bico. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_PresetPump(bico,cash:pchar):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
➢ Cash: total a ser pré-determinado. 
• Retorno da função: 
Página 29 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
➢ 1: comando executado com sucesso; 
➢ 0: falha na execução do comando. 
4.6.7 Ler totalizador em volume 
08/03/2021 
Função que retorna o totalizador em volume do bico que foi passado como parâmetro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_ReadTotalsVolume(bico:pchar):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
• Retorno da função: 
➢ Encerrante: comando executado com sucesso; 
➢ -1: falha na execução do comando. 
4.6.8 Ler totalizador em valor 
Função que retorna o totalizador em valor do bico que foi passado como parâmetro. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_ReadTotalsCash(bico:pchar):integer; 
• Parâmetros de entrada: 
➢ Bico: código do bico; 
• Retorno da função: 
➢ Encerrante: comando executado com sucesso; 
➢ -1: falha na execução do comando. 
4.7 Relógio 
4.7.1 Consultar calendário 
Esta função retorna o valor que está no relógio do concentrador. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_GetClock:pchar; 
• Parâmetros de entrada: nenhum. 
• Retorno da função: 
➢ Data no formato DD/MM/AA HH:MM. 
4.7.2 Ajustar calendário automaticamente 
Função para ajustar o relógio do concentrador utilizando a data e hora do PC. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_SetClock(par:pchar):boolean; 
Página 30 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• Parâmetros de entrada:  
08/03/2021 
➢ Par: ‘AUTO’ para o concentrador sincronizar com o relógio do computador 
ou uma strinf no formato DDHHMM. 
• Retorno da função: 
➢ True: comando executado com sucesso; 
➢ False: falha na execução do comando. 
4.8 Envio de comando 
4.8.1 Enviar comando nativo 
Função para realizar a comunicação com o concentrador utilizando comandos do protocolo. 
• Compatibilidade: Função compatível a partir da versão 5.0.0. 
• Estrutura: Function C_SendReceiveText(comando:shortstring):shortstring; 
• Parâmetros de entrada: 
➢ Comando: contém o comando no formato Companytec. 
• Retorno da função: 
➢ Retorna uma string no formato do protocolo de comunicação Companytec. 
5 Códigos de controle para identificadores 
5.1 Níveis de usuários 
O caractere X nas relações abaixo é apresentado como os níveis de usuários que podem ser 
dados aos tipos de cartões. 
• X1: Tag veículo; 
• X2: Tag máquina de lavar; 
• X3: Reservado; 
• X4: Cliente nível 1; 
• X5: Cliente nível 2; 
• X6: Cliente nível 3; 
• X7: Funcionário nível 1; 
• X8: Funcionário nível 2; 
• X9: Funcionário nível 3; 
• XA: Funcionário nível 4; 
• XB: Funcionário nível 5; 
• XC: Funcionário nível 6; 
• XD: Gerente nível 1; 
• XE: Gerente nível 2; 
Página 31 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
• XF: Controle total. 
5.2 Níveis de permissão 
Estas permissões representam o caractere “X” dos níveis de usuários. 
• 1: Reservado; 
• 2: Libera bomba; 
• 4: Respeita turno; 
• 6: Libera bomba / respeita turno; 
• 8: Libera máquina de lavar; 
• A: Libera bombas / libera máquina de lavar. 
6 Estrutura das respostas 
Abast 
08/03/2021 
• value: boolean; 
• total_dinheiro: currency; 
• total_litros: double; 
• PU: currency; 
• tempo: string[8]; 
• canal: string[2]; 
• data: string[10]; 
• hora: string[5]; 
• st_full: string[55]; 
• registro: integer; 
• encerrante: real; 
• integridade: boolean; 
• checksun: boolean. 
Página 32 de 35 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Manual de funções 
DLL - Protocolo Companytec 
DT 433 
Revisão: 04 
08/03/2021 
 
 
Página 33 de 35 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
AbastPAF1 
• value: boolean; 
• total_dinheiro: currency; 
• total_litros: double; 
• PU: currency; 
• tempo: string[8]; 
• codbico: string[2]; 
• numbico: integer; 
• numtanque: integer; 
• voltanque: integer; 
• codcombustivel: integer; 
• seriecbc: integer; 
• tipocbc: char; 
• data: string[10]; 
• hora: string[5]; 
• registro: integer; 
• encerranteI: double; 
• encerranteF: double; 
• integridade: boolean; 
• checksun: boolean; 
• tag1: string[16]; 
• tag2: string[16]. 
 
Online 
• litragem: array[1...48] of real; 
• bico: array[1…48] of string[2]. 
 
Multistatus 
• status: array[1…48] of StOptions. 
Manual de funções 
DT 433 
Revisão: 04 
DLL - Protocolo Companytec 
Encerrante 
08/03/2021 
• bico: string[2]; 
• valor: real. 
MemoryPointers 
• writePointer: string[4]; 
• readPointer: string[4].





Protocolo de comunicação 
Horustech 
Protocolo de 
comunicaça o 
Horustech 
DT 214 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 2 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
1 INTRODUÇÃO .............................................. 3 
2 ESTRUTURA DO COMANDO ......................... 3 
3 COMANDOS ................................................. 4 
3.1 ABASTECIMENTO ........................................ 4 
3.1.1 Leitura de Abastecimento ............. 4 
3.1.2 Leitura de abastecimento 
estendida........................................................ 5 
3.1.3 Leitura de registro de 
abastecimento ............................................... 7 
3.1.4 Incremento .................................... 8 
3.2 VISUALIZAÇÃO ........................................... 8 
3.2.1 Visualização .................................. 8 
3.2.2 Visualização identificada ............... 9 
3.3 IDENTFID................................................. 10 
3.3.1 Leitura de identificador ............... 10 
3.3.2 Gravação de identificador ........... 11 
3.3.3 Exclusão de identificador ............ 11 
3.3.4 Busca de identificadores ............. 12 
3.3.5 Leitura de registro de 
identificador…. ............................................. 13 
3.3.6 Gravação automática de 
identificadores ............................................. 14 
3.3.7 Incremento de identificador ........ 14 
3.3.8 Comando de lista negra .............. 15 
3.4 STATUS ................................................... 16 
3.4.1 Status .......................................... 16 
3.4.2 Leitura de informações do 
equipamento ................................................ 17 
3.5 GERENCIAMENTO DE BOMBAS ..................... 20 
3.5.1 Leitura de totalizadores .............. 20 
3.5.2 Alteração de preço ...................... 22 
3.5.3 Alteração de preço com níveis .... 23 
3.5.4 Predeterminação (Preset) ........... 24 
3.5.5 Predeterminação identificada 
(Preset identificado) ..................................... 24 
3.5.6 Predeterminação estendida (Preset 
estendido) .................................................... 25 
3.5.7 Predeterminação identificada 
estendida (Preset identificado estendido) .... 26 
3.5.8 Modo de operação ...................... 27 
3.5.9 Tabela de bico ............................. 28 
3.6 RELÓGIO ................................................. 29 
3.6.1 Ajuste de calendário .................... 29 
3.6.2 Leitura de calendário................... 29 
3.7 GERENCIAMENTO DA AUTOMAÇÃO .............. 30 
3.7.1 Leitura de configurações ............. 30 
3.7.2 Diagnóstico ................................. 31 
3.7.3 Leitura de arquivos ...................... 33 
3.7.4 Leitura de índice do arquivo ........ 35 
3.7.5 Diagnóstico wireless .................... 36 
3.7.6 Leitura de porta(s) de comunicação 
utilizadas.. .................................................... 36 
4 CÓDIGOS DE ERRO .....................................37 
5 CÓDIGOS DE STATUS DE EXECUÇÃO ...........38 
6 CÓDIGOS DE TIPOS DE COMBUSTÍVEIS ......38 
7 CÓDIGOS DE TIPOS DE SENSORES ..............38 
8 CÓDIGOS DE MODELOS DE BOMBAS .........39 
9 CÓDIGOS DE FORMA DE TRABALHO DO 
SENSOR IDF ........................................................40 
10 CÁLCULO DE CHECKSUM ............................40 
 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
1 Introdução 
08/03/2021 
• Todos os comandos são iniciados com o caractere “> “, seguido de”?” para consulta e”!” 
para resposta da automação. 
• Após os caracteres de início de bloco, o protocolo informa o tamanho dos dados, em 
hexadecimal. 
• O campo de dados é dividido em duas partes, o índice do comando, informado em 
hexadecimal de dois caracteres (00...FF), e os parâmetros, que serão de tamanho 
variável, de acordo com a informação do cabeçalho. 
• Campos denominados “ICOM” são representados em decimal na faixa de 01 a 03, 
contendo dois caracteres em qualquer comando/resposta. 
• Campos denominados “Bomba” são representados em decimal na faixa de 1 a 4, 
representado em um caractere em qualquer comando/resposta; 
• Campos “bico” são representados em decimal, de 0 a 99; 
• Campos tanque e combustível são representados em decimal, de 0 a 99; 
• Campos “Conector” são representados em decimal, na faixa de 1 a 4 em decimal, sendo 
apresentado sempre com um caractere; 
• Um hexadecimal é representado como dois caracteres, como exemplo: o campo S [2] do 
tipo hexadecimal, possui um dado hexadecimal, o campo K [4] do tipo hexadecimal 
possui 2 hexadecimais. Exemplos de números hexadecimais '1D', '05', '3F', 'FF'. 
• O tamanho do comando é (CCCC) seria quantos caracteres o comando possuí retirando 
o seu checksum, exemplo: 
Comando: >?000402A1D7 
>? – Caractere delimitador e tipo do comando; 
0004 – Tamanho calculado 
2A1 – Camada de dados, possuí 4 caracteres, então o tamanho do comando é 
0004, este tamanho é verificado em decimal, mas quando incluído no 
comando é posto em hexadecimal; 
D7 – Cálculo do checksum do comando inteiro retirando o caractere delimitador. 
2 Estrutura do comando 
Todos os comandos são definidos como a figura abaixo, eles consistem em uma camada de 
transporte, uma região de dados e um checksum, respectivamente. 
Página 3 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
>PCCCCX.... KK 
1. Camada de transporte 
• Delimitador 
• P: Tipo do comando: 
➢ ?: Pergunta para a automação; 
➢ !: Resposta da automação. 
• C [4]: Tamanho do campo DATA em hexadecimal. 
2. Camada de dados 
• X [2...65535]: Dados do comando: 
➢ Tipo [2]: Índice do comando; 
➢ Parâmetros [0...65532]: Parâmetros auxiliares do comando. 
3. Checksum: 
08/03/2021 
• K [2]: Somatório dos valores ACSII dos caracteres do comando, sendo desprezado o byte 
mais significativo. 
3 Comandos 
3.1 Abastecimento 
3.1.1 Leitura de Abastecimento 
Este comando é utilizado para a leitura de abastecimento finalizado. 
• Tamanho: 02 (0x0002) 
• Índice: 02 (0x02) 
• Estrutura do comando: 
>?CCCC02KK 
➢ 02 [02]: Índice do comando.............................................................(hexadecimal) 
• Resposta: Este comando retornará até 107 caracteres, conforme demonstração abaixo. 
No caso de memória vazia, a resposta terá tamanho dois 
• Tamanho: 2 (0x0002) / 107 (0x006B) 
• Estrutura da resposta: 
>!CCCC02NNNNNNBBCCAATTTTTTLLLLLLPPPPXYZttttddmmyyhhnneeeeeeeeeeEEEEE
EEEEEiiiiiiiiiiiiiiiiIIIIIIIIIIIIIIIIaaaaaaaaKK 
➢ 02 [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Índice do abastecimento na memória da automação..................(decimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ C [02]: Código do combustível.................................................................(decimal) 
➢ A [02]: Número do tanque fornecedor do bico........................................(decimal) 
Página 4 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 5 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ T [06]: Valor total do abastecimento.......................................................(decimal) 
➢ L [06]: Volume total do abastecimento...................................................(decimal) 
➢ P [04]: Preço unitário do produto............................................................(decimal) 
➢ X [01]: Número de casas decimais do campo total (T[6]).........................(decimal) 
➢ Y [01]: Número de casas decimais do campo volume (L[6]).....................(decimal) 
➢ Z [01]: Número de casas decimais do campo preço unitário (P[4])..........(decimal) 
➢ t [04]: Tempo de duração do abastecimento...........................................(decimal) 
➢ d [02]: Dia do abastecimento..................................................................(decimal) 
➢ m [02]: Mês do abastecimento................................................................(decimal) 
➢ y [02]: Ano do abastecimento..................................................................(decimal) 
➢ h [02]: Hora do abastecimento................................................................(decimal) 
➢ n [02]: Minuto do abastecimento............................................................(decimal) 
➢ e [10]: Totalizador inicial do abastecimento............................................(decimal) 
➢ E [10]: Totalizador final do abastecimento..............................................(decimal) 
➢ i [16]: Identificador de frentista...............................................................(decimal) 
➢ I [16]: Identificador de cliente.................................................................(decimal) 
➢ a [08]: Volume atual do tanque fornecedor de combustível....................(decimal) 
• Exemplo: 
TX: >?00020263 
RX:>!006B020006840300000001060010601000233000712022016400000080789000
0080895B3CF6CCFFF1FD792FFFFFFFFFFFFFFFF00000000D7 
• Resposta de quando não há abastecimento a ser lido: 
TX: >?00020263 
RX: >!00020245 
3.1.2 Leitura de abastecimento estendida 
Este comando é utilizado para a leitura de abastecimento com 8 dígitos no total a pagar e no 
campo de volume. 
• Tamanho: 04 (0x0004) 
• Índice: 02 (0x02) 
• Parâmetro: A1 
• Estrutura do comando:  
>?CCCC02A1KK 
➢ 02 [02]: Índice do comando.............................................................(hexadecimal) 
➢ A1 [02]: Parâmetro do comando......................................................(hexadecimal) 
• Resposta: Este comando retornará até 118 caracteres, conforme demonstração abaixo. 
No caso de memória vazia, a resposta terá tamanho dois 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 6 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
• Tamanho: 2 (0x0002) / 118 (0x0076) 
• Estrutura da resposta: 
>!CCCC02pNNNNNNBBCCAATTTTTTTTLLLLLLLLPPPPPPXYZttttddmmyyhhnnsseeeeeee
eeeEEEEEEEEEEiiiiiiiiiiiiiiiiIIIIIIIIIIIIIIIIaaaaaaaaaaKK 
➢ 02 [02]: Índice do comando.............................................................(hexadecimal) 
➢ p [01]: Parâmetro do comando.............................................................(caractere) 
➢ N [06]: Índice do abastecimento na memória da automação..................(decimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ C [02]: Código do combustível.................................................................(decimal) 
➢ A [02]: Número do tanque fornecedor do bico........................................(decimal) 
➢ T [08]: Valor total do abastecimento.......................................................(decimal) 
➢ L [08]: Volume total do abastecimento...................................................(decimal) 
➢ P [06]: Preço unitário do produto............................................................(decimal) 
➢ X [01]: Número de casas decimais do campo total(T[8])..........................(decimal) 
➢ Y [01]: Número de casas decimais do campo volume(L[8])......................(decimal) 
➢ Z [01]: Número de casas decimais do campo preço unitário(T[6])...........(decimal) 
➢ t [04]: Tempo de duração do abastecimento...........................................(decimal) 
➢ d [02]: Dia do abastecimento..................................................................(decimal) 
➢ m [02]: Mês do abastecimento................................................................(decimal) 
➢ y [02]: Ano do abastecimento..................................................................(decimal) 
➢ h [02]: Hora do abastecimento................................................................(decimal) 
➢ n [02]: Minuto do abastecimento............................................................(decimal) 
➢ s [02]: Segundos do abastecimento.........................................................(decimal) 
➢ e [10]: Totalizador inicial do abastecimento............................................(decimal) 
➢ E [10]: Totalizador final do abastecimento..............................................(decimal) 
➢ i [16]: Identificador de frentista...............................................................(decimal) 
➢ I [16]: Identificador de cliente.................................................................(decimal) 
➢ a [10]: Volume atual do tanque fornecedor de combustível....................(decimal) 
• Exemplo: 
TX: >?000402A1D7 
RX:>!0076021000684030000000001060000106000100023300071202201640290000
0807890000080895B3CF6CCFFF1FD792FFFFFFFFFFFFFFFF0000000000E8 
• Resposta de quando não há abastecimento a ser lido: 
TX: >?000402A1D7 
RX: >!00020245 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 7 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
3.1.3 Leitura de registro de abastecimento 
Este comando é utilizado para ler um abastecimento de uma determinada posição na 
memória da automação. 
• Tamanho: 08 (0x0008) 
• Índice: 08 (0x08) 
• Estrutura do comando: 
>?CCCC08NNNNNNKK 
➢ 08 [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Posição a ser consultada..............................................................(decimal) 
• Resposta: Este comando retornará a informação completa do abastecimento solicitado, 
totalizando 110 caracteres. Em caso de memória vazia, a resposta terá tamanho zero: 
>!0208KK 
• Tamanho: Variável 2 (0x0002) / 115 (0x0073) 
• Estrutura da resposta: 
>!CCCC08NNNNNNBBCCAATTTTTTLLLLLLPPPPXYZttttDDmmyyhhnneeeeeeeeeeEEEEE
EEEEEiiiiiiiiiiiiiiiiIIIIIIIIIIIIIIIIaaaaaaaaKK 
➢ 08 [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Índice do abastecimento na memória da automação..................(decimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ C [02]: Código do combustível.................................................................(decimal) 
➢ A [02]: Número do tanque fornecedor do bico........................................(decimal) 
➢ T [06]: Valor total do abastecimento.......................................................(decimal) 
➢ L [06]: Volume total do abastecimento...................................................(decimal) 
➢ P [04]: Preço unitário do produto............................................................(decimal) 
➢ X [01]: Número de casas decimais do campo total (T[6]).........................(decimal) 
➢ Y [01]: Número de casas decimais do campo volume (L[6]).....................(decimal) 
➢ Z [01]: Número de casas decimais do campo preço unitário (P[4])..........(decimal) 
➢ t [04]: Tempo do abastecimento.............................................................(decimal) 
➢ D [02]: Dia do abastecimento..................................................................(decimal) 
➢ m [02]: Mês do abastecimento................................................................(decimal) 
➢ y [02]: Ano do abastecimento..................................................................(decimal) 
➢ h [02]: Hora do abastecimento................................................................(decimal) 
➢ n [02]: Minuto do abastecimento............................................................(decimal) 
➢ e [10]: Totalizador inicial do abastecimento............................................(decimal) 
➢ E [10]: Totalizador final do abastecimento..............................................(decimal) 
➢ i [16]: Identificador de frentista.......................................................(hexadecimal) 
➢ I [16]: Identificador de cliente..........................................................(hexadecimal) 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
08/03/2021 
➢ a [08]: Volume atual do tanque fornecedor de combustível....................(decimal) 
• Exemplo: 
TX: >?00080800010595 
RX:>!006B080001050112000000290000241234223002929012113510873372806087
3372830E803341200000000E903785600000000000000001B 
3.1.4 Incremento 
Este comando é utilizado para passar o ponteiro de leitura para o próximo abastecimento. 
• Tamanho: 02 (0x0002) 
• Índice: 06 (0x06) 
• Estrutura do comando: 
>?CCCC06KK 
➢ 06 [02]: Índice do comando.............................................................(hexadecimal) 
• Resposta: Este comando retornará a posição do ponteiro de leitura e a posição do último 
ponteiro a ser lido na memória. Quando não há ponteiro a ser incrementado, a 
automação retorna os dois números iguais indicando, a posição lida e a última posição. 
• Tamanho: 14 (0x00E) 
• Estrutura da resposta: 
>!CCCC06nnnnnnNNNNNNKK 
➢ 06 [02]: Índice do comando.............................................................(hexadecimal) 
➢ n [06]: Posição para a qual o ponteiro foi movido....................................(decimal) 
➢ N [06]: Posição do último ponteiro da lista de abastecimentos...............(decimal) 
• Exemplo: 
TX: >?00020667 
RX: >!000E06000179000181B7 
• Exemplo quando não há ponteiro a ser incrementado: 
TX: >?00020667 
RX: >!000E06000181000181B0 
3.2 Visualização 
3.2.1 Visualização 
Este comando é utilizado para leitura dos abastecimentos em andamento. 
• Tamanho: 02 (0x0002) 
• Índice: 03 (0x03) 
• Estrutura do comando: 
>?CCCC03KK 
➢ 03 [02]: Índice do comando.............................................................(hexadecimal) 
Página 8 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
08/03/2021 
• Resposta: O comando de visualização retornará oito caracteres para cada bico que 
estiver abastecendo no momento da consulta, sendo dois caracteres informando o bico 
e seis caracteres informando o valor em dinheiro. 
• Tamanho: Variável [2...65353]([0x0002...0xFFFF]) 
• Estrutura da resposta: 
>!CCCC03BBLLLLLLKK 
➢ 03 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ L [06]: Valor em dinheiro fornecido no momento da consulta.................(decimal) 
• Exemplo: 
TX: >?00020364 
RX: >!000A0301001222DD 
• Exemplo de quando não há abastecimento em andamento: 
TX: >?00020364 
RX: >!00020346 
3.2.2 Visualização identificada 
Este comando é utilizado para ler o total a pagar e identificador do abastecimento em 
andamento. 
• Tamanho: 02 (0x0002) 
• Índice: 40 (0x28) 
• Estrutura do comando: 
>?CCCC28KK 
➢ 28 [02]: Índice do comando.............................................................(hexadecimal) 
• Resposta: Este comando retornará à informação do total a pagar e identificador de 
todos os bicos com abastecimento em andamento no momento da solicitação. Para 
cada abastecimento em andamento, a automação irá retornar a estrutura abaixo. 
• Tamanho: Variável de acordo com o número de bicos com abastecimento em 
andamento. 
• Estrutura da resposta: 
>!CCCC28BBVVVVVVIIIIIIIIIIIIIIIIKK (V e I = 24 bytes por bico abastecendo) 
➢ 28 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ V [06]: Volume abastecendo...................................................................(decimal) 
➢ I [16]: Código do TAG que liberou o fornecimento...................................(decimal) 
• Exemplo: 
TX: >?0002286B 
Página 9 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 10 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
RX: >!001A2801001763B32800000000001211 
• Exemplo com dois abastecimentos em andamento: 
TX: >?0002286B 
RX: >!00322803000587B32800000000000104000064B328000000000002B6 
• Exemplo de quando não há abastecimento em andamento: 
TX: >?0002286B 
RX: >!0002284D 
3.3 Identfid 
3.3.1 Leitura de identificador 
Este comando é utilizado para ler o código de identificadores lidos pelos sensores, mas a 
automação não sabe o que fazer com ele (ex.: O cartão não está cadastrado na automação). 
Quando a automação está trabalhando desta forma, ela coloca os cartões não cadastrados em 
um buffer e aguarda algum comando para tomar a providência necessária com os mesmos. 
• Tamanho: 02 (0x0002) 
• Índice: 12 (0x0C) 
• Estrutura do comando: 
>?CCCC0CKK 
➢ 0C [02]: Índice comando..................................................................(hexadecimal) 
• Resposta: Este comando retornará a informação completa da identificação lida no 
sensor ou informação de memória vazia. As informações de calendário é do momento 
que o cartão foi passado no sensor. 
• Tamanho: 40 (0x0028) 
• Estrutura da resposta: 
>!CCCC0CNNNNNNaabbccddTTTTTTTTTTTTTTTTDDMMHHNNKK 
➢ 0C [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Posição da leitura na memória.....................................................(decimal) 
➢ aabbccdd [08]: Lista de bicos sob controle do sensor lido.......................(decimal) 
o aa: Bico A; 
o bb: Bico B; 
o cc: Bico C; 
o dd: Bico D; 
➢ T [16]: Código do identificador........................................................(hexadecimal) 
➢ D [02]: Dia................................................................................................(decimal) 
➢ M [02]: Mês.............................................................................................(decimal) 
➢ H [02]: Hora.............................................................................................(decimal) 
➢ N [02]: Minuto.........................................................................................(decimal) 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
• Exemplo: 
TX: >?00020C74 
RX: >!00280C00000001000000B3CF6CCFFF1F51E60102110169 
3.3.2 Gravação de identificador 
08/03/2021 
Este comando é utilizado para gravar códigos de identificadores na memória da automação, 
após este procedimento, os cartões reconhecidos pelo equipamento, podem liberar qualquer 
bico para abastecimento sem intervenção do computador, sendo assim, as liberações passam a 
ser controladas pela automação. 
• Tamanho: 20 (0x0014) 
• Índice: 13 (0x0D) 
• Estrutura do comando: 
>?CCCC0DTTTTTTTTTTTTTTTTLLKK 
➢ 0D [02]: Índice do comando.............................................................(hexadecimal) 
➢ T [16]: Código de identificador.........................................................(hexadecimal) 
➢ L [02]: Controle........................................................................................(decimal) 
o 27: Libera bomba para abastecimento; 
o 04: Cliente, não libera bomba para abastecimento; 
• Resposta: Este comando retornará à posição do identificador em memória ou um código 
de erro caso não tenha sido possível realizar a gravação. 
• Tamanho: 24 (0x0018) / 4 (0x0004) / {Erro} 
• Estrutura da resposta: 
>!CCCC0DNNNNNNTTTTTTTTTTTTTTTTKK  
➢ 0D [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Posição em que o identificador foi armazenado..........................(decimal) 
➢ T [16]: Código do identificador armazenado....................................(hexadecimal) 
>!CCCC0DCDKK 
➢ 0D [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00140DB3280000000000252707 
RX: >!00180D000001B328000000000025A5 
3.3.3 Exclusão de identificador 
Este comando é utilizado para apagar códigos de identificadores na memória da automação. 
• Tamanho: 24 (0x0018) 
Página 11 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
• Índice: 14 (0x0E) 
• Estrutura do comando: 
>?CCCC0ENNNNNNTTTTTTTTTTTTTTTTKK 
08/03/2021 
➢ 0E [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Posição do identificador na memória..........................................(decimal) 
➢ T [16]: Código de identificador a ser deletado.................................(hexadecimal) 
• Resposta: Este comando retornará um caractere informando a situação da execução do 
comando. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC0ECDKK 
➢ 0E [02]: Índice do comando.....................................................................(hexadecimal) 
➢ C [01]: Códigos de Status de execução.........................................................(caractere) 
➢ D [01]: Código do erro.............................................................................(hexadecimal) 
• Exemplo: 
TX: >?00180E000001B328000000000025C4 
RX: >!00040E00BA 
3.3.4 Busca de identificadores 
Este comando é utilizado para localizar códigos de identificadores na memória da 
automação. Lembrando que a posição 000000 da memória da automação também guarda um 
identificador, então caso queira ler a posição 0007 da memória, é necessário enviar a posição 
0006. 
• Tamanho: 08 (0x0008) 
• Índice: 15 (0x0F) 
• Estrutura do comando: 
>?CCCC0FNNNNNNKK 
➢ 0F [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Índice da posição.........................................................................(decimal) 
• Resposta: Este comando retornará a linha do arquivo de identificadores solicitada com 
as informações separadas por “;”. 
• Tamanho: Quando encontra o sensor não retorna tamanho e sim a estrutura indicada 
abaixo. Em caso de espaço da memória sem cartão, o tamanho será 02 (0x0002). 
• Estrutura da resposta em casos que não há identificador no espaço de memória: 
>!CCCC0ACDKK 
➢ 0F [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
Página 12 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 13 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ D [01]: Código do erro.....................................................................(hexadecimal) 
• Exemplo: 
➢ Com cartão cadastrado na automação: 
TX: >?00080F0000029F 
RX: 000002; B328000000000025; 27:CARD ATTENDANT 1 L; 11; FFFF; FFFFFFFF; 
B328000000000025; 00,00; 00000000; 
➢ Com espaço da memória sem cartão: 
TX: >?00080F000003A0 
RX: >!00020F59 
3.3.5 Leitura de registro de identificador 
Este comando é utilizado para obter todos os dados relativos a um identificador presente na 
memória. 
• Tamanho: 08 (0x0008) 
• Índice: 16 (0x10) 
• Estrutura do comando: 
>?CCCC10NNNNNNKK 
➢ 10 [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Índice do registro do identificador...............................................(decimal) 
• Resposta: Este comando retornará os dados relativos ao identificador em memória ou 
um código de erro caso não tenha sido possível encontrar. 
• Tamanho: 40 (0x0028) 
• Estrutura da resposta: 
>!CCCC10NNNNNNCCCCCCCCTTTTTTTTTTTTTTTTDDMMHHmmKK 
➢ 10 [02]: Índice do comando.............................................................(hexadecimal) 
➢ N [06]: Posição do identificador na memória..........................................(decimal) 
➢ C [08]: Código dos bicos ('00' caso não haja tal bico)...............................(decimal) 
➢ T [16]: Código do identificador........................................................(hexadecimal) 
➢ D [02]: Dia................................................................................................(decimal) 
➢ M [02]: Mês.............................................................................................(decimal) 
➢ H [02]: Hora.............................................................................................(decimal) 
➢ m [02]: Minuto........................................................................................(decimal) 
• Exemplo: 
TX: >?0008100000028A 
RX: >!00281000000201000000B3CF6CCFFF1F51E6010211045C 
• Exemplo de espaço de memória vazio: 
TX: >?0008100000028A 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
RX: >!00281000FFFF00000000FFFFFFFFFFFFFFFFFFFFFFFFD4 
3.3.6 Gravação automática de identificadores 
08/03/2021 
Este comando é utilizado para colocar a automação em modo de gravação automática de 
identificadores, neste modo, todos os cartões lidos sensores, são diretamente gravados na 
memória da automação. 
• Tamanho: 06 (0x0006) 
• Índice: 23 (0x17) 
• Estrutura do comando: 
>?CCCC17IITTKK 
➢ 17 [02]: Índice do comando.............................................................(hexadecimal) 
➢ I [02]: Funcionalidade......................................................................(hexadecimal) 
o 5A: Inicia; 
o 5B: Finaliza; 
o 5C: Apaga identificadores armazenados (comando somente possível com 
certificado logado). 
➢ T [02]: Controle.......................................................................................(decimal) 
o 24: Cliente (Não libera a bomba); 
o 27: Frentista (Libera bomba). 
• Resposta: Este comando retornará um código de confirmação de execução. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC17CDKK 
➢ 17 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
➢ Limpeza de memória: 
TX: >?0006175C274E 
RX: >!00041700AD 
3.3.7 Incremento de identificador 
Este comando é utilizado para passar a leitura para o próximo identificador lido. 
• Tamanho:  02 (0x0002) 
• Índice: 24 (0x18) 
• Estrutura do comando: 
>?CCCC18KK 
Página 14 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
08/03/2021 
➢ 18 [2]: Índice do comando...............................................................(hexadecimal) 
• Resposta: Este comando retornará à posição para qual o ponteiro foi movido e a posição 
atual. Quando não há identificador a ser lido, a automação retorna os dois números 
iguais, a posição do sensor lido e a última posição do buffer. 
• Tamanho: 14 (0x000E) 
• Estrutura da resposta: 
>!CCCC18nnnnnnNNNNNN 
➢ 18 [02]: Índice do comando.............................................................(hexadecimal) 
➢ n [06]: Posição para a qual o ponteiro foi movido....................................(decimal) 
➢ N [06]: Posição na qual encontra-se o ponteiro do final da lista..............(decimal) 
• Exemplo: 
TX: >?0002186A 
RX: >!000E18000002000002A3 
3.3.8 Comando de lista negra 
Este comando é utilizado para colocar ou retirar cartões identfid de uma lista que faz com 
que o console não os reconheça temporariamente, ou seja, quando um cartão identfid está na 
lista negra, ele não pode liberar um abastecimento, assim este cartão é remetido a lista de 
cartões pendentes para o sistema tomar a decisão em relação ao mesmo. Esta lista possuí 20 
posições. 
• Tamanho: 04 (0x0004) / 20 (0x0014) 
• Índice: 48 (0x30) 
• Estrutura do comando:  
>?CCCC30MMIIIIIIIIIIIIIIIIKK  
➢ 30 [02]: Índice do comando.............................................................(hexadecimal) 
➢ M [02]: Controle......................................................................................(decimal) 
o 00: Apaga a lista; 
o 01: Põe o cartão na lista; 
o 02: Remove o cartão da lista. 
➢ I [16]: Código do cartão a ser colocado ou removido da lista (não é necessário 
na opção ‘00’)..................................................................................(hexadecimal) 
• Resposta: Este comando retornará um código de confirmação ou mensagem de erro se 
a lista estiver cheia para a opção 01. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC30CDKK  
➢ 30 [02]: Índice do comando.............................................................(hexadecimal) 
Página 15 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
08/03/2021 
➢ C [01]: Códigos de Status de execução (‘E’ se erro, ‘0’ se OK)................(caractere) 
➢ D [01]: Código do erro (não importa se OK).....................................(hexadecimal) 
• Exemplo para adicionar um cartão a lista negra: 
TX: >?00143001B328000000000089F8 
RX: >!00043000A8 
• Exemplo de limpeza da lista negra: 
TX: >?00043000C6 
RX: >!00043000A8 
3.4 Status 
3.4.1 Status 
Este comando é utilizado para ler as informações de estado de cada bico no momento da 
requisição. 
• Tamanho: 02 (0x0002) 
• Índice: 01 (0x01) 
• Estrutura do comando: 
>?CCCC01KK 
➢ 01 [02]:Índice do comando..............................................................(hexadecimal) 
• Resposta: A resposta do comando STATUS retornará até 101 caracteres, sendo dois 
caracteres descrevendo o índice do comando e um caractere para cada bico configurado 
na automação, assim, o tamanho da resposta é variável. Bicos não configurados 
retornarão um espaço em branco. 
• Tamanho: Variável 2 (0x0002) ...101 (0x0065) 
• Estrutura da resposta: 
>!CCCC01FFFFFFFFKK 
➢ 01 [02]: Índice do comando.............................................................(hexadecimal) 
➢ F [00...99]: Caractere de estado..............................................................(caracter) 
o B: Bico bloqueado; 
o L: Bico livre; 
o A: Bico Abastecendo; 
o F: Bico em falha; 
o <SPACE>: Bico não configurado; 
o E: Bico em espera; 
o P: Bico pronto para abastecer; 
o #: Bico Ocupado (outro bico na bomba está abastecendo); 
o !: Erro genérico. 
• Exemplo: 
Página 16 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
TX: >?00020162 
RX: >!0A01AALB P AKK 
• No exemplo acima, o maior bico do cenário é o de número oito. 
• Os bicos um, dois e oito estão abastecendo. 
• O bico três está livre e o quatro está bloqueado. 
• Os bicos cinco e sete não estão configurados na automação. 
• O bico seis está pronto para abastecer. 
3.4.2 Leitura de informações do equipamento 
08/03/2021 
Este comando é utilizado para obter todos os dados relativos ao equipamento, como versão 
de software, status de bateria, etc. 
• Tamanho: 02 (0x0002) 
• Índice: 18 (0x12) 
• Estrutura do comando: 
>?CCCC12KK 
➢ 12 [02]: Índice do comando.............................................................(hexadecimal) 
• Resposta: Este comando retornará as informações relativas ao equipamento. 
• Tamanho: 182 (0x00B6) 
• Estrutura da resposta: 
>!CCCC12vVV.VV fFF.FF DD/MM/AA B bbbbb E eeee C-NNNNNNNN DD/MM/AA 
DD/MM/AA 
MM:MM:MM:MM:MM:MM 
III.III.III.III;DD/MM/AA 
dflt 
CCCCCCCC;FIIIHDPT;RRR.RRR.RRR.RRR;ppppp;a;w;QQQ.QQQ.QQQ.QQQ;rrrrr;KK 
➢ 12 [02]: Índice do tipo do comando.................................................(hexadecimal) 
➢ v [01]: ‘B’ Descritor de versão boot-loader...........................................(caractere) 
➢ V [05]: Versão do boot-loader.................................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ f [01]: ‘F’ Descritor de versão do firmware............................................(caractere) 
➢ F [05]: Versão do firmware......................................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ DD/MM/AA [08]: Data do firmware........................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ B [01]: Informação da bateria..................................................................(decimal) 
o 0: Bateria normal; 
o 1: Bateria baixa; 
o 2: Bateria em estado crítico (providenciar troca). 
➢ <SPACE> [01]: Espaço separador. 
➢ b [05]: Tensão da bateria.........................................................................(decimal) 
Página 17 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 18 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ <SPACE> [01]: Espaço separador. 
➢ E [01]: Informação da rede externa.........................................................(decimal) 
o 0: Desligado; 
o 1: Baixo; 
o 2: Normal; 
o 3: Alto. 
➢ <SPACE> [01]: Espaço separador. 
➢ e [04]: Tensão da rede externa................................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ C [01]: Caractere tipo de Permissões....................................................(caractere) 
➢ <-> [01]: Caractere separador...............................................................(caractere) 
➢ N [08]: Número Serial..............................................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ DD/MM/AA [08]: Data da Fabricação......................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ DD/MM/AA [08]: Última data Válida.......................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ M [17]: MAC............................................................................................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ I [12]: IP...................................................................................................(decimal) 
➢ < ; > [01]: Caractere separador. 
➢ DD/MM/AA [08]: Data validade do certificado (se existir).......................(decimal) 
➢ <SPACE> [01]: Espaço separador. 
➢ < d > [01]: Tipo do IP..............................................................................(caractere) 
o D:DHCP Dinâmico. 
o F: IP Fixo. 
➢ < f > [01]: Protocolo Ativado.................................................................(caractere) 
o C:Companytec; 
o c: CBC; 
o P: PAN; 
o D: Desativado. 
➢ < l > [01]: Login de certificado...............................................................(caractere) 
o L: certificado Logado; 
o <SPACE>: Certificado não logado. 
➢ < t > [01]: Travamento de certificado....................................................(caractere) 
o T: Certificado travado; 
o <SPACE>: Certificado não está travado. 
➢ C [08]: Código do certificado logado ou certificado que está travado......(decimal) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 19 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ < ; > [01]: Caractere separador. 
➢ < F > [01]: Tipo de fonte (a, b, c)............................................................(caractere) 
➢ < I > [03]: Tipo de ICOM (um caractere por ICOM).................................(caractere) 
o 0: ICOM Normal; 
o 6: ICOM ISSO 12; 
o 5: ICOM ISSO 24; 
o 9: ICOM ISSO SW. 
➢ H [01]: Tipo de automação....................................................................(caractere) 
o H: Horustech; 
o M: H4. 
➢ D [01]: Status da bateria.......................................................................(caractere) 
o D: bateria presente; 
o F: bateria ausente; 
o i: bateria invertida. 
➢ P [01]: Troca de preço por cartão IDF....................................................(caractere) 
o P: Ativada; 
o N: Desativada. 
➢ T [01]: Integração com medidor de tanque...........................................(caractere) 
o V: Ativada; 
o N: Desativada. 
➢ < ; > [01]: Caractere separador. 
➢ R [12]: IP para configuração medidor......................................................(decimal) 
➢ < ; > [01]: Caractere separador. 
➢ p [05]: Porta para configuração medidor.................................................(decimal) 
➢ < ; > [01]: Caractere separador. 
➢ a [01]: Caractere de ativação para comunicação medidor....................(caractere) 
o D: cliente não conectado; 
o C: cliente conectado; 
o E: erro. 
➢ < ; > [01]: Caractere separador. 
➢ w [01]: Configuração com 4 ou até 6 posições.........................................(decimal) 
➢ < ; > [01]: Caractere separador. 
➢ Q [12]: IP para configuração MQTT.........................................................(decimal) 
➢ < ; > [01]: Caractere separador. 
➢ R [05]: Porta para configuração MQTT....................................................(decimal) 
➢ < ; > [01]: Caractere separador. 
• Exemplo: 
TX: >?00021264 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 20 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
RX: >!00B612B01.00 F08.05 10/06/20 0 14,21 2 0110 i-00018058 13/02/20 25/06/20 
00:26:28:11:80:58 192.168.000.089;31/08/23 FcL 
00000001;c900HLNN;192.168.000.070;00080;D;4;192.168.000.233;10002;4D 
3.5 Gerenciamento de bombas 
3.5.1 Leitura de totalizadores 
Este comando é utilizado para leitura dos dados de totais de um determinado bico. 
• Tamanho: 06 (0x0006) 
• Índice: 05 (0x05) 
• Estrutura do comando: 
>?CCCC05BBTTKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Bico a ser consultado....................................................................(decimal) 
➢ T [02]: Tipo de pedido..............................................................................(decimal) 
o 01: Litros; 
o 02: Valor em dinheiro; 
o 03: Preço unitário; 
o 04: Completo; 
o 05: Identificadores; 
o 06: Ponteiros (válido apenas para bico “00”); 
o 08: Totalizador de volume, valor e os três preços configurados; 
o 09: Valor do preço unitário dos três níveis. 
• Resposta: Este comando retornará os valores correspondentes aos totalizadores do 
bico. O caractere de tipo define qual informação deve ser lida. 
• Tamanho: Variável 4 (0x0004) / 10 (0x000A) / 16 (0x0010) / 24 (0x0018) / 30 (0x001E) / 
44 (0x002C) 
• Estrutura da resposta: 
▪ Erro na resposta de totais (0x0004): 
>!CCCC05ECKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ E [01]: Indicador de erro.......................................................................(caractere) 
➢ C [01]: Código de erro......................................................................(hexadecimal) 
▪ Resposta de totais em volume (0x0016): 
>!CCCC05BBTTLLLLLLLLLLKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 21 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ L [10]: Totalizador em litros no momento da consulta............................(decimal) 
• Exemplos: 
TX: >?00060501012C 
RX: >!0010050101000129985813 
▪ Resposta de totais em valor (0x0016): 
>!CCCC05BBTTVVVVVVVVVVKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
➢ V [10]: Totalizador em dinheiro no momento da consulta......................(decimal) 
• Exemplo: 
TX: >?00060501022D 
RX: >!00100501020000000000EA 
▪ Resposta de consulta de preço unitário (0x0014): 
>!CCCC05BBTTPPPPXXXXKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
➢ P [04]: Preço unitário praticado pelo bico no momento da consulta.......(decimal) 
➢ X [04]: Preço unitário anterior.................................................................(decimal) 
• Exemplo: 
TX: >?00060501032E 
RX: >!000E05010311110000A3 
▪ Resposta de consulta completa de totais (0x0030): 
>!CCCC05BBTTLLLLLLLLLLVVVVVVVVVVPPPPKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
➢ L [10]: Totalizador em litros no momento da consulta............................(decimal) 
➢ V [10]: Totalizador em dinheiro no momento da consulta......................(decimal) 
➢ P [04]: Preço unitário praticado pelo bico no momento da consulta.......(decimal) 
• Exemplo: 
TX: >?00060501042F 
RX: >!001E050104000129985800000000001111CF 
▪ Resposta de leitura de preço por litro dos três níveis (0x0024): 
>!CCCC05BBTTFFFFFFGGGGGGHHHHHHKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 22 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
➢ F [06]: Preço nível 0 (dinheiro ou à vista).................................................(decimal) 
➢ G [06]: Preço nível 1 (crédito)..................................................................(decimal) 
➢ H [06]: Preço nível 2 (débito)...................................................................(decimal) 
• Exemplo: 
TX: >?000605010934 
RX: >!001805010900111100222200333391 
▪ Resposta de consulta de ponteiros (0x0030): 
>!CCCC05BBTTAAAAAABBBBBBCCCCCCDDDDDDKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
➢ A [06]: Ponteiro POP do abastecimento..................................................(decimal) 
➢ B [06]: Ponteiro PUSH do abastecimento................................................(decimal) 
➢ C [06]: Ponteiro POP de identificador......................................................(decimal) 
➢ D [06]: Ponteiro PUSH de identificador...................................................(decimal) 
• Exemplo: 
TX: >?000605010530 
RX: >!00260501050000000000000000000000000000000014 
▪ Resposta de totalizadores e os três preços configurados (0x0044): 
>!CCCC05BBTTLLLLLLLLLLVVVVVVVVVVFFFFFFGGGGGGHHHHHHKK 
➢ 05 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [02]: Tipo de pedido.............................................................................(decimal) 
➢ L [10]: Totalizador em litros no momento da consulta............................(decimal) 
➢ V [10]: Totalizador em dinheiro no momento da consulta......................(decimal) 
➢ F [06]: Preço nível 0 (dinheiro ou à vista).................................................(decimal) 
➢ G [06]: Preço nível 1 (crédito)..................................................................(decimal) 
➢ H [06]: Preço nível 2 (débito)...................................................................(decimal) 
• Exemplo: 
TX: >?000605410837 
RX: >!002C054108000007435100004250390011110022220033338B 
3.5.2 Alteração de preço 
Este comando é utilizado para alterar o preço unitário do bico. O preço será atualizado no 
display da bomba somente após o bico ser retirado do repouso. 
• Tamanho: 08 (0x0008) 
• Índice: 07 (0x07) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 23 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
• Estrutura do comando: 
>?CCCC07BBPPPPKK 
➢ 07 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Bico..............................................................................................(decimal) 
➢ P [04]: Novo preço (somente números)...................................................(decimal) 
• Resposta: Este comando retornará um código informando a situação do pedido. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC07CDKK 
➢ 07 [2]: Índice do comando...............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00080701123499 
RX: >!00040700AC 
3.5.3 Alteração de preço com níveis 
Este comando é utilizado para alterar o preço de cada bico utilizando seu nível. 
• Tamanho: 11 (0x000B) 
• Índice: 50 (0x32) 
• Estrutura do comando: 
>?CCCC32BBNPPPPPPKK 
➢ 32 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ N [01]: Nível de preço...........................................................................(caractere) 
➢ P [06]: Novo preço (somente números)...................................................(decimal) 
• Resposta: Este comando retornará um código informando a situação do pedido. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC32CDKK 
➢ 32 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?000B320200011112C 
RX: >!00043200AA 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 24 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
3.5.4 Predeterminação (Preset) 
Este comando é utilizado para autorizar um abastecimento com valor máximo definido. 
• Tamanho: 10 (0x000A) 
• Índice: 09 (0x09) 
• Estrutura do comando: 
>?CCCC09BBVVVVVVKK 
➢ 09 [2]: Índice do comando...............................................................(hexadecimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ V [06]: Valor pré-determinado................................................................(decimal) 
• Resposta: Este comando retornará um caractere informando a situação da execução do 
comando. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC09CDKK 
➢ 09 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo:  
TX: >?000A0901000050FF 
RX: >!00040900AE 
 Atenção: Alguns modelos de bombas não são compatíveis com esse comando, nesse 
caso, a bomba será liberada normalmente para abastecimento, porém, o abastecimento não 
finalizará no valor previamente definido. 
3.5.5 Predeterminação identificada (Preset identificado) 
Este comando é utilizado para autorizar o bico e determinar o código identfid responsável 
pelo próximo abastecimento. 
• Tamanho: 36 (0x0024) 
• Índice: 38 (0x26) 
• Estrutura do comando: 
>?CCCC26BBTTTTTTTTTTTTTTTTCAPPPPPPttpRRRRRKK 
➢ 26 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico............................................................................(decimal) 
➢ T [16]: Código do identificador........................................................(hexadecimal) 
➢ C [01]: Tipo do identificador....................................................................(decimal) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 25 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
o 0: Frentista; 
o 1: Cliente; 
o 2: Hodômetro. 
➢ A [01]: Autorização...............................................................................(caractere) 
o S: Sim; 
o N: Não. 
➢ P [06]: Valor do PRESET (0 = Sem limite)..................................................(decimal) 
➢ t [02]: Tempo até retirar o bico................................................................(decimal) 
➢ p [01]: Tipo de preset............................................................................(caractere) 
o $: Dinheiro; 
o V: Volume. 
➢ R [05]: Reservado (preencher com 0 (zero))............................................(decimal) 
• Resposta: Este comando retornará um código de confirmação. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC26CDKK 
➢ 26 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00242601B3280000000000120S00100030$000000B 
RX: >!00042600AD 
3.5.6 Predeterminação estendida (Preset estendido) 
Este comando é utilizado para predeterminar um valor ou volume com 8 dígitos, além da 
informação de nível do preço. 
• Tamanho: 14 (0x000E) 
• Índice: 49 (0x31) 
• Estrutura do comando: 
>?CCCC31BBVVVVVVVVxnKK 
➢ 31 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ V [08]: Valor a ser predeterminado.........................................................(decimal) 
➢ x [01]: Tipo de preset...............................................................................(decimal) 
o 0: Total a pagar; 
o 1: Volume. 
➢ n [01]: Nível do valor...............................................................................(decimal) 
o 0: À vista; 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 26 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
o 1: Crédito; 
o 2: Débito. 
• Resposta: Este comando retornará um código de confirmação. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC31CDKK 
➢ 31 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?000E31010000100000BA 
RX: >!00043100A9 
3.5.7 Predeterminação identificada estendida (Preset identificado estendido) 
Este comando é utilizado para predeterminar um valor ou volume com 8 dígitos com 
identificador e nível de preço. 
• Tamanho: 34 (0x0022) 
• Índice: 52 (0x34) 
• Estrutura do comando: 
>?CCCC34BBTTTTTTTTTTTTTTTTCAPPPPPPPPttpnKK 
➢ 34 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
➢ T [16]: Código do identificador........................................................(hexadecimal) 
➢ C [01]: Tipo do identificador....................................................................(decimal) 
o 0: Frentista; 
o 1: Cliente. 
➢ A [01]: Autorização...............................................................................(caractere) 
o S: Sim; 
o N: Não. 
➢ P [08]: Valor do preset (0 = sem limite)....................................................(decimal) 
➢ t [02]: Tempo até retirar o bico................................................................(decimal) 
➢ p [01]: Tipo de preset............................................................................(caractere) 
o $: Dinheiro; 
o V: Volume. 
➢ Nível de preço.........................................................................................(decimal) 
o 0: Dinheiro ou à vista; 
o 1: Crédito; 
o 2: Débito. 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
• Resposta: Este comando retornará um código de confirmação. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC34CDKK 
08/03/2021 
➢ 34 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00223401B3280000000000010S0000100030$0A6 
RX: >!00043400AC 
3.5.8 Modo de operação 
Este comando é utilizado para alterar a maneira que a automação gerencia as solicitações de 
abastecimento das bombas. 
▪ Em modo “B” (bloqueio), ao retirar o bico para abastecimento, a bomba entrará no 
estado de “E” (espera), assim, para permitir que esse abastecimento seja realizado é 
necessário enviar o comando de modo “A” (autoriza) para que a automação autorize 
o bico e após esse abastecimento, o bico voltará ao estado anterior. 
▪ Em modo “L” (livre), ao receber uma solicitação do bico para abastecer, a automação 
autoriza automaticamente o bico para abastecer. 
• Tamanho: 06 (0x0006) 
• Índice: 19 (0x13) 
• Estrutura do comando: 
>?CCCC13BBMMKK 
➢ 13 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Bico..............................................................................................(decimal) 
➢ M [02]: Modo..........................................................................................(decimal) 
o 01: Automação autoriza o bico para abastecimento sempre que solicitado 
(auto liberação); 
o 02: Automação autoriza o bico para somente um abastecimento; 
o 03: Automação configura o bico em modo bloqueado; 
o 04: Interrompe o abastecimento no momento do comando (para bombas 
que permitem stop); 
o 05: Pausa o abastecimento (para bombas que permitem) se for enviado um 
comando tipo 2 (autoriza) a bomba volta a despachar; 
o 06: Habilita Identificador; 
o 07: Inibe identificador; 
o 10: Clear, limpa autorização e Códigos de Identificação; 
Página 27 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 28 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
• Resposta: Este comando retornará um caractere informando a situação da execução do 
comando. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC13CDKK 
➢ 13 [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00061301032D 
RX: >!00041300A9 
 
TX: >?00061301012B 
RX: >!00041300A9 
3.5.9 Tabela de bico 
Este comando é utilizado para informar dados do bico escolhido, como código do bico na 
automação, número do tanque atribuído e código do produto. 
• Tamanho: 04 (0x0004) 
• Índice: 20 (0x14) 
• Estrutura do comando: 
>?CCCC14BBKK 
➢ 14 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número do bico............................................................................(decimal) 
• Resposta: Este comando retornará os dados relativos ao bico, conforme configurado no 
comando de configuração. 
• Tamanho: 12 (0x000C) 
• Estrutura da resposta: 
>!CCCC14BBIIcbttppKK 
➢ 14 [02]: Índice do comando.............................................................(hexadecimal) 
➢ B [02]: Número de bico na pista...............................................................(decimal) 
➢ I [02]: Canal (1-12)...................................................................................(decimal) 
➢ c [02]: Endereço (1-4)............................................................................(caractere) 
➢ b [02]: Posição(1-4).................................................................................(decimal) 
➢ t [02]: Número do tanque........................................................................(decimal) 
➢ p [02]: Código do produto(códigos de tipo).....................................(hexadecimal) 
• Exemplo: 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 29 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
TX: >?00041401C9 
RX: >!000E14010101000101A0; 
• Estrutura de Resposta de Erro: 
>!CCCC14CDKK 
➢ 14 [02]: Índice do comando.....................................................................(hexadecimal) 
➢ C [01]: Códigos de Status de execução.........................................................(caractere) 
➢ D [01]: Código do erro.............................................................................(hexadecimal) 
3.6 Relógio 
3.6.1 Ajuste de calendário 
Este comando é utilizado para ajustar as definições de data e hora do equipamento. 
• Tamanho: 16 (0x0010) 
• Índice: 10 (0x0A) 
• Estrutura do comando: 
>?CCCC0AYYMMDDddHHNNSSKK 
➢ 0A [02]: Índice do comando.............................................................(hexadecimal) 
➢ Y [02]: Ano...............................................................................................(decimal) 
➢ M [02]: Mês.............................................................................................(decimal) 
• D [02]: Dia................................................................................................(decimal) 
• d [02]: Dia da semana (01=domingo, 02=segunda, ...).............................(decimal) 
• H [02]: Hora.............................................................................................(decimal) 
• N [02]: Minuto.........................................................................................(decimal) 
• S [02]: Segundos......................................................................................(decimal) 
• Resposta: Esse comando retornará um caractere informando a situação da execução do 
comando. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC0ACDKK 
➢ 0A [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [01]: Códigos de status de execução..................................................(caractere) 
➢ D [01]: Código do erro......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00100A120727061549003D 
RX: >!00040A00B6 
3.6.2 Leitura de calendário 
Este comando é utilizado para ler a data e hora do concentrador. 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 30 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
• Tamanho: 02 (0x0002) 
• Índice: 11 (0x0B) 
• Estrutura do comando: 
>?CCCC0BKK 
➢ 0B [02]: Índice do comando.............................................................(hexadecimal) 
• Resposta: Esse comando retornará à informação completa de calendário do 
equipamento. 
• Tamanho: 16 (0x0010) 
• Estrutura da resposta: 
>!CCCC0BYYMMDDddHHNNSSKK 
➢ 0B [02]: Índice do comando.............................................................(hexadecimal) 
➢ Y [02]: Ano...............................................................................................(decimal) 
➢ M [02]: Mês.............................................................................................(decimal) 
➢ D [02]: Dia................................................................................................(decimal) 
➢ d [02]: Dia da semana (01=domingo, 02=segunda, ...).............................(decimal) 
➢ H [02]: Hora.............................................................................................(decimal) 
➢ N [02]: Minuto.........................................................................................(decimal) 
➢ S [02]: Segundos......................................................................................(decimal) 
• Exemplo: 
TX: >?00020B73 
RX: >!00100B1207270615492224 
3.7 Gerenciamento da automação 
3.7.1 Leitura de configurações 
Este comando é utilizado para ler a configuração de bicos presentes no equipamento. 
• Tamanho: 06 (0x0006) 
• Índice: 26 (0x1A) 
• Estrutura do comando: 
>?CCCC1AIIcbKK 
➢ 1A [02]: Índice do comando.............................................................(hexadecimal) 
➢ I [02]: ICOM (01-03).................................................................................(decimal) 
➢ c [01]: Conector (A-D)...........................................................................(caractere) 
➢ b [01]: Endereço (1-4)..............................................................................(decimal) 
• Resposta: Este comando retornará os dados de configuração da ICOM/ conector/ 
posição solicitados. 
• Tamanho: 52 (0x0034) 
• Estrutura da resposta: 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 31 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
>!CCCC1ATThhIIbbXYZAaannmmBbbzzxxCccuurrDddsswwReettppKK 
➢ 1A [02]: Índice do comando.............................................................(hexadecimal) 
➢ T [02]: Caractere de tipo de bomba (Consultar tabela abaixo).........(hexadecimal) 
➢ h [02]: Especificação do hardware...........................................................(decimal) 
➢ I [02]: Canal de comunicação (padrão CBC)(01-03)..................................(decimal) 
➢ b [02]: Endereço Lógico(01-04)...............................................................(decimal) 
➢ X [01]: Número de casas decimais do campo total..................................(decimal) 
➢ Y [01]: Número de casas decimais do campo volume..............................(decimal) 
➢ Z [01]: Número de casas decimais do campo preço unitário....................(decimal) 
➢ A [01]: Início de configurações estendidas da posição A.......................(caractere) 
➢ a [03]: Número atribuído ao bico da posição A........................................(decimal) 
➢ n [02]: Número do tanque atribuído ao bico da posição A.......................(decimal) 
➢ m [02]: Código do combustível atribuído ao bico da posição A................(decimal) 
➢ B [01]: Início de configurações estendidas da posição B.......................(caractere) 
➢ b [03]: Número atribuído ao bico da posição B........................................(decimal) 
➢ z [02]: Número do tanque atribuído ao bico da posição B........................(decimal) 
➢ x [02]: Código do combustível atribuído ao bico da posição B.................(decimal) 
➢ C [01]: Início de configurações estendidas da posição C........................(caractere) 
➢ c [03]: Número atribuído ao bico da posição C........................................(decimal) 
➢ u [02]: Número do tanque atribuído ao bico da posição C.......................(decimal) 
➢ r [02]: Código do combustível atribuído ao bico da posição C..................(decimal) 
➢ D [01]: Início de configurações estendidas da posição D.......................(caractere) 
➢ d [03]: Número atribuído ao bico da posição D........................................(decimal) 
➢ s [02]: Número do tanque atribuído ao bico da posição D.......................(decimal) 
➢ w [02]: Código do combustível atribuído ao bico da posição D................(decimal) 
➢ R [01]: Início de configurações estendidas de sensores IDF..................(caractere) 
➢ e [01]: Tipo de sensor.......................................................................(hexadecimal) 
➢ t [01]: Forma de trabalho do sensor.................................................(hexadecimal) 
➢ p [01]: Tempo do sensor..........................................................................(decimal) 
• Exemplo:  
TX: >?00061A01A14A 
RX: >!00341A07020101233A0010000B0000000C0000000D0000000R15012043 
3.7.2 Diagnóstico 
Este comando é utilizado para ler em tempo real o diagnóstico dos bicos configurados. Com 
este comando é possível descobrir se o bico está respondendo e qual o estado real da bomba. 
• Tamanho: 04 (0x0004) 
• Índice: 27 (0x1B) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 32 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
• Estrutura do comando:  
>?CCCC1BCCKK 
➢ 1B [02]: Índice do comando.............................................................(hexadecimal) 
➢ C [02]: Controle.....................................................................................(caractere) 
o 5A: Diagnóstico; 
o 5B: Bicos ICOM A; 
o 5C: Bicos ICOM B; 
o 5D: Bicos ICOM C; 
o 5E: Diagnóstico identfid. 
• Resposta: O comando retornará uma resposta variável de acordo com o controle que é 
passado para o comando. 
• Tamanho: Variável de acordo com o número de endereços configurados. 
• Estrutura da resposta para o parâmetro 5A: 
O comando retornará um conjunto de 5 caracteres para cada lado de bomba 
configurado na automação, um informando a situação da bomba, dois com status e dois 
com tipo de bomba configurada. 
>!CCCC1B5ASIITTKK 
➢ 1B [02]: Índice do comando.............................................................(hexadecimal) 
➢ 5A [02]: Controle..................................................................................(caractere) 
➢ S [01]: Situação do lado da bomba........................................................(caractere) 
o R: Respondendo; 
o F: Falha; 
o ?: Tipo desconhecido; 
o !: Tipo não autorizado; 
o N: Não configurado; 
o 0: Não existe bico. 
➢ I [02]: Status da bomba.........................................................................(caractere) 
➢ T [02]: Tipo da bomba......................................................................(hexadecimal) 
• Exemplo: 
TX: >?00041B5AEC 
RX: >!001D1B5AR2007F0007N0000N0000F00012A 
• Estrutura da resposta para os parâmetros 5B, 5C e 5D 
Este comando retornará o mapa de bicos o qual mostra o bico que está atrelado para 
cada lado da bomba. 
>!CCCC1BXXAABBCCDDKK (AABBCCDD bloco que se repete) 
➢ 1B [02]: Índice do comando.............................................................(hexadecimal) 
➢ X [02]: Controle (5B,5C,5D)...................................................................(caractere) 
➢ A [02]: Bico configurado na posição 0......................................................(decimal) 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 33 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
➢ B [02]: Bico configurado na posição 1......................................................(decimal) 
➢ C [02]: Bico configurado na posição 2......................................................(decimal) 
➢ D [02]: Bico configurado na posição 3......................................................(decimal) 
• Exemplo: 
TX: >?00041B5BED 
RX: >!00141B5B0121000002220000DA 
• Estrutura da resposta para o parâmetro 5E 
Este comando retornará o diagnóstico dos equipamentos de Identfid. 
>!CCCC1BXXDFFZZKK (DFFZZ bloco que se repete) 
➢ 1B [02]: Índice do comando.............................................................(hexadecimal) 
➢ X [02]: Controle (5E)..............................................................................(caractere) 
➢ D [01]: Status do equipamento............................................................(caractere) 
o 'N': Não configurado 
o 'R': Respondendo 
➢ F [02]: Status do protocolo HRS com o Identfid.......................................(decimal) 
➢ Z [02]: Tipo de sensor..............................................................................(decimal) 
• Exemplo:  
TX: >?00041B5EF0 
RX: >!000E1B5EF0015R003206 
3.7.3 Leitura de arquivos 
Este comando é utilizado para ler registros da memória da automação e retornar a 
informação em formato de arquivo CSV visando facilitar a exportação do mesmo. 
• Tamanho: 04 (0x0004) / 08 (0x0008) 
• Índice: 31 (0x1F) 
• Estrutura do comando: 
>?CCCC1FXXKK 
➢ 1F [02]: Índice do comando.............................................................(hexadecimal) 
➢ X [02]: Tipo de arquivo.............................................................................(decimal) 
o ‘01’: Configurações; 
o ‘02’: Abastecimentos; 
o ‘03’: Eventos; 
o ‘04’: Identificadores; 
o ‘05’: Bicos. 
• Resposta: Este comando retornará, linha por linha, todas as informações solicitadas 
separadas por “;”. 
• Tamanho: Variável. 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 34 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
OBS.: Caso o tipo passado seja “03”, pode ser adicionada a quantidade de eventos 
desejadas, após os caracteres “XX” do comando é adicionada a quantidade com 4 
caracteres, no entanto o tamanho do comando também mudará, ficando 08 (0x0008). 
• Exemplo de leitura de configurações: 
TX: >?00041F01DB 
RX:DD/MM/AA 
HH:NN;Nozzle;Protocol;ICom;Connector;Position;Mode;Hardware;Tank;Fuel;Price;To
tal Comma;Volume Comma;Identifier;Mode;Time 
;01;01: GILBARCO    ;1;B1;01;94;01:Loop High;00;00: NONE  ;1,000;2;3;00: NONE  ;0;0; 
;02;01: GILBARCO    ;1;B2;01;94;01:Loop High;00;00: NONE  ;2,000;2;3;00: NONE  ;0;0; 
#EndOfFile 
• Exemplo de leitura de abastecimentos: 
TX: >?00041F02DC 
RX: 
Registro;Bico;Combustivel;Tanque;Total;Volume;Preço;tempo;dia/mes/ano;Hora:min
;EncerInial;EncerFinal;ID Codigo1;ID Codigo2;VolTanque;Check 
0000;2;0;0;12,06;15,889;0,759;0009;04/05/20;10:41;00000009,34;00036022,63;000
0000000000000;0000000000000000;00000000;0 
0001;1;0;0;17,70;3,187;5,555;0006;04/05/20;10:43;00000129,72;00054043,02;0000
000000000000;0000000000000000;00000000;0 
0002;2;0;0;5,47;7,207;0,759;0009;04/05/20;10:44;00036022,63;00036029,84;00000
00000000000;0000000000000000;00000000;0 
.... 
4534;22;0;0;15,00;0,686;21,853;0063;22/05/20;09:22;36091872,82;36091873,50;00
00000000000000;0000000000000000;00000000;0 
#EndOfFile 
• Exemplo de leitura de 100 eventos: 
TX: >?00081F030100A2 
RX:  
000170;08.04;15/05/20;17:25:16;Pump Date Firmware version;_05_15_00_00_03; 
000171;08.04;15/05/20;17:25:19;Identifier Disconnected;_21_00_00_00_03; 
000172;08.04;15/05/20;17:26:14;Identifier Reconnected;_21_00_00_00_03; 
.... 
000269;08.04;22/05/20;09:22:09;Pump configuration change;_25_16_00_00_03; 
#EndOfFile 
• Exemplo de leitura de identificadores: 
TX: >?00041F04DE 
RX:  
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
08/03/2021 
Position;Card ID;Card Function;Versao;Controle;Codigo;Nome;Discount;CombCntrl 
000000;B3CF6CCFFF1FD792;27: 
CARD 
;11;FFFF;FFFFFFFF;B3CF6CCFFF1FD792              
#EndOfFile 
• Exemplo de leitura de bicos: 
TX: >?00041F05DF 
RX: 
Nozzle;Icom;Connector;Code 
01;1;B;:08 
02;1;B;:09 
#EndOfFile 
ATTENDANT 
;00,00;00000000; 
1 
L  
Atenção: Este comando sempre retornará a última linha com a seguinte 
descrição “#EndOfFile” indicando o final do arquivo. 
3.7.4 Leitura de índice do arquivo 
Este comando é utilizado para ler a informação indexada do arquivo desejado. 
• Tamanho: 10 (0x000A) 
• Índice: 39 (0x27) 
• Estrutura do comando: 
>?CCCC27TTIIIIIIKK 
➢ 27 [02]: Índice do comando.............................................................(hexadecimal) 
➢ T [02]: Tipo de arquivo.............................................................................(decimal) 
o '04': TAG Identificador. 
➢ I [06]: Índice da memória.........................................................................(decimal) 
• Resposta: Este comando retornará a linha solicitada em formato CSV. 
• Tamanho: 04 (0x0004) 
• Estrutura da resposta: 
>!CCCC27TT[CSV]KK 
➢ 27 [02]: Índice do comando.............................................................(hexadecimal) 
➢ T [02]: Tipo de arquivo.............................................................................(decimal) 
o '04': TAG Identificador. 
➢ [CSV] [Variável]: Informação solicitada em formato CSV, separado por “;”. 
• Exemplo: 
TX: >?000A2704000000FD 
RX: >!00742704000000; B328000000000001;27: CARD ATTENDANT 1 L; 11; FFFF; 
FFFFFFFF;B328000000000001;00,00;00000000;9F 
Página 35 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
3.7.5 Diagnóstico wireless 
08/03/2021 
Este comando é utilizado para ler, em tempo real, o diagnóstico por lado de bomba 
configurado no sistema wireless. Com este comando é possível descobrir a qualidade e potência 
de sinal sem fio. 
• Tamanho: 02 (0x0002) 
• Índice: 37 (0x25) 
• Estrutura do comando: 
>?CCCC25KK 
➢ 25 [02]: Índice do comando.............................................................(hexadecimal) 
• Resposta: Este comando retornará um conjunto de caracteres para cada bomba 
configurada na automação desde que esta esteja configurada como wireless, contendo 
sua situação, RSSI e LQI. 
• Tamanho: Variável de acordo com o número de endereços configurados 02 (0x0002) ... 
242 (0x00F2). 
• Estrutura da resposta: 
>!CCCC25SLRKK 
➢ 25 [02]: Índice do comando.............................................................(hexadecimal) 
➢ S [01]: Situação da bomba.....................................................................(caractere) 
o R: Respondendo; 
o F: Falha; 
o N: Não configurado. 
➢ L [01]: LQI [0...F]..............................................................................(hexadecimal) 
➢ R [01]: RSSI [0...F].............................................................................(hexadecimal) 
• Exemplo: 
TX: >?00022568 
RX: >!000525r0E34 
3.7.6 Leitura de porta(s) de comunicação utilizadas 
Este comando é utilizado para ler a(s) porta(s) de comunicação ethernet que estão sendo 
utilizadas na automação. 
• Tamanho:  04 (0x0004) 
• Índice: 28 (0x1C) 
• Estrutura do comando: 
>?CCCC1CD9KK 
➢ 1C [02]: Índice do comando.............................................................(hexadecimal) 
➢ D9 [02]: Controle do comando..............................................................(caractere) 
Página 36 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
08/03/2021 
• Resposta: Este comando retornará os números das portas ethernet que estão sendo 
utilizadas na automação (portas ocupadas), caso esteja mais de uma porta ocupada, a 
resposta terá a mesma estrutura para cada uma delas. 
• Tamanho: Variável. 
• Estrutura da resposta: 
>!CCCC1CrIII.III.III.IIIrRPPPPPrLLLLKK 
➢ 1C [02]: Índice do comando.............................................................(hexadecimal) 
➢ r [01]: Caractere separador...................................................................(caractere) 
➢ I [12]: IP do computador que realizou a solicitação.................................(decimal) 
➢ r [01]: Caractere separador..................................................................(caractere) 
➢ R [01]: Caractere identificador de porta remota...................................(caractere) 
➢ P [05]: Porta remota (porta IP do computador).......................................(decimal) 
➢ r [01]: Caractere separador..................................................................(caractere) 
➢ L [04]: Porta local da automação.............................................................(decimal) 
• Exemplo: 
TX: >?00041CD9F4 
RX: >!001C1C/192.168.0.173:R58806:L771E4 
4 Códigos de erro 
Os códigos descritos abaixo são utilizados por todas as respostas do protocolo que possam 
conter um erro, esses códigos são numerados de 0 a 16 (0x00 a 0x0F): 
• 0: Sucesso; 
• 1: Número de bico da posição A já utilizado; 
• 2: Número de bico da posição B já utilizado; 
• 3: Número de bico da posição C já utilizado; 
• 4: Número de bico da posição D já utilizado; 
• 5: Erro de resposta da bomba; 
• 6: Erro de timeout da bomba; 
• 7: Número de bico inexistente; 
• 8: Bico abastecendo; 
• 9: Modo inválido; 
• A: Identificador diferente; 
• B: Erro ao apagar identificador; 
• C: Erro parâmetro inválido; 
• D: Erro certificado inválido; 
• E: Erro comando inválido. 
Página 37 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
• F: Verificar se realmente há erro. 
5 Códigos de status de execução 
08/03/2021 
Os códigos descritos abaixo são utilizados por todas as respostas do protocolo que possam 
conter erros, informando a situação da execução do comando. 
• 0: Sucesso; 
• E: Erro. 
6 Códigos de tipos de combustíveis 
Código padrão utilizado para enumerar vários tipos de combustíveis disponíveis no mercado. 
• 01: Gasolina Comum; 
• 02: Gasolina Aditivada; 
• 03: Gasolina Premium; 
• 04: Gasolina Fórmula; 
• 05: Gasolina Podium; 
• 06: Gasolina Maxxi; 
• 07: Gasolina Original; 
• 08: Gasolina Garantida; 
• 09: Gasolina V-Power; 
• 10: Diesel; 
• 11: Diesel Aditivado; 
• 12: Diesel Verana; 
• 13: Diesel S50; 
7 Códigos de tipos de sensores 
• 14: Diesel Maxxi; 
• 15: Diesel Esp.; 
• 16: Querosene; 
• 17: GNV; 
• 18: Outro; 
• 19: Etanol; 
• 20: Óleo lubrificante; 
• 21: Óleo motor 15W40; 
• 22: Óleo hidr 10W30; 
• 23: Óleo hidr AW100; 
• 24: Óleo trans 85W140; 
• 25: Óleo trans 10W30. 
Utilizado no comando de configurações. Este código significa o modelo do sensor. 
• 15: Identfid; 
• 18: I-Button; 
• 32: Identfid duplo; 
• 2B: Identfid mestre escravo. 
Página 38 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
Protocolo de Comunicação 
Horustech 
DT 214 
Revisão: 09 
08/03/2021 
 
 
Página 39 de 41 \\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
 
8 Códigos de modelos de bombas 
Código padrão utilizado para enumerar vários tipos de bombas disponíveis no mercado.
• 01: Gilbarco; 
• 02: Wayne Igem 3G; 
• 03: MTB G-180; 
• 04: Wayne Minnow; 
• 05: Daruma; 
• 06: Milleniumm; 
• 07: Tokheim; 
• 08: Wayne Rifran; 
• 09: Stratema; 
• 0A: Booster Galileo; 
• 0B: Aspro ABL; 
• 0C: Eletrogas; 
• 0D: Galileo-PumpControl; 
• 0E: Aspro Develco; 
• 0F: Knox; 
• 10: Compac Agira; 
• 11: Nuovo Pignone; 
• 12: Realtek Metroval; 
• 13: Stratema Fiscal; 
• 14: Safe Graf; 
• 15: Identifid; 
• 16: Lectrocount; 
• 17: Wayne Duplex II; 
• 18: I-Button; 
• 19: Wayne Igem(3G)<32; 
• 1A: MCSH Mecânico; 
• 1B: Simulador; 
• 1C: Galileo Blocked; 
• 1D: Metroval CDM110; 
• 1E: Wireless reader; 
• 1F: Bico de óleo; 
• 20: Wayne DL1; 
• 21: Wayne IGEM Basic; 
• 22: Wayne IGEM P_BCD; 
• 23: Gilbarco Auto-DS; 
• 24: Gilbarco Stratema; 
• 25: Gilbarco Chinesa; 
• 26: Mecânica CBM; 
• 27: Simulador B&IDF; 
• 28: Mecânica Comboio; 
• 29: Dart Standard; 
• 2A: MTB G-180 Trunc; 
• 2B: Identfid_MS; 
• 2C: Gilbarco_Kraus; 
• 2D: Compac_T10; 
• 2E: Hong Yang; 
• 2F: Gilbarco_Petromecânica; 
• 30: Pump_Cntrl_Blkd_Prst; 
• 31: Tokheim TQC775; 
• 32: Identifid duplo; 
• 33: Tokheim PT Bilb; 
• 34: Identfid STR; 
• 35: Petrotec Gilb; 
• 36: Lanfeng; 
• 37: Wertco; 
• 38: Identfid Wertco; 
• 39: Bennet; 
• 3A: Bennet Horizon; 
• 3B: Aapro ABL Blked; 
• 3C: Bennet 96D. 
  
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
9 Códigos de forma de trabalho do sensor IDF 
08/03/2021 
Utilizado no comando de configurações, esse código representa a forma de trabalho do 
sensor Identfid. 
• 00: Desabilitado; 
• 01: Bomba de combustível; 
• 02: Acesso, enviando para o PC cartões não cadastrados; 
• 03: Acesso, ignorando cartões não cadastrados; 
• 04: Cartão ponto; 
• 05: Máquina de lavar. 
10 Cálculo de Checksum 
Somatório em hexadecimais dispostos em dois caracteres adicionados às linhas de comando 
e resposta do equipamento, esse somatório garante que os dados estejam íntegros ao serem 
recebidos nas extremidades de comunicação (PC ⇔ Automação). 
O cálculo do checksum HRS é realizado com um acumulador, onde são acrescidos os valores 
ASCII de cada caractere presente após o caractere de início de bloco nas linhas de comando e 
resposta. 
Para manter um tamanho fixo de dois caracteres, as casas mais significativas são removidas 
do resultado do somatório, até que seu tamanho preestabelecido seja respeitado. 
Página 40 de 41 
\\192.168.0.250\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais 
Protocolo de Comunicação 
DT 214 
Revisão: 09 
Horustech 
Companytec Automação e Controle Ltda. 
Av. Ferreira Viana, 1421 - Areal - 96080-000 - Pelotas - RS 
www.companytec.com.br 
Fone: (53) 3284-8129 
desenvolvimento@companytec.com.br 
08/03/2021 