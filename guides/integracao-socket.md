# Integração Clube Seven — Fila via Socket.IO

Substituímos as chamadas HTTP da bridge por uma **fila persistida + Socket.IO**. O servidor agora emite comandos para a máquina (concentrador); a máquina executa e confirma.

## Conexão

- URL: `ws://<servidor-clube-seven>:8080` (mesma porta da API)
- Lib: `socket.io-client` (v4)
- **Auth no handshake** (obrigatório):

```js
const { io } = require("socket.io-client");
const socket = io("http://servidor:8080", {
  auth: { token: "<Token do posto>" } // mesmo Token de API já usado no header X-Api-Key/Authorization
});
```

- Token inválido/ausente → conexão recusada com erro `Acesso negado` / `Token inválido`.
- Cada posto entra automaticamente na sua sala (`machine:<postoId>`) — só recebe os comandos dele.

## Eventos

### Servidor → Máquina

**`queue:new`** — novo comando na fila. Payload = documento da fila:

```json
{
  "_id": "68b...",
  "action": "alterarPreco",
  "data": { "posto": "idDoPosto", "bico": "01", "price": "5490" },
  "status": "pending",
  "createdAt": "..."
}
```

Actions existentes:

| action | data | comportamento esperado |
|---|---|---|
| `alterarPreco` | `bico` ("01"), `price` (string, em milésimos, ex. "5490" = R$5,490) | alterar o preço do bico |
| `visualizarStream` | `bico`, `intervalMs` (default 200) | **ligar o stream de volume daquele bico**: começar a emitir `bico:volume` a cada `intervalMs` (ver Telemetria abaixo) por ~60s ou enquanto houver abastecimento |
| `checkBridge` | (vazio) | resposta de vida/health |

**`queue:sync`** — resposta ao pedido de sincronização (array de docs `pending`, mesmo formato acima).

### Máquina → Servidor

```js
// 1. ao RECEBER um queue:new — confirme imediatamente
socket.emit("queue:ack", { queueId: doc._id });

// 2. ao TERMINAR de executar o comando
socket.emit("queue:done", {
  queueId: doc._id,
  responseTime: 1234,              // ms (opcional)
  result: { volume: 12.34 }        // resultado do comando (opcional; p/ visualizarStream, volume final lido)
});
```

### Telemetria contínua — `bico:volume`

Enquanto o stream de um bico estiver ativo (acionado pela action `visualizarStream`), a máquina emite:

```js
setInterval(() => {
  socket.emit("bico:volume", { bico: "01", volumeLitros: 12.34, ts: Date.now() });
}, intervalMs); // ex.: 200ms
```

- `volumeLitros`: número (litros atuais no display do bico).
- `ts`: timestamp (ms ou s — o servidor normaliza).
- O servidor mantém esse valor em cache e o app lê dele (sem polling na bridge).
- Pare de emitir após ~60s sem atividade ou quando o abastecimento encerrar; se o servidor voltar a pedir `visualizarStream`, reinicie o stream.

## Reconexão / resiliência

- Todo comando fica salvo no servidor com `status: pending` até ser confirmado.
- **Ao conectar (e a cada reconexão)**, pedir o backlog:

```js
socket.on("connect", () => {
  socket.emit("queue:sync"); // servidor responde com "queue:sync" contendo os pendentes
});
```

- Fluxo de status no servidor: `pending` → (`queue:ack`) → `processing` → (`queue:done`) → `done`.

## Mínimo para funcionar

```js
let streamTimers = {};

socket.on("queue:new", async (doc) => {
  socket.emit("queue:ack", { queueId: doc._id });

  if (doc.action === "visualizarStream") {
    ligarStream(doc.data.bico, doc.data.intervalMs || 200); // não completa a queue — stream contínuo
  } else {
    await executar(doc.action, doc.data); // lógica atual dos endpoints HTTP
    socket.emit("queue:done", { queueId: doc._id, responseTime: ms });
  }
});

function ligarStream(bico, intervalMs) {
  clearInterval(streamTimers[bico]);
  streamTimers[bico] = setInterval(() => {
    socket.emit("bico:volume", { bico, volumeLitros: lerVolume(bico), ts: Date.now() });
  }, intervalMs);
  // encerrar após ~60s sem atividade (setTimeout → clearInterval + delete streamTimers[bico])
}
```

Dúvidas sobre o token do posto: mesmo fluxo de hoje (`Token` de API emitido pelo painel, campo `posto` define a máquina).
