let mockTv = [
    { pacienteNome: "Pedro Henrique Silva", profissionalNome: "Dr. André Rodrigues", salaNome: "Sala Azul", dataHoraInicio: "2026-06-15T08:00:00", status: "Em Andamento" },
    { pacienteNome: "Lucas Gabriel Silva", profissionalNome: "Dra. Amanda Cavalcanti", salaNome: "Sala Verde", dataHoraInicio: "2026-06-15T09:00:00", status: "Confirmado" }
];

function renderizarPainel() {
    const agendamentos = mockTv;
    const divAtual = document.getElementById('em-atendimento');
    const divProximos = document.getElementById('proximos-atendimentos');

    divAtual.innerHTML = '';
    divProximos.innerHTML = '';

    agendamentos.forEach(ag => {
        const hora = new Date(ag.dataHoraInicio).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        const isAtual = ag.status === 'Em Andamento';

        const cardHtml = `
            <div class="card ${isAtual ? 'atual' : ''}">
                <div class="paciente">${ag.pacienteNome}</div>
                <div class="detalhes">${ag.profissionalNome} | ${ag.salaNome}</div>
                <div class="horario">${hora}</div>
            </div>
        `;

        if (isAtual) divAtual.innerHTML += cardHtml;
        else divProximos.innerHTML += cardHtml;
    });
}

setInterval(renderizarPainel, 10000);
renderizarPainel();