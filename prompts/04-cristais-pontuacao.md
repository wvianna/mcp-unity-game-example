# T4 — Cristais e pontuação

Implemente os colecionáveis e a pontuação básica.

## Cristais
- Crie um prefab de cristal usando geometria simples.
- Use material emissivo, rotação e uma pequena animação de flutuação.
- Distribua cristais nas três faixas em posições alcançáveis.
- Detecte coleta sem gerar múltiplas contagens.
- Faça o cristal desaparecer ou retornar ao pool após coleta.

## Pontuação
- Cada cristal coletado vale 10 pontos.
- Exiba a pontuação atual durante a partida.
- Mantenha a lógica de pontuação separada da lógica do jogador e dos cristais.
- Garanta que a pontuação seja reiniciada no início de uma nova partida.

## Restrições
- Não implementar recordes persistentes nem bônus por distância nesta etapa.
- Não alterar o sistema de obstáculos, salvo integração estritamente necessária.

## Aceite
- O cristal é visível e coletável.
- Cada cristal incrementa exatamente 10 pontos uma única vez.
- A interface atualiza imediatamente.
- A pontuação reinicia corretamente.

Apresente o plano antes de modificar. Informe quais testes foram executados e quais dependem de Play Mode.
