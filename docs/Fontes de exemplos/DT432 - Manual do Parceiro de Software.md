








Manual    de
## Desenvolvimento
Manual    de
## Desenvolvimento
Manual    de
## Desenvolvimento

## DT400

Manual do
parceiro de
software
## DT432
Manual do parceiro de software



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 2 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- Introdução ................................................................................................................................ 3
1.1. Memória de Abastecimento ..................................................................................... 4
1.1.1. Leitura de Abastecimento e porta de conexão .............................................. 5
1.2. Status de Bombas ..................................................................................................... 6
1.3. Alteração de Preço.................................................................................................... 8
1.4. Visualização de Abastecimento ................................................................................ 8
1.5. Predeterminação de Valor ........................................................................................ 8
1.6. Comando de Modo ................................................................................................... 9
- Fluxo de Operação .................................................................................................................. 11
2.1. Fluxo de Operação com Bombas Livres .................................................................. 11
2.2. Fluxo de Operação com Bombas Bloqueadas ........................................................ 12
- Identfid® ................................................................................................................................. 14
3.1. Funcionamento ....................................................................................................... 14
3.2. Operações com Identfid® ....................................................................................... 15
3.2.1. Leitura de Abastecimento Identificado ........................................................ 15
3.2.2. Comando de Modo Identfid® ....................................................................... 16
3.3. Inclusão e exclusão de identificadores ................................................................... 16
3.4. Comando de modo para lista negra ....................................................................... 16
- Fluxo de Execução com Identfid® ............................................................................................ 17
4.1. Fluxo de Operação com Cartões Cadastrados ........................................................ 17
4.2. Fluxo de operação sem cartões cadastrados.......................................................... 18
- Horustech ................................................................................................................................ 20
5.1. Mapeamento dos bicos .......................................................................................... 20
5.2. Abastecimento ........................................................................................................ 21
5.3. Status ...................................................................................................................... 21
5.4. Considerações Finais ............................................................................................... 21





Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 3 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## 1. Introdução
A Companytec   é   nacional e   internacionalmente   conhecida   pela   sua   fabricação   de
concentradores  de  bombas  para  postos  de  combustíveis.  O  concentrador  é o  equipamento
intermediário entre bomba e PC, ou seja, ele lê todos os abastecimentos feitos pelas bombas de
combustíveis e os deixa “legíveis” para que o PC possa ler.
A Companytec possuí vários modelos de concentradores no mercado, desde os modelos CBC,
o Horustech  e  o  mais  atual  que é o modelo Concept,  este  último  funciona  também como
medidor de tanques e monitoramento ambiental.
A figura abaixo representa o fluxo de comunicação.

Figura 1 – Comunicação entre computador x concentrador x bombas
Como  informado  anteriormente,  o concentrador  tem  por  finalidade  controlar  os  dados  de
bombas de combustíveis e dispensadores de GNV monitorando, gerenciando e armazenando os
dados INDEPENDENTEMENTE do PC.
A comunicação com o concentrador pode ser feita de duas formas:
DLL Companytec - fornece diversas funções para comunicação com o equipamento em um
alto  nível  de  abstração,  facilitando  o  processo  de  conexão,  seja  via  serial  ou  ethernet.  Os
métodos   de   comunicação,   que   entregam   os   dados   recebidos   pela   automação   para   o
programador,  possuem  estruturas  pré-definidas. A  DLL foi  desenvolvida  usando  o  protocolo
CBC/Companytec usando a linguagem de programação Delphi.
Protocolo de comunicação nativo - outra solução para a comunicação com o concentrador
é  através  da  utilização  do protocolo  de  comunicação.  Possibilita  ao  desenvolvedor  escolher  a
melhor solução para comunicação serial e ethernet e permite que o programador trate os dados
da melhor forma para o seu sistema. Quando se trata de protocolo nativo, temos duas opções:
- Protocolo de comunicação CBC/Companytec: este protocolo funciona com qualquer
modelo  de  concentrador  da  Companytec,  desde  a CBC até  a  Concept  (mais  atual).
Quando haviam somente os modelos CBC no mercado, a Companytec desenvolveu
o   protocolo   CBC   para   a   comunicação   com   o   concetrador. Quando   houve   o
lançamento da Horustech, em meados do ano 2012, foi lançado um novo protocolo
(protocolo Horustech), só que o concentrador também deveria ter a compatibilidade



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 4 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

