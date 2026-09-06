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
| `checkBridge` | `bridgeUrl`, `bridgeKey` | testar a conexão local com a bridge. Ao concluir, emitir `queue:done` com `result: { ok: true }` ou `result: { ok: false, error: "motivo" }`. O servidor espera **no máx. 10s** — se não responder, o check falha |

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

- `volumeLitros`: **mesmo valor que o endpoint HTTP antigo `/visualizacao/stream` retornava em `bico.volumeLitros`** (o total em R$ do display — o app divide pelo preço para obter litros). Não converter para litros.
- `ts`: timestamp (ms ou s — o servidor normaliza).
- O servidor mantém esse valor em cache e o app lê dele (sem polling na bridge).
- Pare de emitir após ~60s sem atividade ou quando o abastecimento encerrar; se o servidor voltar a pedir `visualizarStream`, reinicie o stream.

## Reconexão / resiliência

- Todo comando fica salvo no servidor com `status: pending` até ser confirmado.
- **Ao conectar, o servidor já envia o backlog pendente** no evento `queue:sync` (mesmo sem a máquina pedir). Ao reconectar, a máquina também pode pedir:
  ```js
  socket.on("connect", () => {
    socket.emit("queue:sync"); // servidor responde com "queue:sync" contendo os pendentes
  });
  ```
- Por isso uma mesma queue pode chegar **duas vezes** (push do servidor + `queue:new`). Comandos devem ser idempotentes e/ou a máquina deve ignorar `queueId` já executado.
- **Conexões duplicadas são derrubadas**: o servidor mantém apenas a conexão mais recente por posto. Um disconnect com motivo `server namespace disconnect` significa que uma conexão nova assumiu (normal após queda silenciosa).
- Use **uma única instância** de `io(...)` com `reconnection: true` (padrão) — não crie instâncias novas em loops de retry.
- Fluxo de status no servidor: `pending` → (`queue:ack`) → `processing` → (`queue:done`) → `done`.

## Mínimo para funcionar

```js
let streamTimers = {};

socket.on("queue:new", async (doc) => {
  socket.emit("queue:ack", { queueId: doc._id });

  if (doc.action === "visualizarStream") {
    ligarStream(doc.data.bico, doc.data.intervalMs || 200); // não completa a queue — stream contínuo
    return;
  }

  try {
    await executar(doc.action, doc.data); // NUNCA deixe lançar: exceção sem catch derruba o processo (e a conexão)
    socket.emit("queue:done", { queueId: doc._id, responseTime: ms });
  } catch (err) {
    // falhou (ex.: bridge local offline) — reporte e continue vivo
    socket.emit("queue:done", { queueId: doc._id, result: { ok: false, error: String(err.message || err) } });
  }
});

socket.on("queue:sync", (queues) => {
  for (const doc of queues) socket.emit("queue:new", doc); // reprocessa pelo mesmo caminho acima
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
