# T5 — Progressão da dificuldade

Implemente a progressão gradual da dificuldade.

## Requisitos
1. Aumentar gradualmente a velocidade do deslocamento do cenário/obstáculos.
2. Reduzir gradualmente o intervalo de geração, respeitando um limite mínimo seguro.
3. Definir velocidade inicial, velocidade máxima, duração da progressão e intervalo mínimo como parâmetros ajustáveis.
4. Evitar que a progressão gere sequências impossíveis.
5. Exibir tempo de sobrevivência, se isso puder ser integrado sem criar dependências desnecessárias.
6. Garantir que reiniciar a partida restaure os parâmetros iniciais.

Centralize a progressão em um componente próprio ou integre ao gerenciador existente se a arquitetura atual justificar. Evite duplicar estados de velocidade em vários scripts.

## Não fazer
- Não adicionar novos tipos de obstáculos.
- Não adicionar sistemas complexos de níveis ou upgrades.

## Aceite
- A dificuldade aumenta de maneira perceptível e gradual.
- Existe limite máximo de velocidade e limite mínimo de intervalo.
- O jogo permanece possível de jogar.
- Reiniciar restaura a dificuldade inicial.

Antes de editar, apresente o plano. Depois, informe os parâmetros criados e os testes realmente realizados.