com o protocolo CBC, então foi lançado o protocolo Companytec, que nada mais é
que o protocolo CBC compatível com os modelos mais novos de concentrador. Um
exemplo mais comum é o comando de status (&S), na CBC a resposta era somente
com 8 canais de comunicação, limite da automação e com a Horustech essa resposta
vem com 12 canais de comunicação que é o total que tem o concentrador.
- Protocolo de comunicação Horustech: este protocolo é compatível com os modelos
Horustech  e  Concept e  tem  uma sintaxe diferente  e  mais  robusta  que  o  protocolo
CBC/Companytec. Ele também    é    o    protocolo    utilizado    para    personalizar
equipamentos TWC.
1.1. Memória de Abastecimento
O  concentrador Horustech tem  capacidade  para  armazenar  10  mil  abastecimentos  e
totalizadores e 08 mil eventos. Já o concentrador Concept, como tem um banco de dados, esse
número é bem maior, mas a forma como ele armazena os abastecimentos para que o PC possa
ler é a mesma forma que a Horustech. A memória de abastecimentos é armazenada de forma
circular, então se o abastecimento anterior foi 9999, o próximo abastecimento será armazenado
no endereço 0000.

Figura 2 – Ponteiros de abastecimentos
Dado  a  forma  como  foi  implementado  o  concentrador,  o  programador  do  sistema  não
necessita  preocupar-se  com  o  endereço  de  abastecimento  atual,  já  que  o  equipamento  faz  a
gerência dos endereços, facilitando a comunicação.
O  programador deve utilizar um dos comandos de leitura de abastecimento, retornando o
abastecimento  no  endereço  de  leitura.  O  retorno  desta  chamada  de  função  pode  ser  um
abastecimento válido ou não (um abastecimento é invalido quando o ponteiro de escrita é igual
ao  ponteiro  de  leitura).  Ao  utilizar  a  função  da  DLL  LeAbastecimento, por  exemplo, a  DLL  irá
tratar de fazer o incremento do ponteiro de leitura, como segue a imagem abaixo. Nem todas
as  funções  da  DLL  fazem  o  auto  incremento,  cheque  o  manual  ou  entre  em  contato  com  o
desenvolvimento da Companytec.



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 5 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais


Figura 3 – Ponteiro de leitura de abastecimento
No caso de o sistema utilizar o protocolo de comunicação nativo é necessário, após a leitura
do   abastecimento,   que   o   programa envie o   comando   de   incremento de   ponteiro ao
concentrador, para que o efeito seja o mesmo feito com a DLL.
1.1.1. Leitura de Abastecimento e porta de conexão
O concentrador tem a capacidade de conexão de múltiplos softwares fazendo requisições de
abastecimento   independentes.   Para   isso,   o   equipamento   possui   ponteiros   de   leitura
independentes  para  cada  porta  Ethernet  e  para  comunicação  serial.  No  diagrama  abaixo
suponha que três sistemas estão conectados no concentrador: dois pela Ethernet (um na porta
2001 e outro na porta 1771) e um conectado na porta serial.

Figura 4 – Ponteiro de leitura de abastecimentos x Portas




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 6 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

Supondo  agora  que  o  sistema  que  está  conectado  na ethernet  pela  porta  1771  faça  uma
leitura  de  abastecimento,  os  ponteiros  de  leitura  dos  outros  sistemas (portas) não  serão
alterados, mantendo a integridade dos dados.

Figura 5 – Exemplo de leitura de abastecimento
1.2. Status de Bombas
A informação de STATUS, presente no protocolo de comunicação Companytec, informa um
caractere para cada LADO DE BOMBA e não por bico. A imagem abaixo representa uma bomba
de dois lados: lado A (esquerda), contendo os códigos de bico 04 e 44 e lado B (direita), contendo
os códigos de bico 05 e 45 (códigos de bicos são atrelados ao bicos físicos das bombas, realizando
assim  um  mapeamento  interno).  Cada  lado  desta  bomba  é  representado  por  um  caractere
quando é feito a chamada do comando de status.

Figura 6 – Bombas de combustível




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 7 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

