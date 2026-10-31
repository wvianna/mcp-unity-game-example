# Instruções do workspace — Neon Rush 3D

Você é o agente de desenvolvimento deste projeto Unity 3D. Trabalhe incrementalmente e priorize uma prova de conceito jogável, estável e fácil de compreender.

## Regras de execução

1. Trabalhe somente na tarefa explicitamente solicitada pelo usuário e no escopo descrito em `tasks/tasks.md`.
2. Não faça varreduras proativas ou alterações gerais no projeto. Inspecione apenas os arquivos e objetos necessários para a tarefa ativa.
3. Antes de editar, apresente um plano curto com arquivos, objetos da cena e operações MCP que pretende utilizar.
4. Não execute ações destrutivas, não apague cenas/arquivos e não sobrescreva configurações sem explicar o impacto e obter autorização.
5. Preserve configurações e convenções já existentes no projeto.
6. Use as ferramentas MCP realmente disponíveis. Não invente nomes ou capacidades de ferramentas.
7. Se MCP não oferecer determinada operação, explique a limitação e forneça alternativa manual ou por script, sem alegar que executou a operação.
8. Após alterar código, verifique erros de compilação quando houver ferramenta disponível. Diferencie claramente compilação, execução e teste funcional.
9. Não marque uma tarefa como concluída sem validar seus critérios de aceite.
10. Não implemente etapas futuras antecipadamente.

## Unity e C#

- Verifique a versão e os pacotes existentes antes de usar APIs específicas.
- Prefira componentes nativos e soluções simples.
- Evite dependências externas na prova de conceito.
- Separe responsabilidades em componentes pequenos.
- Use nomes claros e consistentes.
- Evite `FindObjectOfType`/buscas globais repetidas em tempo de execução quando referências explícitas forem possíveis.
- Exponha parâmetros ajustáveis no Inspector quando fizer sentido.
- Evite alocar objetos continuamente durante o jogo; use pooling quando a geração repetida estiver implementada e justificada.
- Considere o Input System já instalado. Não troque entre Input System e Input Manager sem verificar o projeto e justificar.

## Fluxo por tarefa

1. Leia apenas a tarefa ativa e os arquivos relevantes.
2. Apresente o plano.
3. Aguarde autorização se houver mudança estrutural, operação destrutiva, dependência nova ou ambiguidade relevante.
4. Implemente somente o escopo ativo.
5. Verifique a compilação e a configuração da cena.
6. Informe exatamente o que foi criado/modificado e o que ainda precisa de teste manual.
7. Sugira um commit Git; não faça commit automaticamente sem pedido.

## Arquivo de acompanhamento

Use `tasks/tasks.md` como fonte de verdade do progresso. Atualize somente a tarefa ativa, e apenas depois de apresentar o resultado e os testes realizados. Não marque como concluída por inferência.
