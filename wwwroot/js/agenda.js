let mockAgendamentos = [
    { id: 1, pacienteNome: "Pedro Henrique Silva", responsavelNome: "Carlos Augusto Silva", profissionalNome: "Dr. André Rodrigues", salaNome: "Sala Azul - Integração Sensorial", dataHoraInicio: "2026-06-15T08:00:00", dataHoraFim: "2026-06-15T09:00:00", status: "Confirmado", observacoes: "Consulta mensal." },
    { id: 2, pacienteNome: "Lucas Gabriel Silva", responsavelNome: "Carlos Augusto Silva", profissionalNome: "Dra. Amanda Cavalcanti", salaNome: "Sala Verde - Atendimento Infantil", dataHoraInicio: "2026-06-15T09:00:00", dataHoraFim: "2026-06-15T10:00:00", status: "Em Andamento", observacoes: "Sessão semanal." }
];

async function carregarAgenda() {
    try {
        const agendamentos = mockAgendamentos;
        const container = document.getElementById('lista-agendamentos');
        container.innerHTML = '';

        agendamentos.forEach(ag => {
            const horaInicio = new Date(ag.dataHoraInicio).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
            const horaFim = new Date(ag.dataHoraFim).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

            const card = document.createElement('div');
            const classeStatus = ag.status === 'Em Andamento' ? 'em-andamento' : ag.status === 'Finalizado' ? 'finalizado' : '';

            card.className = `card ${classeStatus}`;
            card.innerHTML = `
                <div class="info">
                    <h3>Paciente: ${ag.pacienteNome}</h3>
                    <p><strong>Responsável:</strong> ${ag.responsavelNome}</p>
                    <p><strong>Profissional:</strong> ${ag.profissionalNome}</p>
                    <p><strong>Sala:</strong> ${ag.salaNome}</p>
                    ${ag.observacoes ? `<p><strong>Obs:</strong> ${ag.observacoes}</p>` : ''}
                    <span class="status-tag">${ag.status}</span>
                </div>
                <div class="horario">${horaInicio} - ${horaFim}</div>
            `;
            container.appendChild(card);
        });
    } catch (error) {
        console.error("Erro ao carregar agenda:", error);
    }
}

setInterval(carregarAgenda, 30000);
carregarAgenda();