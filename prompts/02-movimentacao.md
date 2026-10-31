# T2 — Movimentação do jogador

Implemente somente a movimentação do personagem do Neon Rush.

## Mecânica
- O personagem permanece aproximadamente na mesma posição longitudinal.
- A pista se deslocará em direção ao jogador nas etapas seguintes.
- O jogador troca entre três faixas.
- O personagem pode pular e deslizar.

## Controles desejados
- A ou seta esquerda: faixa à esquerda.
- D ou seta direita: faixa à direita.
- Espaço ou seta para cima: pular.
- S ou seta para baixo: deslizar.

## Requisitos técnicos
1. Verifique o sistema de entrada já configurado no projeto. Não altere o pacote ou configuração global sem necessidade.
2. Use `CharacterController` ou justifique uma alternativa.
3. Implemente transição lateral suave entre as posições das três faixas.
4. Implemente gravidade e salto com parâmetros ajustáveis.
5. Implemente deslize temporário, restaurando corretamente altura/centro do collider.
6. Evite que o jogador saia dos limites laterais.
7. Separe a lógica em componentes pequenos e documente parâmetros importantes.
8. Configure o componente no personagem pela Unity/MCP quando possível.

## Não fazer
- Não criar obstáculos nem cristais.
- Não implementar pontuação ou interface.
- Não reestruturar scripts não relacionados.

## Aceite
- Troca correta entre as três faixas.
- Movimento lateral suave.
- Salto e retorno ao chão.
- Deslize com duração definida e restauração do collider.
- O jogador não atravessa o chão nem sai da pista.

Apresente o plano antes de alterar. Depois, verifique compilação e descreva os testes que foram realmente executados. Se o teste de entrada exigir Play Mode manual, deixe isso explícito.
