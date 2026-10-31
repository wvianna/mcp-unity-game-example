# T3 — Obstáculos, geração e colisões

Implemente somente os obstáculos e a condição de derrota.

## Tipos
1. Obstáculo bloqueando uma faixa: deve ser evitado mudando de faixa.
2. Obstáculo baixo: deve ser ultrapassado com salto.
3. Obstáculo suspenso: deve ser evitado deslizando.

## Requisitos
- Criar prefabs reutilizáveis para os obstáculos.
- Criar um gerador que escolha faixas e tipos respeitando combinações possíveis.
- Deslocar obstáculos em direção ao jogador, mantendo coerência com a movimentação do cenário.
- Remover ou reciclar objetos que ultrapassarem a área de jogo.
- Detectar colisão e notificar o estado de fim de partida.
- Expor velocidade, intervalo de geração e distância mínima entre obstáculos no Inspector.
- Evitar sequências impossíveis de superar. Começar com padrões simples e seguros.
- Usar pooling se a geração repetida justificar; documentar a estratégia.

## Restrições
- Não implementar cristais, pontuação ou menus.
- Não aumentar dificuldade dinamicamente ainda.
- Não adicionar dependências externas.

## Aceite
- Os três tipos aparecem.
- É possível evitar cada tipo usando a ação adequada.
- Uma colisão válida encerra a partida uma única vez.
- Objetos antigos são removidos/reciclados.
- Não há erros de compilação.

Antes de alterar, informe os arquivos e objetos que pretende modificar. Ao concluir, separe testes executados automaticamente de testes que precisam ser feitos no Play Mode.