Vamos  supor  neste  exemplo,  para  fins  didáticos,  que  em  um  posto  só  temos  esta  bomba
configurada.  A  informação  para  o  comando  de  STATUS  deve  retornar,  quando  utilizado  o
protocolo de comunicação COMPANYTEC, no formato:
## (SXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXFFDDCVVVVMMMMPTT)
Cada caractere X, presente na resposta dada pelo concentrador, representa um estado entre
os citados abaixo:
- “L”: Livre, significa que o lado da bomba se encontra em modo de auto liberação,
indicando que a automação irá permitir abastecimentos sempre que solicitado;
- “A”: Abastecendo, informa que algum dos bicos do lado da bomba está em processo
de abastecimento;
- “C”:  Concluiu,  o  lado  da  bomba  está  em  processo  de  conclusão.  Esse  status  é
transitório,  ou  seja,  não  é  garantido  que  o  software  receberá  essa  informação,
portanto, não se deve depender de nenhuma situação para ler abastecimentos;
- “F”: Lado da bomba está em falha ou não configurado;
- “P”: Pronta para abastecer, informa que o lado da bomba já solicitou a autorização
para abastecer e já foi liberada;
- “B”:  Lado  da  bomba  bloqueado  para abastecimento. Caso o  bico  seja  retirado  do
descanso nesse estado, o mesmo irá para “E” de espera, indicando que a automação
necessita de uma liberação do software ou do Identfid® para abastecer;
- “E”: Esse estado significa que o bico pertencente a esse lado da bomba encontra-se
fora do descanso, aguardando a liberação para abastecer.
Logo teremos, em caso de nenhum abastecimento ocorrendo, o seguinte status:
## (SLLFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDCVVVVMMMMPTT)
Percebe-se que os dois primeiros caracteres estão livres, indicando os dois lados da bomba.
No  caso  de  abastecimento  em  um  dos  bicos  04  ou  44,  teremos  a  seguinte  resposta  do
concentrador:
## (SALFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDCVVVVMMMMPTT)
Não  temos  como  saber  qual  é  o  código  de  bico está  abastecendo através  do  comando  de
status. Esta informação somente aparecerá quando o abastecimento terminar e for feita uma
leitura do comando  de  abastecimento,  nesta  leitura  de  abastecimento, o  código  do  bico
responsável pelo abastecimento estará presente.
Já no protocolo Horustech, o status vem por BICO, então a resposta vem da seguinte forma:
## >!0A01AALB P AKK




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 8 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

1.3. Alteração de Preço
O comando de alteração de preço é enviado para um bico da bomba. Este comando tem por
finalidade alterar o preço unitário cobrado pelo produto em questão.
Atenção: A alteração do preço somente será visível para o frentista quando um novo
abastecimento for realizado.

1.4. Visualização de Abastecimento
O concentrador  permite  a  visualização  dos  abastecimentos  em  andamento  pelo  sistema
gerencial. A visualização é feita através do comando de visualização o qual informa para cada
bico abastecendo no momento, o código de bico mais o valor do abastecimento até o momento.
Caso esteja usando o protocolo Horustech, a visualização será pelo número do bico na pista.
A  automação  também  possuí  o  comando  de  visualização  identificada,  ou  seja,  é  possível
verificar,  no  momento  que  está  ocorrendo  o  abastecimento,  o  código  do  identificador  que
liberou o bico.
Atenção: Este é um comando de visualização e não necessariamente mostra o valor
ou volume que  está  na  bomba com  precisão,  já  que há  alguns  atrasos.  O atraso  da  bomba,  o
atraso  do  processamento do  concentrador, junto  ao atraso  do  sistema  gerencial  faz  com  que
este dado chegue desatualizado.

1.5. Predeterminação de Valor
O comando  de  Predeterminação  do  valor  permite  que  seja  passado  para  um  bico  X  o
valor/volume para ocorrer uma predeterminação. O abastecimento feito, após este comando,
irá parar quando alcançar este valor/volume.




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 9 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

1.6. Comando de Modo
O  controle de modo é uma ação sobre como as bombas devem se comportar. O  diagrama
abaixo demostra os possíveis modos e como se dá a troca de status das bombas. Algumas destas
trocas são possíveis com a utilização do comando de modo. Existem três estados iniciais para
um lado de bomba: livre, bloqueado ou em falha. A partir destes acontecem as transições.

