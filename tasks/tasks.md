# Neon Rush 3D — quadro de tarefas

## Objetivo
Entregar um protótipo Unity 3D jogável de corrida infinita, com personagem robótico, três faixas, salto, deslize, obstáculos, cristais, pontuação e tela de fim de jogo.

## Estado
- [x] T0 — Diagnóstico do projeto e das ferramentas MCP
- [x] T1 — Cenário 3D, personagem provisório e câmera
  - Validado em Play Mode: frames avançando (`job wait` 340→346), console 0 erros/0 avisos, 2 divisórias ciano delimitando 3 faixas, robô na faixa central com contraste 1,38x e detalhe emissivo visível.
- [ ] T2 — Movimentação do jogador
  - Implementado e verificado: faixas (x 0 → -2,50 → +2,50, clamp na faixa 2), salto (ápice 1,37 e pouso em 0,03), deslize (timer 0,7, collider 0,90/0,45 → 1,80/0,90, robô achatado 0,50 → 1,00) e mapeamento de entrada (A/D/Espaço/Seta cima/S conferidos).
  - Pendente: confirmar com teclado físico no Play Mode. O dispositivo sintético do Editor (`FastKeyboard`) só expõe a tecla por alguns frames, então pressão real de tecla não pôde ser reproduzida de forma confiável pelo CLI.
- [ ] T3 — Obstáculos, geração e colisão
  - Implementado e verificado: 3 prefabs (Blocker 0,00–2,00 · Low 0,00–0,50 · High 1,15–2,25), ondas de 1–2 faixas (sempre sobra faixa livre), pooling estabilizado em 14 objetos por 24 s, fim de partida disparado organicamente em t≈12 s e exatamente 1 vez com 3 obstáculos simultâneos.
  - Matriz de sobreposição confirma que cada tipo exige a ação certa: Low só passa saltando, High só passa deslizando, Blocker só trocando de faixa.
  - Pendente: confirmar com teclado físico que saltar/deslizar de fato evita as colisões em tempo real.
- [ ] T4 — Cristais e pontuação
  - Implementado e verificado: prefab com rotação/flutuação, coleta por trigger contando uma única vez (+10 exato: 60→70), HUD sincronizado (`CRYSTALS 70`), pontuação zerada em nova sessão, pool estável (8→10, limitado) e coleta natural em partida (score 80).
  - Corrigido no teste: cristal era ciano igual às divisórias de faixa e só ocupava ~8 px na tela; aumentado para ~1,15 de altura e recolorido para verde saturado.
  - Pendente: confirmar visual humano da flutuação/rotação e ajustar valores de exposição se necessário.
- [ ] T5 — Progressão da dificuldade
  - Implementado e verificado: `DifficultyManager` como fonte única (velocidade 12→26 em 75 s; intervalo de obstáculos 1,60→0,85; cristais 1,90→1,15), spawners confirmados lendo os mesmos valores (13,90 == 13,90), saturação exata nos limites (`prog=1.000`), reinício restaurando o início e solidez mantida no máximo (0 ondas impossíveis em 96 amostras).
  - Tempo de sobrevivência exibido no HUD (`TIME 7,2`) e congelado após o fim de partida.
  - Pendente: avaliar se a rampa de 75 s é confortável com teclado físico.
- [ ] T6 — Interface, áudio e polimento
  - Implementado e verificado: menu inicial com NEON RUSH e botão PLAY, HUD (pontuação + tempo) só em partida, tela de GAME OVER com pontuação final, reinício limpo (pontuação, faixa, pool e dificuldade zerados) e entrada bloqueada fora da partida (`PlayerInput.Enabled=false`).
  - Polimento: flash verde na coleta (alpha 0,18), flash magenta no impacto (0,45) com tremor de câmera (desvio máx. 0,150 e retorno exato) e pulso no texto de pontuação.
  - Áudio: pontos de integração prontos (`GameFeedback` com campos de clipe para música, coleta, impacto, salto e deslize). **Nenhum arquivo de áudio foi adicionado** — o jogo roda em silêncio até os clipes serem fornecidos e arrastados no Inspector.
  - Pendente: teste com teclado físico e clique real nos botões (acionados por código no teste).
- [ ] T7 — Validação final

## Regras
- Executar uma tarefa por vez.
- Não iniciar a próxima tarefa enquanto os critérios da atual não forem testados.
- Registrar limitações do MCP e testes que dependam de intervenção humana.
- Fazer commit após cada etapa validada.

## Critérios globais de conclusão
- O projeto compila sem erros.
- A cena principal pode ser executada pelo Editor.
- O jogador consegue iniciar, jogar, perder e reiniciar.
- Não há dependência externa adicionada sem autorização.
- O relatório final distingue o que foi implementado, compilado e testado em execução.