Figura 7 – Status da bomba
- Livre → Bloqueado: Pode ser feito com a utilização do comando de Modo Bloqueio
ou com o uso do equipamento Identfid® quando configurado para o lado de bomba;
- Livre → Abastecendo: Quando é iniciado um abastecimento;
- Bloqueado → Livre: Utiliza-se o comando de Modo de Liberar ou desconfigurar o
## Identfid®;
- Bloqueado → Espera: O  bico  foi  retirado  pelo  frentista e  espera  autorização  do
sistema;
- Bloqueado → Pronto:  O  sistema  gerencial  envia  um  comando  de  autoriza
abastecimento ou  após  o  frentista  ter  passado  o  cartão  Identfid®  no  sensor  do
referido bico;
- Espera → Bloqueado: O bico é colocado de volta no descanso;
- Espera → Pronto:  O  sistema  autoriza  o  abastecimento ou  o  frentista  passou  o
cartão Identfid® no sensor do referido bico;
- Pronto → Abastecendo: O Frentista começa o abastecimento;



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 10 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- Abastecendo → Concluiu:  O  abastecimento  foi  finalizado. (Essa  troca  é  bem
rápida e, algumas vezes, pode passar despercebida pelo sistema e ir direto para o
estado de Livre ou Bloqueado);
- Abastecendo → Espera: É enviado o comando de Modo Pausa para o bico;
- Abastecendo → Bloqueado: Esta transição ocorre quando o comando de Modo
STOP  é  utilizado, o  abastecimento  para  e  o  frentista  precisa  colocar  o  bico  no
descanso novamente;
- Concluiu → Livre/Bloqueado:  Quando  o  abastecimento é  finalizado,  então  o
status do bico volta ao seu estado inicial;
- Qualquer Estado → Falha:  Ocorre  quando  há  perda  de  comunicação entre  a
bomba e o concentrador;
- Falha → Qualquer Estado: Ocorre  quando  a  comunicação  entre  a  bomba  e  o
concentrador retorna. O estado que o lado da bomba retornará é o que estava antes
de entrar em falha.




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 11 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- Fluxo de Operação
Neste capítulo serão apresentados os fluxos básicos de operação para cada tipo de sistema.
A comunicação entre o sistema e o concentrador deve ser focada na leitura de abastecimento.
Este é o comando mais importante.
2.1. Fluxo de Operação com Bombas Livres

Fluxo 1 – Fluxo de operação (Livre)
O primeiro fluxograma mostra a situação mais básica, em que um posto se encontra com as
bombas configuradas para funcionarem livremente. Logo, o sistema precisa somente registrar
os abastecimentos e seguir o fluxo de execução descrito acima.  O fluxo se resume basicamente
a um comando de leitura de  abastecimento e análise  do retorno. Caso o retorno seja a string
“(0)” (no caso de uso com o protocolo, equivale a não existência de um abastecimento para ser
lido), é feito a leitura do status das bombas, uma leitura de visualização (opcional) e o retorno
para  o início do  fluxo.  Caso  o  abastecimento  seja  válido,  é  feito  o  cálculo  do checksum e  a



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 12 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

verificação se este equivale  ao enviado pelo concentrador. Caso o checksum esteja correto, o
abastecimento  é  salvo  e  é  feito  o  incremento do  ponteiro  de  leitura (caso  utilize  a  DLL
Companytec, algumas funções já realizam o auto incremento) e retorno ao fluxo inicial.
2.2. Fluxo de Operação com Bombas Bloqueadas

Fluxo 2 – Fluxo de operação (Bloqueadas)



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 13 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

Quando as bombas do posto trabalham em modo bloqueado, o que é bom para que se possa
ter um controle sobre as bombas de combustível, temos o fluxo básico descrito acima, o qual
tem  o  mesmo  fluxo  anterior  acrescentado  da  verificação  do  status  das  bombas  testando  se
algum lado de bomba está em modo ESPERA. (Observe no diagrama de modo do capitulo 1, a
troca  de  estado  de  bloqueado  para  ESPERA).  Caso  algum  lado  de  bomba esteja  em  espera, o
caixa pode ou não liberar para a realização de um abastecimento. Autorizado, o abastecimento
é realizado, caso contrário, o fluxo retorna para o início.
Outro modo de controle pode ser adotado com a utilização da tecnologia Identfid® que será
apresentada  no  próximo  capítulo.  Apresentaremos  o  equipamento  Identfid®,  os  comandos
compatíveis, todas as funcionalidades e um fluxo básico de operação quando o posto utiliza o
equipamento.
Nota: Em caso de alguma dúvida por parte do desenvolvedor, recomenda-se o contado com
a equipe de desenvolvimento e suporte da COMPANYTEC.
E-mail: desenvolvimento@companytec.com.br
## Telefone: +55 (53) 3284-8129
WhatsApp: +55 (53) 99709-7581





Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 14 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## 3. Identfid®
O   Identfid®   é   uma   solução   que pode   ser integrada   ao   concentrador permitindo a
identificação  de  frentistas  e  clientes  do  posto  de  combustíveis,  utilizando  a  tecnologia  RFID.
Totalmente seguro, é um sistema de identificação de pessoas certificado pelo NCC na classe de
equipamentos   elétricos   para   atmosferas   explosivas   nas   condições de   gases   e   vapores
inflamáveis. Algumas das possibilidades com o equipamento Identfid® são:
- Identificar  funcionários  gerando  controle  de  caixa  individualizado,  assiduidade,
produtividade, etc.
- Criar campanhas de fidelização, concessões de crédito com controle total da carta
de clientes do posto;
- Manter  as  bombas  bloqueadas  e liberá-las  para  pessoas  autorizadas  via  software
gerencial;
- Controlar a bomba ou a máquina de lavagem de veículos;
- Fornecer cartões ou TAGs para fidelização de clientes;
- Armazenar mais de 16.000 identificadores.
## 3.1. Funcionamento
O  equipamento  Identfid®  é  conectado  a  cada  lado  da  bomba  no  posto  criando  mais  uma
camada  de  comunicação.  O  concentrador  que  se  comunicava  diretamente  com  as  bombas,
agora  vai  se  comunicar  com  o  equipamento  Identfid®  e  estes  farão  a  comunicação  com  as
bombas.

Figura 8 - Comunicação entre Computador x concentrador x Identfid x bombas

O  equipamento  funciona  em  conjunto  com  cartões  de  identificação  únicos.  Estes  cartões
devem  estar  cadastrados  no  concentrador  para  que,  quando  um  cartão  for  passado  no
equipamento Identfid® da bomba, o equipamento envie o código para o concentrador que irá
verificar  a  existência  do número do  cartão  e  se  ele  possui  permissão  para  liberar  um
abastecimento.  Caso  o  cartão  tenha  poder  para  liberar  a  bomba,  a mesma entra  em  modo
PRONTO, esperando que o frentista retire o bico e comece um abastecimento.



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 15 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais


Figura 8 – Status da bomba

Quando  o  cartão  é  passado  e o  mesmo está  cadastrado  no  concentrador  temos, a
representação da troca de estado “2”. Quando o cartão não está cadastrado no concentrador
temos a situação representada pela transição “1”, caso o bico esteja fora do descanso. O caso
em  que  o  sistema opta  por  não  armazenar os  cartões  no concentrador  será  abordado mais  a
frente, pois envolve um outro fluxo de operação.
3.2. Operações com Identfid®
Visto o controle sobre os abastecimentos, existem também comandos a serem aplicados que
retornam estas informações a fim de obter um controle sobre as operações no posto.
3.2.1. Leitura de Abastecimento Identificado
Como  as  bombas  serão  liberadas  somente  com  o  uso  de  cartões,  os  dados  do  número  do
Identfid® do frentista e do cliente podem ser anexados ao comando de abastecimento. Para que
o  comando  de  abastecimento  retorne  tal  informação,  podemos  usar  as seguintes funções do
protocolo de comunicação Companytec:
- Comando de Abastecimento Identificado;
- Comando de Abastecimento com dupla Identificação;
- Comando  de  Abastecimento  PAF1 e  PAF2,  estes  dois,  os  mais  utilizados  e  que
englobam mais informações que os demais comandos.
No  protocolo  de  comunicação  Horustech,  todos  os  comandos  retornam  os  dados  de
identificadores, caso sejam ou não utilizados.



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 16 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

3.2.2. Comando de Modo Identfid®
Além  dos  comandos  de  modo  para  gerenciar a  bomba,  explicados  anteriormente,  existem
também  os  comandos  de  modo  para  lidar  com  o  modo  de  funcionamento  do  equipamento
## Identfid®.
- Habilitar sensor de Identificação - o sensor quando configurado deve ser habilitado
para que a bomba passe a funcionar em modo Bloqueio;
- Desabilitar  sensor  de  Identificação – desabilitando  o  sensor,  a  bomba  passa  a
funcionar livremente indo para modo Livre.

3.3. Inclusão e exclusão de identificadores
A inclusão  de  cartões  de  identificação  pode  ser  feita  com  a  utilização  dos  programas  de
configuração  disponibilizados  pela  Companytec:  CBCManager, HRSConsole e  WebApp. Caso o
sistema queira ter esta funcionalidade, é possível através da utilização de comandos de inclusão
e exclusão de identificadores.
Existe também um comando que permite que todos os cartões configurados no concentrador
sejam apagados. O  comando de leitura de  identificação possibilita pesquisar um identificador
pelo índice na memória do concentrador. Este comando também retorna à quantidade de TAG
de identificação que estão armazenadas na memória.
Atenção: O  fabricante  do  componente  de  memória  presente  no  concentrador
Horustech afirma até 100 mil operações  de  escrita  em um mesmo endereço. Estes dados são
teóricos, logo é necessário cautela ao apagar o registro da memória. Para prolongar a vida útil
da memória deve-se utilizar o comando de modo para lista negra.

3.4. Comando de modo para lista negra
Para gerenciar os cartões cadastrados no concentrador, existe o comando de Modo para Lista
Negra. Com o comando é possível colocar um cartão (que ainda está configurado) em uma lista
que o impede de autorizar a bomba para o abastecimento. O comando de lista negra, além de
gerenciar os cartões permite que esta lista seja esvaziada.




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 17 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

- Fluxo de Execução com Identfid®
Neste capítulo serão apresentados dois fluxos de execução: um fluxo onde os cartões estão
cadastrados no concentrador e outro onde a aplicação irá gerenciar os cartões do posto.
4.1. Fluxo de Operação com Cartões Cadastrados

Fluxo 3 – Fluxo de operação com cartões cadastrados
Como pode ser observado na imagem acima, o fluxo não é muito diferente do apresentado
pelo  fluxo  padrão.  A  única  diferença  é  que  o  sistema  faz  uma  das  chamadas  de  leitura  de
abastecimento Identificado, mostrada neste documento.




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 18 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

4.2. Fluxo de operação sem cartões cadastrados
Antes de mostrarmos o fluxo de execução quando o sistema faz a gerencia dos cartões de
identificação, é necessário saber o que ocorre com um cartão quando ele não está cadastrado
no concentrador e é passado no equipamento Identfid®.
Quando um cartão não está registrado, o sistema verifica a possibilidade de autorizá-lo ou
não. Quando não reconhecido o cartão fica armazenado em uma região que contém cartões não
cadastrados.  Para  fazer  a  leitura  deste  cartão  é  necessário  utilizar  o  comando  de  leitura  de
Identificador (Protocolo). Este comando irá retornar o código do cartão para que o sistema possa
verificar se tem autorização para realizar o abastecimento, possui crédito, etc. Após esta leitura,
é   necessário  incrementar  o  ponteiro  de  leitura  do  mesmo  jeito  como  é  feito  com  os
abastecimentos. No comando de leitura de Identificador é possível visualizar também o código
de bico em que o cartão foi passado.
Se o cartão passado atende aos requisitos do sistema, então a liberação da bomba pode ser
feita  através  do  comando  de  predeterminação  identificado ou  somente  ser  enviado  um
comando autorizando o bico. Para este comando é passado o bico, o identificador, a autorização
da bomba “S”, o valor para fazer o preset e o tempo até retirar o bico, dentre outros parâmetros.
Atenção: Cuidado com o tempo ao retirar o bico. Muitos sistemas utilizam um tempo
baixo de 10 ou 20 segundos, o que pode acarretar na impossibilidade de o frentista realizar o
abastecimento já que após este tempo, a bomba irá retornar para o modo de bloqueio. Outro
ponto  a  ser  notado  é  que  o  comando  de predeterminação  tem  como último parâmetro  6
caracteres 0 (ZERO).




Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 19 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais


Fluxo 4 – Fluxo de operação sem cartões cadastrados



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 20 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

## 5. Horustech
Até o momento foram apresentados os conceitos básicos e os fundamentos de como é feito
a integração e o monitoramento de dados das bombas do posto através do concentrador. Nos
capítulos  anteriores a  comunicação  com  o  concentrador  baseava-se em utilizar  a  DLL  ou  o
protocolo de comunicação Companytec, estes desenvolvidos para o equipamento CBC.
O concentrador Horustech e o Concept possuem um outro protocolo de comunicação além
do CBC/Companytec. Esses equipamentos possuem diversas semelhanças com o equipamento
CBC, o que facilita a transição de um equipamento para o outro. Eles possuem a funcionalidade
de   emulação   de   protocolo   CBC,   logo   todo   sistema   que   atualmente   funciona em   um
concentrador CBC, irá funcionar perfeitamente em um concentrador Horustech.
Quando  o  sistema  é  desenvolvido  especificamente  para estes  concentradores, o  sistema
pode utilizar das funcionalidades e facilidades de um protocolo robusto que tem uma estrutura
predefinida com  uma  camada  de  transporte,  camada  de  dados  e  o checksum no  final  do
comando como segue legenda abaixo.
## >PCCCCX.... KK
- Camada de transporte:
## ➢ >: Delimitador;
➢ P: Tipo de comando:
o ?: Consulta para o concentrador;
o !: Resposta da automação.
➢ C[4]:  Tamanho  do  campo  DATA  em  hexadecimal  incluindo  o  índice  do
comando.
-  Camada de dados:
➢ X[2...65535]: Dados do comando:
o  Tipo[2]: Índice do comando;
o Parâmetros[0...65535]: Parâmetros auxiliares do comando.
## •  Checksum:
➢  K[2]:  Somatório  dos  valores  ASCII  de  todos  os  caracteres  do  comando,
desprezando o byte mais significativo.

5.1. Mapeamento dos bicos
O concentrador CBC utiliza para o mapeamento dos bicos do posto o conceito de código de
bico,  que  é  um  mapeamento  fixo  em  hardware  do  equipamento. O código  de  bico  se  dá  em
função  do  mapeamento  deste  para  um  canal,  endereço  e  posição.  Nos concentradores
Horustech e Concept temos o conceito de número de bico. A facilidade de trabalhar com número
de  bico  é  que  este  não  é  fixo,  logo o  mapeamento  dos  bicos  no  concentrador  pode  ser  feito
simetricamente  com  a  numeração  dos  bicos do  pista,  por  exemplo,  temos  no  posto  duas
bombas, uma  com  os  bicos  01,  02,  03, 04  e  outra  com  os  bicos  09, 10, 11  e 12.  Podemos



Manual do parceiro de software
## DT432
## Revisão: 02
## 22/07/2025

Página 21 de 22
\\SRV1-COMPANYTEC\Documentação SGQ\Publicações\Documento Técnico (DT)\Manuais

configurar exatamente estes números de bico no concentrador, para quando visualizarmos um
abastecimento com número de bico 09, sabemos que é o bico 09 da pista, que está realizando
um abastecimento.
## 5.2. Abastecimento
Os concentradores Horustech  e  Concept apresentam o  mesmo  modelo  de  memória  de
abastecimento, contendo um vetor circular com 10.000 registros de abastecimento. Assim como
no modelo anterior, eles possuem vários ponteiros de abastecimento, um para porta serial, três
por conexão ethernet, nas portas 857, 1771 e 2001, esta última indicada para usos com sistemas
de pista.
## 5.3. Status
Diferente  da  comunicação  com  a  DLL  e o  protocolo  de  comunicação  CBC, o  comando  de
status quando utilizado o protocolo de comunicação Horustech retorna à informação para cada
bico do posto.
## 5.4. Considerações Finais
O    concentrador    Horustech    possui,    como    já    mencionado    anteriormente,    diversas
características  que  foram  reutilizadas  na  sua concepção. O equipamento  funciona  com  os
mesmos modos e com as mesmas transições já mostradas nos capítulos anteriores.
O equipamento também possui total compatibilidade com o equipamento Identfid®, com os
produtos do  Sistema Wireless Companytec,  Terminal Wireless Companytec e  com  as  novas
tecnologias que ainda estão em desenvolvimento.







Companytec Automação e Controle Ltda.
Av. Ferreira Viana, 1421 - Areal - 96080-000 - Pelotas - RS
www.companytec.com.br
## Fone: (53) 3284-8129
desenvolvimento@companytec.com.br

